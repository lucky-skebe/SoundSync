// -----------------------------------------------------------------------
// <copyright file="SrcPadViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using CStreamer.Base.BaseElements;
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Designer.Avalonia.Views;

    [LocateView(typeof(PadView))]
    internal class SrcPadViewModel : PadViewModel
    {
        public SrcPadViewModel(ISrcPad model, ElementViewModel element, int padIndex)
            : base(element, padIndex)
        {
            this.Model = model;
            this.Info = string.Empty;
        }

        public ISrcPad Model { get; }

        public string Name => this.Model.Name;

        public string Caps => this.Model.Caps;

        public string Info { get; }

        protected override double XOffset => Settings.ElementWidth;
    }
}
