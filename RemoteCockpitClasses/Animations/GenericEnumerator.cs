using RemoteCockpitClasses.Animations.Actions;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class GenericEnumerator : IEnumerator
    {
        private object[] arr;
        private int position = -1;

        public GenericEnumerator(object[] list)
        {
            arr = list;
        }

        //public GenericEnumerator(AnimationXMLConverter list)
        //{
        //    arr = ConvertToArray(list);
        //}
        //public GenericEnumerator(AnimationXMLConverter list)
        //{
        //    arr = ConvertToArray(list);
        //}
        //public GenericEnumerator(ActionXMLConverter list)
        //{
        //    arr = ConvertToArray(list);
        //}

        //private T[] ConvertToArray<T>(IEnumerable<T> enm)
        //{
        //    List<T> items = new List<T>();
        //    foreach (var item in enm)
        //    {
        //        items.Add(item);
        //    }
        //    return items.ToArray();
        //}


        public bool MoveNext()
        {
            position++;
            return (position < arr.Length);
        }

        public void Reset()
        {
            position = -1;
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public object Current
        {
            get
            {
                try
                {
                    return arr[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}