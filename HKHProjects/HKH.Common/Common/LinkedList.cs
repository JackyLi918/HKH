using System.Collections;
using System.Collections.Generic;

namespace HKH.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// public class LinkedEnumerable<T> : IEnumerable<T>, IEnumerable
    public class LinkedList<T> : IEnumerable<T>, IEnumerable
    {
        private List<IEnumerable<T>> enumerableList = null;

        public LinkedList(params IEnumerable<T>[] enumerables)
        {
            enumerableList = new List<IEnumerable<T>>();
            enumerableList.AddRange(enumerables);
        }

        public LinkedList(IList<IEnumerable<T>> enumerables)            
        {
            enumerableList = new List<IEnumerable<T>>();
            enumerableList.AddRange(enumerables);
        }

        #region Internal Members

        internal int Count { get { return enumerableList.Count; } }

        internal IEnumerable<T> this[int index] { get { return enumerableList[index]; } }

        #endregion

        public LinkedList<T> Add(params IEnumerable<T>[] enumerables)
        {
            enumerableList.AddRange(enumerables);
            return this;
        }

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
        private LinkedList<T> linkedEnumerable = null;

        internal LinkedEnumerator(LinkedList<T> linkedEnumerable)
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
