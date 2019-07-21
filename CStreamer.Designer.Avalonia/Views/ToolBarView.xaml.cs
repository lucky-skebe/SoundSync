using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;
using System;

namespace CStreamer.Designer.Avalonia.Views
{
    public class ToolBarView : ReactiveUserControl<ToolBarViewModel>
    {
        private bool isDragging;
        private DataObject? dragData;
        private Point startPoint;

        public ToolBarView()
        {
            this.InitializeComponent();
        }

        private void HandleDragStart<TViewModel>(PointerPressedEventArgs e, string format)
            where TViewModel : class
        {
            ContentPresenter? listViewItem = (e.Source as IControl)?.FindAnchestor<ContentPresenter>();

            if (listViewItem == null)
            {
                return;
            }

            this.startPoint = e.GetPosition(this);
            this.isDragging = false;

            this.dragData = new DataObject();
            this.dragData.Set(format, listViewItem.Content);
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

        private void HandleDragLeave(PointerEventArgs e)
        {
            if (this.isDragging || this.dragData == null || this.startPoint == null)
            {
                return;
            }



            if (e.InputModifiers.HasFlag(InputModifiers.LeftMouseButton))
            {
                this.isDragging = true;

                DragDrop.DoDragDrop(this.dragData, DragDropEffects.Move);
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            this.isDragging = false;
            this.dragData = null;
            this.HandleDragStart<string>(e, "newElement");
            base.OnPointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            this.HandleDragOver(e);
            base.OnPointerMoved(e);
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            this.HandleDragLeave(e);
            base.OnPointerLeave(e);
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {

            });
            AvaloniaXamlLoader.Load(this);
        }

        public EventObserver<ToolBarView> Events()
        {
            return new EventObserver<ToolBarView>(this);
        }
    }
}
