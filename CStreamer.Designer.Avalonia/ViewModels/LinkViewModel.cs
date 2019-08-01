// -----------------------------------------------------------------------
// <copyright file="LinkViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Designer.Avalonia.Helper;
    using global::Avalonia;
    using ReactiveUI;

    internal class LinkViewModel : ViewModelBase, ICStreamerViewModel
    {
        private readonly ObservableAsPropertyHelper<double> x;
        private readonly ObservableAsPropertyHelper<double> y;
        private readonly ObservableAsPropertyHelper<double> width;
        private readonly ObservableAsPropertyHelper<double> height;
        private readonly ObservableAsPropertyHelper<Point> start;
        private readonly ObservableAsPropertyHelper<Point> control1;
        private readonly ObservableAsPropertyHelper<Point> control2;
        private readonly ObservableAsPropertyHelper<Point> end;

        public LinkViewModel(PadViewModel src, PadViewModel sink)
        {
            this.Src = src;
            this.Sink = sink;

            var srcX = src.WhenAnyValue(src => src.X);
            var srcY = src.WhenAnyValue(src => src.Y);
            var sinkX = sink.WhenAnyValue(sink => sink.X);
            var sinkY = sink.WhenAnyValue(sink => sink.Y);

            var x = srcX.CombineLatest(sinkX, Math.Min).Select(x => x - Settings.LinkBezierPadding);
            var y = srcY.CombineLatest(sinkY, Math.Min).Select(y => y - Settings.LinkBezierPadding);

            this.x = x.ToProperty(this, x => x.X);
            this.y = y.ToProperty(this, x => x.Y);

            var width = srcX.CombineLatest(sinkX, (srcX, sinkX) => Math.Max(1, Math.Abs(sinkX - srcX)) + (2 * Settings.LinkBezierPadding));
            var height = srcY.CombineLatest(sinkY, (srcY, sinkY) => Math.Max(1, Math.Abs(sinkY - srcY)) + (2 * Settings.LinkBezierPadding));

            this.width = width.ToProperty(this, pad => pad.Width);
            this.height = height.ToProperty(this, pad => pad.Height);

            var srcXOffset = x.CombineLatest(srcX, (x, srcX) => srcX + Settings.PadRadius - x).Where(offset => offset >= 0);
            var srcYOffset = y.CombineLatest(srcY, (y, srcY) => srcY + Settings.PadRadius - y).Where(offset => offset >= 0);
            var sinkXOffset = x.CombineLatest(sinkX, (x, sinkX) => sinkX + Settings.PadRadius - x).Where(offset => offset >= 0);
            var sinkYOffset = y.CombineLatest(sinkY, (y, sinkY) => sinkY + Settings.PadRadius - y).Where(offset => offset >= 0);

            var start = srcXOffset.CombineLatest(srcYOffset, (x, y) => new Point(x, y));
            var end = sinkXOffset.CombineLatest(sinkYOffset, (x, y) => new Point(x, y));

            this.start = start.ToProperty(this, pad => pad.Start);
            this.end = end.ToProperty(this, pad => pad.End);

            var control1 = start.CombineLatest(width, (start, width) =>
            {
                return start + new Vector(Math.Max(width / 2, 25), 0);
            });
            var control2 = end.CombineLatest(width, (end, width) =>
            {
                return end + new Vector(-Math.Max(width / 2, 25), 0);
            });

            this.control1 = control1.ToProperty(this, pad => pad.Control1);
            this.control2 = control2.ToProperty(this, pad => pad.Control2);

            this.WhenActivated((disposables) =>
            {
                this.x.DisposeWith(disposables);
                this.y.DisposeWith(disposables);
                this.width.DisposeWith(disposables);
                this.height.DisposeWith(disposables);
                this.start.DisposeWith(disposables);
                this.control1.DisposeWith(disposables);
                this.control2.DisposeWith(disposables);
                this.end.DisposeWith(disposables);
            });
        }

        public int ZIndex => ZLayer.Links;

        /// <inheritdoc/>
        public double X => this.x.Value;

        /// <inheritdoc/>
        public double Y => this.y.Value;

        /// <summary>
        /// Gets the Width.
        /// </summary>
        /// <value>
        /// The Width.
        /// </value>
        public double Width => this.width.Value;

        /// <summary>
        /// Gets the Height.
        /// </summary>
        /// <value>
        /// The Height.
        /// </value>
        public double Height => this.height.Value;

        /// <summary>
        /// Gets the startpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The startpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Start => this.start.Value;

        /// <summary>
        /// Gets the endpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The endpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point End => this.end.Value;

        /// <summary>
        /// Gets the first controlpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The first controlpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Control1 => this.control1.Value;

        /// <summary>
        /// Gets the second controlpoint of the Bezier Curve connecting two elements.
        /// </summary>
        /// <value>
        /// The second controlpoint of the Bezier Curve connecting two elements.
        /// Relative to the position defined by <see cref="X"/> and <see cref="Y"/>.
        /// </value>
        public Point Control2 => this.control2.Value;

        internal PadViewModel Src { get; }

        internal PadViewModel Sink { get; }
    }
}
