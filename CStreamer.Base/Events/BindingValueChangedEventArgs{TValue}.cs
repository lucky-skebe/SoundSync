// -----------------------------------------------------------------------
// <copyright file="BindingValueChangedEventArgs{TValue}.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.Events
{
    /// <summary>
    /// Describes a change in one of an elements properties.
    /// </summary>
    /// <typeparam name="TValue" >The Type contained in the Property.</typeparam>
    public class BindingValueChangedEventArgs<TValue> : BindingValueChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingValueChangedEventArgs{TValue}"/> class.
        /// </summary>
        /// <param name="newValue">The new value of the Property.</param>
        public BindingValueChangedEventArgs(TValue newValue)
            : base(newValue)
        {
            this.NewValue = newValue;
        }

        /// <summary>
        /// Gets the new value of the Property.
        /// </summary>
        /// <value>
        /// The new value of the Property.
        /// </value>
        public new TValue NewValue { get; }
    }
}
