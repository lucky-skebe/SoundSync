using CStreamer.Designer.Avalonia.Helper;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class PadViewModel : ViewModelBase, ICStreamerViewModel
    {
        public ElementViewModel Element { get; }
        public PadType Type { get; }
        public int PadIndex { get; }

        private ObservableAsPropertyHelper<double> x;
        private ObservableAsPropertyHelper<double> y;

        public enum PadType
        {
            Src,
            Sink
        }

        public PadViewModel(ElementViewModel element, PadType type, int padIndex)
        {
            Element = element;
            Type = type;
            PadIndex = padIndex;

            this.x = this.Element.WhenAnyValue(e => e.X).Select(x => x + this.XOffset).ToProperty(this, x => x.X);
            this.y = this.Element.WhenAnyValue(e => e.Y).Select(y => y + this.YOffset).ToProperty(this, x => x.Y);

            this.WhenActivated((disposables) =>
            {
                

                this.x.DisposeWith(disposables);
                this.y.DisposeWith(disposables);
            });
        }


        public int ZIndex => ZLayer.Pads;

        private double XOffset => this.Type == PadType.Sink ? 0 : Settings.ElementWidth;

        private double YOffset => (this.PadIndex * (Settings.PadDistance + Settings.PadSize)) + Settings.PadTopOffset;

        public double X => this.x.Value;

        public double Y => this.y.Value;

    }
}
