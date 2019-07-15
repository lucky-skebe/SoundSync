// -----------------------------------------------------------------------
// <copyright file="GraphicalSinkPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using SharPipes.Pipes.Base;

    /// <summary>
    /// Draws a single SinkPad of the given <see cref="Parent"/> <see cref="GraphicalElement"/>.
    /// </summary>
    public class GraphicalSinkPad : Graphical<IPipeSinkPad>
    {
        private int padIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicalSinkPad"/> class.
        /// </summary>
        /// <param name="element">The underlying SinkPad.</param>
        /// <param name="parent">The element his pad belongs to.</param>
        /// <param name="padIndex">The Index of this Pad used to calculate position relative to the element.</param>
        public GraphicalSinkPad(IPipeSinkPad element, GraphicalElement parent, int padIndex = 0)
            : base(element)
        {
            this.padIndex = padIndex;
            this.Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            this.Parent.PropertyChanged += this.Parent_PropertyChanged;
        }

        /// <summary>
        /// Gets the element his pad belongs to.
        /// </summary>
        /// <value>
        /// The element his pad belongs to.
        /// </value>
        public GraphicalElement Parent { get; }

        /// <inheritdoc/>
        public override double X => this.Parent.X - 5;

        /// <inheritdoc/>
        public override double Y => this.Parent.Y + (this.padIndex * 15) + 10;

        /// <inheritdoc/>
        public override int ZIndex => (int)ZLayer.Pads;

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X")
            {
                this.OnPropertyChanged("X");
            }
            else if (e.PropertyName == "Y")
            {
                this.OnPropertyChanged("Y");
            }
        }
    }
}
