using CStreamer.Designer.Avalonia.Helper;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public abstract class PadViewModel : ViewModelBase, ICStreamerViewModel
    {
        public ElementViewModel Element { get; }
        public int PadIndex { get; }

        private ObservableAsPropertyHelper<double> x;
        private ObservableAsPropertyHelper<double> y;

        public PadViewModel(ElementViewModel element, int padIndex)
        {
            Element = element;
            PadIndex = padIndex;

            this.x = this.Element.WhenAnyValue(e => e.X).Select(x => x + this.XOffset - Settings.PadRadius).ToProperty(this, x => x.X);
            this.y = this.Element.WhenAnyValue(e => e.Y).Select(y => y + this.YOffset - Settings.PadRadius).ToProperty(this, x => x.Y);

            this.WhenActivated((disposables) =>
            {
                this.x.DisposeWith(disposables);
                this.y.DisposeWith(disposables);
            });
        }


        public int ZIndex => ZLayer.Pads;

        protected abstract double XOffset { get; }

        private double YOffset => (this.PadIndex * (Settings.PadDistance + Settings.PadSize)) + Settings.PadTopOffset;

        public double X => this.x.Value;

        public double Y => this.y.Value;

    }
}
