using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Threading;

namespace SharPipes.UI.Helpers
{
    public abstract class DispatchedObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        private ObservableCollection<T> collection;
        private Dispatcher dispatcher;

        public DispatchedObservableCollection(ObservableCollection<T> collection, Dispatcher dispatcher)
        {
            this.collection = collection;
            this.dispatcher = dispatcher;

            this.collection.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(this.CollectionChanged != null)
            {
                this.dispatcher.Invoke(() =>
                {
                    if (this.CollectionChanged != null)
                    {
                        this.CollectionChanged(this, e);
                    }
                });
            }
        }

        public T this[int index] { get => this.collection[index]; set => this.collection[index]=value; }

        public int Count => this.collection.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            this.collection.Add(item);
        }

        public void Clear()
        {
            this.collection.Clear();
        }

        public bool Contains(T item)
        {
            return this.collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.collection.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.collection.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.collection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.collection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }
    }
}
