using System;
using System.Collections.Generic;
using System.Text;

namespace CodingK_EventSystem.Heap
{
    /// <summary>
    /// 小顶堆优先队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HeapPriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _list;
        public int Count => _list.Count;
        public HeapPriorityQueue(int capacity = 16)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// 入队
        /// </summary>
        public void Enqueue(T item)
        {
            _list.Add(item);

            HeapifyUp(_list.Count - 1);
        }

        /// <summary>
        /// 出队
        /// </summary>
        public T Dequeue()
        {
            if (_list.Count == 0)
            {
                return default(T);
            }
            T item = _list[0];
            int endIndex = _list.Count - 1;
            _list[0] = _list[endIndex];
            _list.RemoveAt(endIndex);
            --endIndex;
            HeapifyDown(0, endIndex);

            return item;
        }

        public T Peek()
        {
            return _list.Count > 0 ? _list[0] : default(T);
        }

        /// <summary>
        /// TODO 可以做查找优化到logn
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int IndexOf(T t)
        {
            return _list.IndexOf(t);
        }

        public T RemoveAt(int rmvIndex)
        {
            if (_list.Count <= rmvIndex)
            {
                return default(T);
            }
            T item = _list[rmvIndex];
            int endIndex = _list.Count - 1;
            _list[rmvIndex] = _list[endIndex];
            _list.RemoveAt(endIndex);
            --endIndex;

            if (rmvIndex < endIndex)
            {
                int parentIndex = (rmvIndex - 1) / 2;
                if (parentIndex > 0 && _list[rmvIndex].CompareTo(_list[parentIndex]) < 0)
                {
                    HeapifyUp(rmvIndex);
                }
                else
                {
                    HeapifyDown(rmvIndex, endIndex);
                }
            }

            return item;
        }

        public T RemoveItem(T t)
        {
            int index = IndexOf(t);
            return index != -1 ? RemoveAt(index) : default(T);
        }

        public void Clear()
        {
            _list.Clear();
        }
        public bool Contains(T t)
        {
            return _list.Contains(t);
        }
        public bool IsEmpty()
        {
            return _list.Count == 0;
        }
        public List<T> ToList()
        {
            return _list;
        }
        public T[] ToArray()
        {
            return _list.ToArray();
        }

        // 节点往上堆化
        void HeapifyUp(int childIndex)
        {
            int parentIndex = (childIndex - 1) / 2;
            while (childIndex > 0 && _list[childIndex].CompareTo(_list[parentIndex]) < 0)
            {
                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
                parentIndex = (childIndex - 1) / 2;
            }
        }

        // 节点往下堆化
        void HeapifyDown(int topIndex, int endIndex)
        {
            while (true)
            {
                int minIndex = topIndex;
                int childIndex = topIndex * 2 + 1;
                if (childIndex <= endIndex && _list[childIndex].CompareTo(_list[topIndex]) < 0)
                    minIndex = childIndex;
                childIndex = topIndex * 2 + 2;
                if (childIndex <= endIndex && _list[childIndex].CompareTo(_list[minIndex]) < 0)
                    minIndex = childIndex;
                if (topIndex == minIndex) break;
                Swap(topIndex, minIndex);
                topIndex = minIndex;
            }
        }

        private void Swap(int a, int b)
        {
            (_list[a], _list[b]) = (_list[b], _list[a]);
        }
    }
}
