using Avalonia;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace CStreamer.Designer.Avalonia.Views
{
    public class LinkView : ReactiveUserControl<LinkViewModel>
    {
        public LinkView()
        {
            Path p;
            this.InitializeComponent();
        }

        (Point, Point, Point, Point) points;

        public override void Render(DrawingContext context)
        {
            var pen = new Pen(new SolidColorBrush(Colors.Black));
            base.Render(context);
            var geometry = new PathGeometry();
            var pathFigure = new PathFigure { StartPoint = this.points.Item1, IsClosed= false };

            geometry.Figures.Add(pathFigure);

            pathFigure.Segments.Add(new BezierSegment { Point1 = this.points.Item2,
                Point2 = this.points.Item3,
                Point3 = this.points.Item4 });

            context.DrawGeometry(new SolidColorBrush(), pen, geometry);
        }

        void Redraw((Point, Point, Point, Point) points)
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
                tmp1.CombineLatest(tmp2, (tmp1, tmp2) => (tmp1.start, tmp1.control1, tmp2.control2, tmp2.end)).Subscribe(Redraw).DisposeWith(disposables);

            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
