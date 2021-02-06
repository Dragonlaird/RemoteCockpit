using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteCockpitClasses
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> list, object item)
        {
            return list.ToList().IndexOf(item);
        }
    }
}
