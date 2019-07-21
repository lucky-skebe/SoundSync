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
using static CStreamer.Designer.Avalonia.Views.ToolBarView;

namespace CStreamer.Designer.Avalonia.Views
{
    public class PipelineView : ReactiveUserControl<PipelineViewModel>
    {
        private Point startPoint;
        private bool isDragging;
        private Point offset;
        private DataObject? dragData;

        private void Canvas_Drop(DragEventArgs e)
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
                (Math.Abs(diff.X) > Settings.MinDragDistance ||
                Math.Abs(diff.Y) > Settings.MinDragDistance))
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
            this.InitializeComponent();
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
                    .Subscribe((ev) => {
                        if (ev.Name != null)
                        {
                            this.ViewModel.CreateElement(ev.Name, ev.Position);
                        }
                    })
                    .DisposeWith(disposables);

            });
            AvaloniaXamlLoader.Load(this);
        }

        public EventObserver<PipelineView> Events()
        {
            return new EventObserver<PipelineView>(this);
        }
    }
}
