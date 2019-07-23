﻿using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.Views;
using CStreamer.Plugins.Interfaces;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    [LocateView(typeof(PadView))]
    class SinkPadViewModel : PadViewModel
    {
        public SinkPadViewModel(ISinkPad model, ElementViewModel element, int padIndex) : base(element, padIndex)
        {
            Model = model;
        }

        public ISinkPad Model { get; }
        public double Settins { get; private set; }

        protected override double XOffset => 0;
    }
}
