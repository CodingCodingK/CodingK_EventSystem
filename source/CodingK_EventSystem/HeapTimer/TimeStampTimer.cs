using CodingK_EventSystem.TimerBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CodingK_EventSystem.Heap;

namespace CodingK_EventSystem.HeapTimer
{
    /// <summary>
    /// 时间戳定时器，根据具体的时间点来作为执行的目标。
    /// 可以传入Delay，但也是用时间点为目标。
    /// 可以配合最小堆优先队列实现定时器。
    /// </summary>
    internal class TimeStampTimer : CodingKTimer
    {
        private const string tidLock = "TimeStampTimer_tidLock";

        private readonly DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private readonly HeapPriorityQueue<TimeStampTask> taskQueue;
        private readonly Thread timerThread;
        private readonly ConcurrentQueue<TimeStampTaskPack> packQue;
        private readonly bool setHandle;

        /// <summary>
        /// 时间戳定时器，根据具体的时间点来作为执行的目标。
        /// 可以传入Delay，但也是用时间点为目标。
        /// 可以配合最小堆优先队列实现定时器。
        /// </summary>
        /// <param name="interval">循环周期。如果为0或不传，就认为是外部做循环；如果不为0，就内部启用线程执行循环这个TickTimer</param>
        /// <param name="setHandle">是否使用Handle,默认为true</param>
        public TimeStampTimer(int interval = 0, bool setHandle = true, int capacity = 16)
        {
            taskQueue = new HeapPriorityQueue<TimeStampTask>(capacity);
            this.setHandle = setHandle;

            if (setHandle)
            {
                packQue = new ConcurrentQueue<TimeStampTaskPack>();
            }

            if (interval > 0)
            {
                // 使用新线程：
                timerThread = new Thread(new ThreadStart(StartTick));
                timerThread.Start();

                void StartTick()
                {
                    try
                    {
                        while (true)
                        {
                            UpdateTask();

                            Thread.Sleep(interval);
                        }
                    }
                    catch (ThreadAbortException e)
                    {
                        WarnFunc?.Invoke($"Tick Thread Abort:{e}");
                        throw;
                    }
                }
            }
        }

        public void HandleTask()
        {
            while (packQue != null && packQue.Count > 0)
            {
                if (packQue.TryDequeue(out TimeStampTaskPack pack))
                {
                    pack.cb.Invoke(pack.tid);
                }
                else
                {
                    ErrorFunc?.Invoke("packQue Dequeue data error.");
                }
            }
        }


        /// <summary>
        /// 可以在Unity的Mono中调用这个tick，那么就可以确定性的保证任务是Unity主线程调用的了。
        /// </summary>
        public void UpdateTask()
        {
            double nowTime = GetUtcMs();


            foreach (var item in taskQueue)
            {
                TimeStampTask task = item.Value;
                if (nowTime < task.destTime)
                {
                    continue;
                }

                ++task.loopIndex;

                if (task.count > 0)
                {
                    --task.count;
                    if (task.count == 0)
                    {
                        // 线程安全字典，遍历过程中删除无影响。
                        FinishTask(task.tid);
                    }
                    else
                    {
                        // task.destTime += task.delay; 避免浮点数累加误差，所以采用以下方式。
                        task.destTime = task.startTime + task.delay * (task.loopIndex);
                        CallTaskCB(task.tid, task.taskCB);
                    }
                }
                else
                {
                    task.destTime = task.startTime + task.delay * (task.loopIndex);
                    CallTaskCB(task.tid, task.taskCB);
                }
            }
        }

        void FinishTask(int tid)
        {
            if (taskQueue.TryRemove(tid, out TimeStampTask task))
            {
                CallTaskCB(tid, task.taskCB);
                task.taskCB = null;
            }
            else
            {
                WarnFunc?.Invoke($"KEY:{tid} remove failed when finished task.");
            }
        }

        void CallTaskCB(int tid, Action<int> taskCB)
        {
            if (setHandle)
            {
                packQue.Enqueue(new TimeStampTaskPack(tid, taskCB));
            }
            else
            {
                taskCB.Invoke(tid);
            }
        }


        private double GetUtcMs()
        {
            return GetMsByDateTime(DateTime.UtcNow);
        }

        private double GetMsByDateTime(DateTime dataTime)
        {
            TimeSpan ts = dataTime - startDateTime;
            return ts.TotalMilliseconds;
        }

        protected override int GenerateTid()
        {
            lock (tidLock)
            {
                while (true)
                {
                    ++m_tid;
                    if (m_tid == Int32.MaxValue)
                    {
                        m_tid = 0;
                    }

                    if (!taskQueue.ContainsKey(m_tid))
                    {
                        return m_tid;
                    }
                }
            }
        }


        #region API

        /// <summary>
        /// 根据时间点创建任务（推荐）
        /// </summary>
        /// <param name="firstFireTime">第一次执行的时间点</param>
        /// <param name="taskCB"></param>
        /// <param name="cancelCB"></param>
        /// <param name="delay">第一次执行时间点后，loop执行的延迟</param>
        /// <param name="count">如果为0就无限循环，>=1就是次数</param>
        /// <returns></returns>
        public int AddTask(DateTime firstFireTime, Action<int> taskCB, Action<int> cancelCB, uint delay = 0, int count = 1)
        {
            int tid = GenerateTid();
            double startTime = GetUtcMs();
            double firstDelay = GetMsByDateTime(firstFireTime);
            double destTime = startTime + firstDelay;
            TimeStampTask task = new TimeStampTask(tid, delay, count, destTime, taskCB, cancelCB);

            if (taskQueue.TryAdd(tid, task))
            {
                return tid;
            }
            else
            {
                WarnFunc?.Invoke($"KEY:{tid} already exist.");
                return -1;
            }
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="firstDelay">第一次执行的等待时长</param>
        /// <param name="delay"></param>
        /// <param name="taskCB"></param>
        /// <param name="cancelCB"></param>
        /// <param name="count">如果为0就无限循环，>=1就是次数</param>
        /// <returns></returns>
        public override int AddTask(uint firstDelay, Action<int> taskCB, Action<int> cancelCB, uint delay = 0, int count = 1)
        {
            int tid = GenerateTid();
            double startTime = GetUtcMs();
            double destTime = startTime + firstDelay;
            TimeStampTask task = new TimeStampTask(tid, delay, count, destTime, taskCB, cancelCB);

            if (taskQueue.TryAdd(tid, task))
            {
                return tid;
            }
            else
            {
                WarnFunc?.Invoke($"KEY:{tid} already exist.");
                return -1;
            }
        }

        public override bool DeleteTask(int tid)
        {
            if (taskQueue.TryRemove(tid, out TimeStampTask task))
            {
                if (setHandle && task.cancelCB != null)
                {
                    packQue.Enqueue(new TimeStampTaskPack(tid, task.cancelCB));
                }
                else
                {
                    task.cancelCB?.Invoke(tid);
                }
                return true;
            }
            else
            {
                WarnFunc?.Invoke($"KEY:{tid} remove failed when custom delete.");
                return false;
            }
        }

        public override void Reset()
        {
            if (packQue != null && !packQue.IsEmpty)
            {
                WarnFunc?.Invoke("Reset:packQue is not empty.");
            }

            taskQueue.Clear();
            if (timerThread != null)
            {
                timerThread.Abort();
            }
        }

        #endregion

        class TimeStampTaskPack
        {
            public int tid;
            public Action<int> cb;

            public TimeStampTaskPack(int tid, Action<int> cb)
            {
                this.tid = tid;
                this.cb = cb;
            }
        }


        class TimeStampTask : IComparable<TimeStampTask>
        {
            public int tid;

            /// <summary>
            /// 循环执行延迟时间
            /// </summary>
            public uint delay;

            /// <summary>
            /// 执行次数,初期值为0就是一直执行
            /// </summary>
            public int count;

            /// <summary>
            /// 下一次的执行时间: StartTime + Delay
            /// </summary>
            public double destTime;

            /// <summary>
            /// 开始时间
            /// </summary>
            public double startTime;

            /// <summary>
            /// 避免浮点数destTime累加出错
            /// </summary>
            public ulong loopIndex;

            public Action<int> taskCB;
            public Action<int> cancelCB;

            public TimeStampTask(
                int tid,
                uint delay,
                int count,
                double destTime, // 实际开始时间 = startTime + delay
                Action<int> taskCB,
                Action<int> cancelCB)
            {
                this.tid = tid;
                this.delay = delay;
                this.count = count;
                this.destTime = destTime;
                this.taskCB = taskCB;
                this.cancelCB = cancelCB;
                this.startTime = destTime;
                this.loopIndex = 0;
            }

            public int CompareTo(TimeStampTask other)
            {
                return this.destTime.CompareTo(other.destTime);
            }
        }
    }
}
