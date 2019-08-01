// -----------------------------------------------------------------------
// <copyright file="GraphicalEdge.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Draws a Beziere curve between two grapical pads.
    /// </summary>
    public class GraphicalEdge : IGraphical, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicalEdge"/> class.
        /// </summary>
        /// <param name="src">The <see cref="GraphicalSinkPad"/> this link should start from.</param>
        /// <param name="sink">The <see cref="GraphicalSinkPad"/> this link should go to.</param>
        public GraphicalEdge(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            this.Src = src ?? throw new ArgumentNullException(nameof(src));
            this.Sink = sink ?? throw new ArgumentNullException(nameof(sink));

            this.Src.PropertyChanged += this.Dependant_PropertyChanged;
            this.Sink.PropertyChanged += this.Dependant_PropertyChanged;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public double X => Math.Min(this.Src.X, this.Sink.X);

        /// <inheritdoc/>
        public double Y => Math.Min(this.Src.Y, this.Sink.Y);

        /// <summary>
        /// Gets the Width.
        /// </summary>
        /// <value>
        /// The Width.
        /// </value>
        public double Width => Math.Max(1, Math.Abs(this.Sink.X - this.Src.X)) + 10;

        /// <summary>
        /// Gets the Height.
        /// </summary>
        /// <value>
        /// The Height.
        /// </value>
        public double Height => Math.Max(1, Math.Abs(this.Sink.Y - this.Src.Y)) + 10;

        /// <summary>
        /// Gets the startpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The startpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Start => new Point(this.Src.X - this.X + 5, this.Src.Y - this.Y + 5);

        /// <summary>
        /// Gets the endpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The endpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point End => new Point(this.Sink.X - this.X + 5, this.Sink.Y - this.Y + 5);

        /// <summary>
        /// Gets the first controlpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The first controlpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Control1 => this.Start + new Vector(Math.Max(this.Width / 2, 25), 0);

        /// <summary>
        /// Gets the second controlpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The second controlpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Control2 => this.End + new Vector(-Math.Max(this.Width / 2, 25), 0);

        /// <summary>
        /// Gets the <see cref="GraphicalSrcPad"/> this link should start from.
        /// </summary>
        /// <value>
        /// The <see cref="GraphicalSrcPad"/> this link should start from.
        /// </value>
        public GraphicalSrcPad Src { get; }

        /// <summary>
        /// Gets the <see cref="GraphicalSinkPad"/> this link should go to.
        /// </summary>
        /// <value>
        /// The <see cref="GraphicalSinkPad"/> this link should go to.
        /// </value>
        public GraphicalSinkPad Sink { get; }

        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public int ZIndex => ZLayer.Links;

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
        protected void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void Dependant_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X")
            {
                this.OnPropertyChanged("X");
                this.OnPropertyChanged("Width");
                this.OnPropertyChanged("Start");
                this.OnPropertyChanged("End");
                this.OnPropertyChanged("Control1");
                this.OnPropertyChanged("Control2");
            }
            else if (e.PropertyName == "Y")
            {
                this.OnPropertyChanged("Y");
                this.OnPropertyChanged("Height");
                this.OnPropertyChanged("Start");
                this.OnPropertyChanged("End");
                this.OnPropertyChanged("Control1");
                this.OnPropertyChanged("Control2");
            }
        }
    }
}
