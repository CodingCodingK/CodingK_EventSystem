﻿using System;
using System.Threading.Tasks;
using PEUtils;
using CodingK_EventSystem;
using CodingK_EventSystem.HeapTimer;
using System.Diagnostics;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PELog.InitSettings();
            PELog.ColorLog(LogColor.Green, "test Starting...");

            TimeStampTimerExample(false, false);

            Console.ReadKey();
        }

        static void TimeStampTimerExample(bool useHandle, bool outUpdate)
        {
            int timerInternal = outUpdate ? 0 : 10;
            uint interval = 66;
            int count = 100;
            int sum = 0;
            int taskId = 0;
            uint firstDelay = 1000;

            // 统计回调执行偏差时间
            DateTime historyTime;
            void taskCB(int tid)
            {
                DateTime nowTime = DateTime.UtcNow;
                TimeSpan ts = nowTime - historyTime;
                historyTime = nowTime;
                int delta = (int)(ts.TotalMilliseconds - interval);
                PELog.ColorLog(LogColor.Yellow, $"间隔差:{delta} ms");
                sum += Math.Abs(delta);
                PELog.ColorLog(LogColor.Magenta, "tid:{0} working.", tid);
            };

            void cancelCB(int tid)
            {
                PELog.ColorLog(LogColor.Magenta, "tid:{0} canceled.", tid);
            };

            TimeStampTimer timer = new TimeStampTimer(timerInternal, useHandle)
            {
                LogFunc = PELog.Log,
                WarnFunc = PELog.Warn,
                ErrorFunc = PELog.Error,
            };

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                historyTime = DateTime.UtcNow.AddMilliseconds(firstDelay - interval);
                Console.WriteLine(historyTime);
                Console.WriteLine(DateTime.UtcNow);
                taskId = timer.AddTask(historyTime, taskCB, cancelCB, interval, count);
            });

            if (useHandle || outUpdate)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    PELog.Log("Handle Start.");
                    while (true)
                    {
                        if (outUpdate)
                        {
                            timer.UpdateTask();
                        }

                        if (useHandle)
                        {
                            timer.HandleTask();
                        }

                        await Task.Delay(2);
                    }
                });
            }


            while (true)
            {
                if (Console.ReadLine() == "calc")
                {
                    PELog.ColorLog(LogColor.Red, "平均间隔：" + sum * 1.0f / count + " ms");
                }
                else if (Console.ReadLine() == "del")
                {
                    timer.DeleteTask(taskId);
                }
            }
        }
    }
}
