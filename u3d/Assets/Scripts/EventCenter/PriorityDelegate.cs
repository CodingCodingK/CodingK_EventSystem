using System;
using System.Collections.Generic;

namespace CodingK_SystemCenter
{
    public interface IPriority
    {
        int Priority { get; set; }
    }
    
    public abstract class PriorityDelegateAbstract : IPriority
    {
        protected PriorityDelegateAbstract(Delegate cb, int priority, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = cb.Method.Name; // 也许名字可以更复杂一点，以便唯一
            }

            CallBack = cb;
            Name = name;
            Priority = priority;
        }
        public Delegate CallBack { get; protected set; }
        
        public string Name { get; protected set; }
        
        public int Priority { get; set; }
    }
    
    public class PriorityDelegate : PriorityDelegateAbstract
    {
        internal PriorityDelegate(Action cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }
    
    public class PriorityDelegate<T> : PriorityDelegateAbstract
    {
        internal PriorityDelegate(Action<T> cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }
    
    public class PriorityDelegate<T1, T2> : PriorityDelegateAbstract
    {
        internal PriorityDelegate(Action<T1, T2> cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }
    
    public class PriorityDelegate<T1, T2, T3> : PriorityDelegateAbstract
    {
        internal PriorityDelegate(Action<T1, T2, T3> cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }
    
    public class PriorityDelegate<T1, T2, T3, T4> : PriorityDelegateAbstract
    {
        internal PriorityDelegate(Action<T1, T2, T3, T4> cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }
    
    public class PriorityDelegate<T1, T2, T3, T4, T5> : PriorityDelegateAbstract
    {
        internal PriorityDelegate(System.Action<T1, T2, T3, T4, T5> cb, int priority = int.MaxValue, string name = null) : base(cb, priority, name)
        {
        }
    }

    public abstract class PriorityDelegateListAbstract
    {
        protected PriorityDelegateListAbstract()
        {
            _eventList = new LinkedList<PriorityDelegateAbstract>();
            Count = 0;
        }
        
        protected readonly LinkedList<PriorityDelegateAbstract> _eventList;

        public PriorityDelegateAbstract First => _eventList.First?.Value;
        
        public int Count { get; private set; }

        protected void Add(PriorityDelegateAbstract newNode)
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

        public bool Remove(Delegate targetCb)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack != targetCb) continue;
                if (!_eventList.Remove(priorityDelegate)) break;
                Count--;
                return true;
            }
            return false;
        }
        
        public bool Remove(string targetName)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.Name != targetName) continue;
                if (!_eventList.Remove(priorityDelegate)) break;
                Count--;
                return true;
            }
            return false;
        }
        
        public bool Remove(string targetName, int priority)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.Priority != priority) continue;
                if (priorityDelegate.Name != targetName) continue;
                if (!_eventList.Remove(priorityDelegate)) break;
                Count--;
                return true;
            }
            return false;
        }
        
        public bool Remove(Delegate targetCb, int priority)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.Priority != priority) continue;
                if (priorityDelegate.CallBack != targetCb) continue;
                if (!_eventList.Remove(priorityDelegate)) break;
                Count--;
                return true;
            }
            return false;
        }
        
    }

    public class PriorityDelegateList : PriorityDelegateListAbstract
    {
        public void Add(Action cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate(cb, priority, name));
        }
        
        public void Fire()
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is Action cb)
                {
                    cb.Invoke();
                }
            }
        }
    }
    
    public class PriorityDelegateList<T> : PriorityDelegateListAbstract
    {
        public void Add(Action<T> cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate<T>(cb, priority, name));
        }
        
        public void Fire(T param1)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is Action<T> cb)
                {
                    cb.Invoke(param1);
                }
            }
        }
    }
    
    public class PriorityDelegateList<T1, T2> : PriorityDelegateListAbstract
    {
        public void Add(Action<T1, T2> cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate<T1, T2>(cb, priority, name));
        }
        
        public void Fire(T1 param1, T2 param2)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is Action<T1, T2> cb)
                {
                    cb.Invoke(param1, param2);
                }
            }
        }
    }
    
    public class PriorityDelegateList<T1, T2, T3> : PriorityDelegateListAbstract
    {
        public void Add(Action<T1, T2, T3>cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate<T1, T2, T3>(cb, priority, name));
        }
        
        public void Fire(T1 param1, T2 param2, T3 param3)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is Action<T1, T2, T3> cb)
                {
                    cb.Invoke(param1, param2, param3);
                }
            }
        }
    }
    
    public class PriorityDelegateList<T1, T2, T3, T4> : PriorityDelegateListAbstract
    {
        public void Add(Action<T1, T2, T3, T4>cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate<T1, T2, T3, T4>(cb, priority, name));
        }
        
        public void Fire(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is Action<T1, T2, T3, T4> cb)
                {
                    cb.Invoke(param1, param2, param3, param4);
                }
            }
        }
    }
    
    public class PriorityDelegateList<T1, T2, T3, T4, T5> : PriorityDelegateListAbstract
    {
        public void Add(System.Action<T1, T2, T3, T4, T5>cb, int priority = int.MaxValue, string name = null)
        {
            Add(new PriorityDelegate<T1, T2, T3, T4, T5>(cb, priority, name));
        }
        
        public void Fire(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            foreach (var priorityDelegate in _eventList)
            {
                if (priorityDelegate.CallBack is System.Action<T1, T2, T3, T4, T5> cb)
                {
                    cb.Invoke(param1, param2, param3, param4, param5);
                }
            }
        }
    }
}