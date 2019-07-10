using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class NotifyCollectionWrapper<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly IList<T> inner = new List<T>();

        public NotifyCollectionWrapper()
        {
            this.CollectionChanged += Inner_CollectionChanged;
        }

        private void Inner_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"Action: {e.Action}, #New Items: {e.NewItems?.Count}, #Old Items: {e.OldItems?.Count}, NewStartIndex: {e.NewStartingIndex}, OldStartIndex: {e.OldStartingIndex}");
        }

        public T this[int index] { get => this.inner[index]; set => this[index] = value; }

        public int Count => this.inner.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            this.inner.Add(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if(this.CollectionChanged != null)
            {
                this.CollectionChanged(this, notifyCollectionChangedEventArgs);
            }
        }

        public void Clear()
        {
            this.inner.Clear();
        }

        public bool Contains(T item)
        {
            return this.inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.inner.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.inner.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.inner.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.inner.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }
    }
}
