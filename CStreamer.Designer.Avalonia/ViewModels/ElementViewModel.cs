// -----------------------------------------------------------------------
// <copyright file="ElementViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Plugins.Interfaces;
    using global::Avalonia;
    using ReactiveUI;

    class ElementViewModel : ViewModelBase, ICStreamerViewModel
    {
        private double x;

        private double y;

        public ElementViewModel(IElement model, double x, double y)
        {
            this.x = x;
            this.y = y;
            this.Model = model;
        }

        public ElementViewModel(IElement model, Point position)
        {
            this.x = position.X;
            this.y = position.Y;
            this.Model = model;
        }

        public IElement Model { get; }

        public double X { get => this.x; private set => this.RaiseAndSetIfChanged(ref this.x, value); }

        public double Y { get => this.y; private set => this.RaiseAndSetIfChanged(ref this.y, value); }

        public string Name => this.Model.GetElementName();

        public int ZIndex => ZLayer.Elements;

        public void MoveTo(Point position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }

        public bool Equals(ElementViewModel other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Model.Equals(other.Model);
        }
    }
}
