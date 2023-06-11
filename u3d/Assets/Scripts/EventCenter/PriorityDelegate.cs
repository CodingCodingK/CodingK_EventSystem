using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodingK_SystemCenter
{
    public class PriorityDelegate
    {
        public PriorityDelegate(Delegate cb, int priority = int.MaxValue)
        {
            CallBack = cb;
            Priority = priority;
        }
        
        public Delegate CallBack { get; set; }
        public int Priority { get; set; }
    }
    
    public class PriorityDelegateList
    { 
        public PriorityDelegateList()
        {
            _eventList = new LinkedList<PriorityDelegate>();
            Count = 0;
        }
        
        private readonly LinkedList<PriorityDelegate> _eventList;
        
        public int Count { get; private set; }

        public void Add(PriorityDelegate newNode)
        {
            var node = _eventList.First;
            while (node != null)
            {
                if (node.Value.Priority > newNode.Priority)
                {
                    break;
                }
                node = node.Next;
            }

            if (node != null)
            {
                _eventList.AddBefore(node, newNode);
            }
            else
            {
                _eventList.AddLast(newNode);
            }

            Count++;
        }
        
        public void Remove(PriorityDelegate targetNode)
        {
            if (_eventList.Remove(targetNode))
            {
                Count--;
            }
        }

        public void Fire()
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack cb)
                {
                    cb.Invoke();
                }
            }
        }
        
        public void Fire<T>(T param1)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack<T> cb)
                {
                    cb.Invoke(param1);
                }
            }
        }
        
        public void Fire<T1,T2>(T1 param1,T2 param2)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack<T1,T2> cb)
                {
                    cb.Invoke(param1, param2);
                }
            }
        }
        
        public void Fire<T1,T2,T3>(T1 param1,T2 param2, T3 param3)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack<T1,T2,T3> cb)
                {
                    cb.Invoke(param1, param2, param3);
                }
            }
        }
        
        public void Fire<T1,T2,T3,T4>(T1 param1,T2 param2, T3 param3, T4 param4)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack<T1,T2,T3,T4> cb)
                {
                    cb.Invoke(param1, param2, param3, param4);
                }
            }
        }
        
        public void Fire<T1,T2,T3,T4,T5>(T1 param1,T2 param2, T3 param3, T4 param4,T5 param5)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is CallBack<T1,T2,T3,T4, T5> cb)
                {
                    cb.Invoke(param1, param2, param3, param4, param5);
                }
            }
        }
    }
}