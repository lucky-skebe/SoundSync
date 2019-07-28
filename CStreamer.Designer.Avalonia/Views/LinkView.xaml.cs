// -----------------------------------------------------------------------
// <copyright file="LinkView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;
    using global::Avalonia.Media;
    using ReactiveUI;

    public class LinkView : ReactiveUserControl<LinkViewModel>
    {
        private (Point, Point, Point, Point) points;

        public LinkView()
        {
            this.InitializeComponent();
        }

        public override void Render(DrawingContext context)
        {
            var pen = new Pen(new SolidColorBrush(Colors.Black));
            base.Render(context);
            var geometry = new PathGeometry();
            var pathFigure = new PathFigure { StartPoint = this.points.Item1, IsClosed = false };

            geometry.Figures.Add(pathFigure);

            pathFigure.Segments.Add(new BezierSegment
            {
                Point1 = this.points.Item2,
                Point2 = this.points.Item3,
                Point3 = this.points.Item4,
            });

            context?.DrawGeometry(new SolidColorBrush(), pen, geometry);
        }

        protected void Redraw((Point, Point, Point, Point) points)
        {
            this.points = points;
            this.InvalidateMeasure();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                var start = this.ViewModel.WhenAnyValue(vm => vm.Start);
                var control1 = this.ViewModel.WhenAnyValue(vm => vm.Control1);
                var control2 = this.ViewModel.WhenAnyValue(vm => vm.Control2);
                var end = this.ViewModel.WhenAnyValue(vm => vm.End);

                var tmp1 = start.CombineLatest(control1, (start, control1) => (start, control1));
                var tmp2 = control2.CombineLatest(end, (control2, end) => (control2, end));
                tmp1.CombineLatest(tmp2, (tmp1, tmp2) => (tmp1.start, tmp1.control1, tmp2.control2, tmp2.end)).Subscribe(this.Redraw).DisposeWith(disposables);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
