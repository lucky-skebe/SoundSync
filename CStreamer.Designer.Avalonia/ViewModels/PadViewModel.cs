// -----------------------------------------------------------------------
// <copyright file="PadViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Designer.Avalonia.Helper;
    using ReactiveUI;

    internal abstract class PadViewModel : ViewModelBase, ICStreamerViewModel
    {
        private readonly ObservableAsPropertyHelper<double> x;

        private readonly ObservableAsPropertyHelper<double> y;

        protected PadViewModel(ElementViewModel element, int padIndex)
        {
            this.Element = element;
            this.PadIndex = padIndex;

            this.x = this.Element.WhenAnyValue(e => e.X).Select(x => x + this.XOffset - Settings.PadRadius).ToProperty(this, pad => pad.X);
            this.y = this.Element.WhenAnyValue(e => e.Y).Select(y => y + this.YOffset - Settings.PadRadius).ToProperty(this, pad => pad.Y);

            this.WhenActivated((disposables) =>
            {
                this.x.DisposeWith(disposables);
                this.y.DisposeWith(disposables);
            });
        }

        public ElementViewModel Element { get; }

        public int PadIndex { get; }

        public int ZIndex => ZLayer.Pads;

        public double X => this.x.Value;

        public double Y => this.y.Value;

        protected abstract double XOffset { get; }

        private double YOffset => (this.PadIndex * (Settings.PadDistance + Settings.PadSize)) + Settings.PadTopOffset;
    }
}
