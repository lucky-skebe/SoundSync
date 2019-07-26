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
            this.Type = "double";
            this.Info = "";
        }

        public ISinkPad Model { get; }

        public string Name => this.Model.Name;

        public string Type { get; }

        public string Info { get; }

        protected override double XOffset => 0;
    }
}
