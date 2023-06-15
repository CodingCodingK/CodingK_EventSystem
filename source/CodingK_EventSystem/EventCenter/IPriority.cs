using System;
using System.Collections.Generic;
using System.Text;

namespace CodingK_EventSystem.EventCenter
{
    public interface IPriority<T> : IComparable<IPriority<T>>
    {
        T Priority { get; set; }

        // default
        //public int CompareTo(IPriority<int> other)
        //{
        //    if (Priority < other.Priority)
        //    {
        //        return -1;
        //    }
        //    else if (Priority > other.Priority)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
    }

}
