using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HKH.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedEnumerable<T> : IEnumerable<T>, IEnumerable
    {
        private IList<IEnumerable<T>> enumerableList = null;

        public LinkedEnumerable(params IEnumerable<T>[] enumerables)
        {
            enumerableList = new List<IEnumerable<T>>();
            foreach (IEnumerable<T> enumerable in enumerables)
            {
                enumerableList.Add(enumerable);
            }
        }

        public LinkedEnumerable(IList<IEnumerable<T>> enumerables)
        {
            enumerableList = enumerables;
        }

        #region Internal Members

        internal int Count { get { return enumerableList.Count; } }

        internal IEnumerable<T> this[int index] { get { return enumerableList[index]; } }

        #endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
        {
            return new LinkedEnumerator<T>(this);
        }

        #endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class LinkedEnumerator<T> : IEnumerator<T>, IEnumerator
    {
        private IEnumerator<T> enumerator = null;
        private int enumerableIndex = -1;
        private LinkedEnumerable<T> linkedEnumerable = null;

        internal LinkedEnumerator(LinkedEnumerable<T> linkedEnumerable)
        {
            this.linkedEnumerable = linkedEnumerable;
        }

		#region IEnumerator<T> Members

		public T Current
        {
            get { return enumerator.Current; }
        }

        #endregion

		#region IEnumerator Members

		object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (enumerator == null)
                {
                    if (enumerableIndex < linkedEnumerable.Count - 1)
                    {
                        enumerableIndex++;
                        IEnumerable<T> nextEnumerable = linkedEnumerable[enumerableIndex];
                        if (nextEnumerable != null)
                            enumerator = nextEnumerable.GetEnumerator();
                    }
                    else
                    {
                        Reset();
                        return false; //We are all done.
                    }
                }
                else
                //Move to the next T in enumerator.
                {
                    if (enumerator.MoveNext())
                        return true;
                    else
                        CloseEnumerator();
                }
            }
        }

        public void Reset()
        {
            enumerableIndex = -1;
            CloseEnumerator(); ;
        }

        #endregion

		#region IDisposable Members

		public void Dispose()
        {
            CloseEnumerator();
        }

        #endregion

        private void CloseEnumerator()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
            }
        }
    }
}
