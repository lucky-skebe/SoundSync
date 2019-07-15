// -----------------------------------------------------------------------
// <copyright file="GraphicalElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using System.Windows;
    using SharPipes.Pipes.Base;

    /// <summary>
    /// Draws an element of a specific pipeline.
    /// </summary>
    public class GraphicalElement : Graphical<IPipeElement>, IEquatable<GraphicalElement>
    {
        private readonly GraphicalPipeline pipeline;
        private Point position;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicalElement"/> class.
        /// </summary>
        /// <param name="element">The element that should be drawn.</param>
        /// <param name="position">The position where the element should be drawn.</param>
        /// <param name="pipeline">The pipline that should be notified of changes to the element.</param>
        public GraphicalElement(IPipeElement element, Point position, GraphicalPipeline pipeline)
            : base(element)
        {
            this.position = position;
            this.pipeline = pipeline;
        }

        /// <inheritdoc/>
        public override double X => this.position.X;

        /// <inheritdoc/>
        public override double Y => this.position.Y;

        /// <summary>
        /// Gets the name of the element Type.
        /// </summary>
        /// <value>
        /// The name of the element Type.
        /// </value>
        public string Name => this.Element.TypeName;

        /// <inheritdoc/>
        public override int ZIndex => (int)ZLayer.Elements;

        /// <summary>
        /// Move the element to given coordinate.
        /// </summary>
        /// <param name="position">the new position of the topleft corner of the element.</param>
        public void MoveTo(Point position)
        {
            this.position = position;
            this.OnPropertyChanged("X");
            this.OnPropertyChanged("Y");
        }

        /// <inheritdoc/>
        public bool Equals(GraphicalElement other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Element.Equals(other.Element);
        }

        internal void Delete()
        {
            this.pipeline.Remove(this);
        }
    }
}
