// -----------------------------------------------------------------------
// <copyright file="SinkPadViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Designer.Avalonia.Views;
    using CStreamer.Plugins.Interfaces;

    [LocateView(typeof(PadView))]
    public class SinkPadViewModel : PadViewModel
    {
        public SinkPadViewModel(ISinkPad model, ElementViewModel element, int padIndex)
            : base(element, padIndex)
        {
            this.Model = model;
            this.Type = "double";
            this.Info = string.Empty;
        }

        public ISinkPad Model { get; }

        public string Name => this.Model.Name;

        public string Type { get; }

        public string Info { get; }

        protected override double XOffset => 0;
    }
}
