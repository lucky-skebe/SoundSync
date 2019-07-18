// -----------------------------------------------------------------------
// <copyright file="Graphical.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Base class for differnet Graphical Elements rendering a specific type of object.
    /// </summary>
    /// <typeparam name="TPipe">Thy type of object this class renders.</typeparam>
    public abstract class Graphical<TPipe> : IGraphical, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graphical{TPipe}"/> class.
        /// </summary>
        /// <param name="element">The object to render using this Class.</param>
        protected Graphical(TPipe element)
        {
            this.Id = Guid.NewGuid();
            this.Element = element;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public virtual double X { get; }

        /// <inheritdoc/>
        public virtual double Y { get; }

        /// <summary>
        /// Gets the object to render using this Class.
        /// </summary>
        /// <value>
        /// The object to render using this Class.
        /// </value>
        public TPipe Element { get; }

        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public abstract int ZIndex { get; }

        /// <inheritdoc/>
        public bool Equals(IGraphical other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        /// <summary>
        /// Fires a <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propName">The name of the changed property.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
