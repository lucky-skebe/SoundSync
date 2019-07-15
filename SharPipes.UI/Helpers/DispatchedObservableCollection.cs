// -----------------------------------------------------------------------
// <copyright file="DispatchedObservableCollection.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Threading;

    /// <summary>
    /// Wraps a <see cref="ObservableCollection{T}"/> to send all it's event onto a given <see cref="Dispatcher"/>.
    /// </summary>
    /// <typeparam name="T">The tyoe of the Items in the underlying Observablecollection.</typeparam>
    public abstract class DispatchedObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<T> collection;
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchedObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The underlyinf collection.</param>
        /// <param name="dispatcher">The to send the events on.</param>
        protected DispatchedObservableCollection(ObservableCollection<T> collection, Dispatcher dispatcher)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            this.dispatcher = dispatcher;

            this.collection.CollectionChanged += this.Collection_CollectionChanged;
        }

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc/>
        public int Count => this.collection.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public T this[int index] { get => this.collection[index]; set => this.collection[index] = value; }

        /// <inheritdoc/>
        public void Add(T item)
        {
            this.collection.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.collection.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.collection.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            return this.collection.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            this.collection.Insert(index, item);
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            return this.collection.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.collection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.dispatcher.Invoke(() =>
                {
                    this.CollectionChanged?.Invoke(this, e);
                });
            }
        }
    }
}
