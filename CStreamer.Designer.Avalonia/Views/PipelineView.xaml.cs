// -----------------------------------------------------------------------
// <copyright file="PipelineView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Designer.Avalonia.ViewModels;
    using CStreamer.Plugins.Interfaces;
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Presenters;
    using global::Avalonia.Data;
    using global::Avalonia.Input;
    using global::Avalonia.Markup.Xaml;
    using ReactiveUI;
    using static CStreamer.Designer.Avalonia.Views.ToolBarView;

    public class PipelineView : ReactiveUserControl<PipelineViewModel>
    {
        public static readonly StyledProperty<IElement?> SelectedElementProperty = AvaloniaProperty.Register<PipelineView, IElement?>(nameof(SelectedElement));

        private Point startPoint;

        private bool isDragging;

        private Point offset;

        private DataObject? dragData;

        public PipelineView()
        {
            this.InitializeComponent();
        }

        public IElement? SelectedElement
        {
            get => this.GetValue(SelectedElementProperty);
            set => this.SetValue(SelectedElementProperty, value);
        }

        public EventObserver<PipelineView> Events()
        {
            return new EventObserver<PipelineView>(this);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            this.isDragging = false;
            this.dragData = null;
            this.HandleDragStart<ElementView, ElementViewModel>(e, "moveElement");
            this.HandleDragStart<PadView, PadViewModel>(e, "drawEdge");
            base.OnPointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            this.HandleDragOver(e);
            base.OnPointerMoved(e);
        }

        private void HandleDragStart<TItem, TViewModel>(PointerPressedEventArgs e, string format)
            where TItem : class, IControl, IViewFor<TViewModel>
            where TViewModel : class
        {
            TItem? listViewItem = (e.Source as IControl)?.FindAnchestor<TItem>();

            if (listViewItem == null)
            {
                return;
            }

            this.startPoint = e.GetPosition(this);
            this.isDragging = false;

            this.offset = e.GetPosition(listViewItem);

            this.dragData = new DataObject();
            this.dragData.Set(format, listViewItem.ViewModel);
        }

        private void HandleDragOver(PointerEventArgs e)
        {
            if (this.isDragging || this.dragData == null || this.startPoint == null)
            {
                return;
            }

            Point mousePos = e.GetPosition(this);

            var diff = this.startPoint - mousePos;

            if (e.InputModifiers.HasFlag(InputModifiers.LeftMouseButton) &&
                (Math.Abs(diff.X) > Settings.MinDragDistance ||
                Math.Abs(diff.Y) > Settings.MinDragDistance))
            {
                this.isDragging = true;

                DragDrop.DoDragDrop(this.dragData, DragDropEffects.Move);
            }
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                var drop = this.Events().Drop;

                drop.Where(@event => @event.EventArgs.Data.Contains("moveElement"))
                    .Select(@event => (@event.EventArgs.Data.Get("moveElement") as ElementViewModel, @event.EventArgs.GetPosition(this) - this.offset))
                    .Subscribe((ev) => ev.Item1?.MoveTo(ev.Item2))
                    .DisposeWith(disposables);

                drop.Where(@event => @event.EventArgs.Data.Contains("newElement"))
                    .Select(@event => new { Name = @event.EventArgs.Data.Get("newElement") as string, Position = @event.EventArgs.GetPosition(this) - new Vector(Settings.ElementWidth / 2, Settings.ElementHeight / 2) })
                    .Subscribe((ev) =>
                    {
                        if (ev.Name != null)
                        {
                            this.ViewModel.CreateElement(ev.Name, ev.Position);
                        }
                    })
                    .DisposeWith(disposables);

                drop.Where(@event => @event.EventArgs.Data.Contains("drawEdge"))
                    .Select(@event => new { Src = @event.EventArgs.Data.Get("drawEdge") as SrcPadViewModel, Sink = (@event.EventArgs.Source as IControl)?.FindAnchestor<PadView>()?.DataContext as SinkPadViewModel })
                    .Where(link => link.Src != null && link.Sink != null)
#pragma warning disable CS8604 // Mögliches Nullverweisargument.
                    .Subscribe((ev) => this.ViewModel.TryConnect(ev.Src, ev.Sink))
#pragma warning restore CS8604 // Mögliches Nullverweisargument.
                    .DisposeWith(disposables);

                this.Events().PointerReleased
                    .Select(@event => (@event.EventArgs.Source as IControl)?.FindAnchestor<ElementView>()?.ViewModel?.Model)
                    .Subscribe((val) =>
                    {
                        this.SetValue(SelectedElementProperty, val, BindingPriority.LocalValue);
                    })
                    .DisposeWith(disposables);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
