using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingK_SystemCenter
{
    public class EventCenter
    {
        // TODO 封装委托，让它带优先级，然后fire的时候把委托链按优先级排序再fire
        private static Dictionary<EventType, Delegate> _eventDic = new();
        
        private static void OnListenerAdding(EventType eventType, Delegate callBack)
        {
            if (!_eventDic.ContainsKey(eventType))
            {
                _eventDic.Add(eventType, null);
            }

            Delegate d = _eventDic[eventType];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(
                    $"OnListenerAdding Error: Trying add EventType [{eventType}] delegate, need delegate type is [{d.GetType()}], adding delegate type is[{callBack.GetType()}]");
            }
        }

        private static void OnListenerRemoving(EventType eventType, Delegate callBack)
        {
            if (_eventDic.ContainsKey(eventType))
            {
                Delegate d = _eventDic[eventType];
                if (d == null)
                {
                    throw new Exception($"OnListenerRemoving Error: Data Null, EventType Key = {eventType}");
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(
                        $"ListenerRemoving Error: Trying remove EventType [{eventType}] key-value-pair, need value type is [{d.GetType()}], removing value type is[{callBack.GetType()}]");
                }
            }
            else
            {
                throw new Exception($"OnListenerRemoving Error: No such EventType Key = {eventType}");
            }
        }

        private static void OnListenerRemoved(EventType eventType)
        {
            if (_eventDic[eventType] == null)
            {
                _eventDic.Remove(eventType);
            }
        }

        #region parameters Method

        
        //0 parameters
        public static void AddListener(EventType eventType, CallBack callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack) _eventDic[eventType] + callBack;
        }

        //1 parameters
        public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack<T>) _eventDic[eventType] + callBack;
        }

        //2 parameters
        public static void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X>) _eventDic[eventType] + callBack;
        }

        //3 parameters
        public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y>) _eventDic[eventType] + callBack;
        }

        //4 parameters
        public static void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y, Z>) _eventDic[eventType] + callBack;
        }

        //5 parameters
        public static void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerAdding(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y, Z, W>) _eventDic[eventType] + callBack;
        }

        //0 parameters
        public static void RemoveListener(EventType eventType, CallBack callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //1 parameters
        public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack<T>) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //2 parameters
        public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X>) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //3 parameters
        public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y>) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //4 parameters
        public static void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y, Z>) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //5 parameters
        public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            _eventDic[eventType] = (CallBack<T, X, Y, Z, W>) _eventDic[eventType] - callBack;
            OnListenerRemoved(eventType);
        }


        //0 parameters
        public static void TriggerEvent(EventType eventType)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d is CallBack callBack)
                {
                    callBack();
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }

        //1 parameters
        public static void TriggerEvent<T>(EventType eventType, T arg)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d is CallBack<T> callBack)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }

        //2 parameters
        public static void TriggerEvent<T, X>(EventType eventType, T arg1, X arg2)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d is CallBack<T, X> callBack)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }

        //3 parameters
        public static void TriggerEvent<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d as CallBack<T, X, Y> is { } callBack)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }

        //4 parameters
        public static void TriggerEvent<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d as CallBack<T, X, Y, Z> is { } callBack)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }

        //5 parameters
        public static void TriggerEvent<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (_eventDic.TryGetValue(eventType, out d))
            {
                if (d as CallBack<T, X, Y, Z, W> is { } callBack)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception($"TriggerEvent Error: EventType {eventType}");
                }
            }
        }
        

        #endregion
        
    }
}