﻿using Avalonia;
using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Plugins.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class ElementViewModel : ViewModelBase, ICStreamerViewModel
    {
        private double x;

        private double y;

        public IElement Model { get; }

        public ElementViewModel(IElement model, double x, double y)
        {
            this.x = x;
            this.y = y;
            this.Model = model;
        }

        public ElementViewModel(IElement model, Point position )
        {
            this.x = position.X;
            this.y = position.Y;
            this.Model = model;
        }

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

        //internal void Delete()
        //{
        //    this.pipeline.Remove(this);
        //}
    }
}