using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CStreamer.Designer.Avalonia.Views
{
    public class PipelineView : ReactiveUserControl<PipelineViewModel>
    {
        private Point startPoint;
        private bool isDragging;
        private Point offset;
        private DataObject? dragData;

        public class EventObserver
        {
            private readonly PipelineView parent;

            public EventObserver(PipelineView parent)
            {
                this.parent = parent;
            }

            public IObservable<EventPattern<PointerPressedEventArgs>> PointerPressed => Observable.FromEventPattern<PointerPressedEventArgs>(handler => this.parent.PointerPressed += handler, handler => this.parent.PointerPressed -= handler);
            public IObservable<EventPattern<PointerEventArgs>> PointerMoved => Observable.FromEventPattern<PointerEventArgs>(handler => this.parent.PointerMoved += handler, handler => this.parent.PointerMoved -= handler);
            public IObservable<EventPattern<PointerReleasedEventArgs>> PointerReleased => Observable.FromEventPattern<PointerReleasedEventArgs>(handler => this.parent.PointerReleased += handler, handler => this.parent.PointerReleased -= handler);

        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            //if (e.Data.Contains("fromToolBar"))
            //{
            //    if (e.Data.Get("fromToolBar") is IElement template)
            //    {
            //        this.Pipeline.CreateNodeFromTemplate(template, e.GetPosition(sender as IInputElement) - this.offset);
            //    }
            //}
            //else if (e.Data.Contains("drawEdge"))
            //{
            //    if (e.Data.Get("drawEdge") is GraphicalSrcPad src)
            //    {
            //        PipeLineItem? listViewItem =
            //            ((DependencyObject)e.OriginalSource).FindAnchestor<PipeLineItem>();

            //        if (!(sender is PipeLineRenderer listView) || listViewItem == null)
            //        {
            //            return;
            //        }

            //        // Find the data behind the ListViewItem
            //        if (listView.ItemContainerGenerator.
            //            ItemFromContainer(listViewItem) is GraphicalSinkPad sink)
            //        {
            //            this.Pipeline.TryConnect(src, sink);
            //        }
            //    }
            //}
            //else 
            if (e.Data.Contains("moveElement"))
            {
                if (e.Data.Get("moveElement") is ElementViewModel element)
                {
                    Point newPos = e.GetPosition(sender as IInputElement) - this.offset;

                    element.MoveTo(newPos);
                }
            }
        }

        private void HandleDragStart<TItem, TViewModel>(PointerPressedEventArgs e, string format)
            where TItem : class, IControl, IViewFor<TViewModel>
            where TViewModel : class
        {
            TItem? listViewItem = (e.Source as IControl)?.FindAnchestor<TItem>();

            var presenter = (e.Source as IControl)?.FindAnchestor<Canvas>();

            //Debug.WriteLine(presenter.GetValue(Canvas.TopProperty));

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
                (Math.Abs(diff.X) > 5 ||
                Math.Abs(diff.Y) > 5)) //TODO: create constants
            {
                this.isDragging = true;

                DragDrop.DoDragDrop(this.dragData, DragDropEffects.Move);
            }
        }

        

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            this.isDragging = false;
            this.dragData = null;
            this.HandleDragStart<ElementView, ElementViewModel>(e, "moveElement");
            //this.HandleDragStart<PipeLineRenderer, PipeLineItem, GraphicalSrcPad>(e, "drawEdge");
            base.OnPointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            this.HandleDragOver(e);
            base.OnPointerMoved(e);
        }

        public PipelineView()
        {

            this.WhenActivated(disposables =>
            {
                this.AddHandler<DragEventArgs>(DragDrop.DropEvent, Canvas_Drop);
            });
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public EventObserver Events()
        {
            return new EventObserver(this);
        }
    }
}
