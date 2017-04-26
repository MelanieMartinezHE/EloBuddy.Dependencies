namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ControlList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection where T: Control
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Control> <RefList>k__BackingField;

        internal ControlList(ref List<Control> refList)
        {
            this.RefList = refList;
        }

        public void Add(T item)
        {
            this.RefList.Add(item);
        }

        public void Clear()
        {
            this.RefList.Clear();
        }

        public bool Contains(T item) => 
            this.RefList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.RefList.CopyTo(array, arrayIndex);
        }

        public int FindIndex(Predicate<T> match)
        {
            for (int i = 0; i < this.RefList.Count; i++)
            {
                if (match(this.RefList[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public IEnumerator<T> GetEnumerator() => 
            this.RefList.Cast<T>().GetEnumerator();

        public int IndexOf(T item) => 
            this.RefList.IndexOf(item);

        public void Insert(int index, T item)
        {
            this.RefList.Insert(index, item);
        }

        public bool Remove(T item) => 
            this.RefList.Remove(item);

        public void RemoveAt(int index)
        {
            this.RefList.RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        public int Count =>
            this.RefList.Count;

        public T this[int index]
        {
            get => 
                this.RefList[index];
            set
            {
                this.RefList[index] = value;
            }
        }

        internal List<Control> RefList { get; set; }

        bool ICollection<T>.IsReadOnly =>
            false;

        bool ICollection.IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IList.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

