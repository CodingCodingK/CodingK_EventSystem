using System;
using System.Collections.Generic;
using System.Text;

namespace CodingK_EventSystem.EventCenter
{
     /// <summary>
    /// 不依赖于UnityEngine
    /// 按照优先级进入
    /// callback默认加入队尾
    /// </summary>
    public class EventCenter<TEventType> where TEventType : Enum
    {
        // TODO 有空实现一个业务委托链完成后，自动衔接执行另一个委托链
        private static readonly Dictionary<TEventType, PriorityDelegateListAbstract> _eventDic =
            new Dictionary<TEventType, PriorityDelegateListAbstract>();
        
        private static void OnListenerAdding(TEventType eventType, PriorityDelegate callBack)
        {
            OnListenerAdding(eventType, callBack?.CallBack);
        }
        
        private static bool OnListenerAdding(TEventType eventType, Delegate callBack)
        {
            bool had = _eventDic.TryGetValue(eventType,out var d);
            if (d?.Count > 0 && d.First.CallBack.GetType() != callBack.GetType())
            {
                throw new Exception(
                    $"OnListenerAdding Error: Trying add EventType [{eventType}] delegate, need delegate type is [{d.First.CallBack?.GetType()}], adding delegate type is[{callBack.GetType()}]");
            }

            return had;
        }

        private static bool OnListenerRemoving(TEventType eventType)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d == null || d.Count < 1)
                {
                    OnListenerRemoved(eventType);
                    return false;
                }
            }

            return true;
        }
        
        // private static void OnListenerRemoving(EventType eventType, Delegate callBack)
        // {
        //     if (_eventDic.TryGetValue(eventType, out var d))
        //     {
        //         if (d == null || d.Count < 1)
        //         {
        //             throw new Exception($"OnListenerRemoving Error: Data Null or Empty, EventType Key = {eventType}");
        //         }
        //         else if (d.First.CallBack.GetType() != callBack.GetType())
        //         {
        //             throw new Exception(
        //                 $"ListenerRemoving Error: Trying remove EventType [{eventType}] key-value-pair, need value type is [{d.First?.CallBack?.GetType()}], removing value type is[{callBack.GetType()}]");
        //         }
        //     }
        //     else
        //     {
        //         throw new Exception($"OnListenerRemoving Error: No such EventType Key = {eventType}");
        //     }
        // }

        private static void OnListenerRemoved(TEventType eventType)
        {
            if (_eventDic[eventType] == null || _eventDic[eventType].Count < 1)
            {
                _eventDic.Remove(eventType);
            }
        }

        #region parameters Method

        
        //0 parameters
        public static void AddListener(TEventType eventType, Action callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList());
            
            ((PriorityDelegateList)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //1 parameters
        public static void AddListener<T>(TEventType eventType, Action<T> callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList<T>());
            
            ((PriorityDelegateList<T>)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //2 parameters
        public static void AddListener<T1,T2>(TEventType eventType, Action<T1,T2> callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList<T1,T2>());
            
            ((PriorityDelegateList<T1,T2>)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //3 parameters
        public static void AddListener<T1,T2,T3>(TEventType eventType, Action<T1,T2,T3> callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList<T1,T2,T3>());
            
            ((PriorityDelegateList<T1,T2,T3>)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //4 parameters
        public static void AddListener<T1,T2,T3,T4>(TEventType eventType, Action<T1,T2,T3,T4> callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList<T1,T2,T3,T4>());
            
            ((PriorityDelegateList<T1,T2,T3,T4>)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //5 parameters
        public static void AddListener<T1,T2,T3,T4,T5>(TEventType eventType, Action<T1,T2,T3,T4,T5> callBack, int priority = int.MaxValue, string name = null)
        {
            if(!OnListenerAdding(eventType, callBack))
                _eventDic.Add(eventType, new PriorityDelegateList<T1,T2,T3,T4,T5>());
            
            ((PriorityDelegateList<T1,T2,T3,T4,T5>)_eventDic[eventType]).Add(callBack, priority, name);
        }

        //each parameter
        public static bool RemoveListener(TEventType eventType, string cbName)
        {
            if (!OnListenerRemoving(eventType)) return false;
            bool isSuccess = _eventDic[eventType].Remove(cbName);
            OnListenerRemoved(eventType);
            return isSuccess;
        }

        //0 parameters
        public static void Trigger(TEventType eventType)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList list)
                {
                    list.Fire();
                }
            }
        }

        //1 parameters
        public static void Trigger<T>(TEventType eventType, T p1)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList<T> list)
                {
                    list.Fire(p1);
                }
            }
        }

        //2 parameters
        public static void Trigger<T1,T2>(TEventType eventType, T1 p1, T2 p2)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList<T1,T2> list)
                {
                    list.Fire(p1,p2);
                }
            }
        }

        //3 parameters
        public static void Trigger<T1,T2,T3>(TEventType eventType, T1 p1, T2 p2, T3 p3)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList<T1,T2,T3> list)
                {
                    list.Fire(p1,p2,p3);
                }
            }
        }

        //4 parameters
        public static void Trigger<T1,T2,T3,T4>(TEventType eventType, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList<T1,T2,T3,T4> list)
                {
                    list.Fire(p1,p2,p3,p4);
                }
            }
        }

        //5 parameters
        public static void Trigger<T1,T2,T3,T4,T5>(TEventType eventType, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            if (_eventDic.TryGetValue(eventType, out var d))
            {
                if (d is PriorityDelegateList<T1,T2,T3,T4,T5> list)
                {
                    list.Fire(p1,p2,p3,p4,p5);
                }
            }
        }
        

        #endregion
        
    }
}
