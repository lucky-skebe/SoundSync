using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.Views;
using CStreamer.Plugins.Interfaces;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    [LocateView(typeof(PadView))]
    public class SrcPadViewModel : PadViewModel
    {
        public SrcPadViewModel(ISrcPad model, ElementViewModel element, int padIndex) : base(element, padIndex)
        {
            Model = model;
        }

        public ISrcPad Model { get; }
        public double Settins { get; private set; }

        protected override double XOffset => Settings.ElementWidth;
    }
}
