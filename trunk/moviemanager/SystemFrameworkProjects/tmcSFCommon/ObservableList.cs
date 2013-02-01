using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Tmc.SystemFrameworks.Common
{
    public class ObservableList<T> : List<T>, INotifyCollectionChanged
    {
        public ObservableList(){ }
        public ObservableList(IEnumerable<T> subs) : base(subs) { }
        public ObservableList(int capacity) : base(capacity) { }

        public new void Add(T obj)
        {
            base.Add(obj);
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public new void AddRange(IEnumerable<T> obj)
        {
            base.AddRange(obj);
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public new void Insert(int index, T obj)
        {
            base.Insert(index, obj);
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public new void InsertRange(int index, IEnumerable<T> obj)
        {
            base.InsertRange(index, obj);
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public new void Remove(T obj)
        {
            base.Add(obj);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove);
        }


        public new int RemoveAll(Predicate<T> predicate)
        {
            int RetVal = base.RemoveAll(predicate);
            if (RetVal > 0)
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove);
            }
            return RetVal;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public new void Reverse()
        {
            base.Reverse();
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new void Sort()
        {
            base.Sort();
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new void Sort(IComparer<T> comparer)
        {
            base.Sort(comparer);
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            base.Sort(index, count, comparer);
            OnCollectionChanged(NotifyCollectionChangedAction.Move);
        }

        public new int Capacity
        {
            get { return base.Capacity; }
            set {
                base.Capacity = value;
                if (value < Count)
                {
                     OnCollectionChanged(NotifyCollectionChangedAction.Remove);
                }
            }
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
        }

           public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace);
            }
        }
        
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
