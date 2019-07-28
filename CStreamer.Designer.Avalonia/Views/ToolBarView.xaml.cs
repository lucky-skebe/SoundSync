// -----------------------------------------------------------------------
// <copyright file="ToolBarView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using System;
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Presenters;
    using global::Avalonia.Input;
    using global::Avalonia.LogicalTree;
    using global::Avalonia.Markup.Xaml;
    using ReactiveUI;

    public class ToolBarView : ReactiveUserControl<ToolBarViewModel>
    {
        private bool isDragging;
        private DataObject? dragData;
        private Point startPoint;

        public ToolBarView()
        {
            this.InitializeComponent();
        }

        public EventObserver<ToolBarView> Events()
        {
            return new EventObserver<ToolBarView>(this);
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

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
