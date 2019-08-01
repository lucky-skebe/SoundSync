// -----------------------------------------------------------------------
// <copyright file="DispatchedObservableCollectionConverter.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Threading;

    /// <summary>
    /// Wraps an <see cref="ObservableCollection{T}"/> into a <see cref="DispatchedObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The Type of the underlying Observable Collections items.</typeparam>
    public abstract class DispatchedObservableCollectionConverter<T> : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<T> collection)
            {
                return this.GetWrapper(collection, Dispatcher.CurrentDispatcher);
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a wrapper around.
        /// </summary>
        /// <param name="collection">The inner Collection that should be wrapped.</param>
        /// <param name="dispatcher">The dispatcher to send all Events to.</param>
        /// <returns>The wrapped List.</returns>
        protected abstract DispatchedObservableCollection<T> GetWrapper(ObservableCollection<T> collection, Dispatcher dispatcher);
    }
}
