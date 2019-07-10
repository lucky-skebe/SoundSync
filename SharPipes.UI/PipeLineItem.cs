using SharPipes.UI.GraphicalDecorators;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SharPipes.UI
{
    class PipeLineItem : Control
    {
        private GraphicalElement? element;
        private StackPanel? AdornerIcons;
        private Path? DeleteIcon;

        static PipeLineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PipeLineItem),
                new FrameworkPropertyMetadata(typeof(PipeLineItem)));
        }

        public PipeLineItem()
        {
            this.DataContextChanged += PipeLineItem_DataContextChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AdornerIcons = this.GetTemplateChild("AdornerIcons") as StackPanel;
            
            if (DeleteIcon != null)
            {
                DeleteIcon.PreviewMouseLeftButtonUp -= DeleteIcon_PreviewMouseLeftButtonUp;
            }
            DeleteIcon = this.GetTemplateChild("DeleteIcon") as Path;
            if (DeleteIcon != null)
            {
                DeleteIcon.PreviewMouseLeftButtonUp += DeleteIcon_PreviewMouseLeftButtonUp;
            }
        }

        private void DeleteIcon_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this?.element?.Delete();
            e.Handled = true;
        }

        private void PipeLineItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.element = e.NewValue as GraphicalElement;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (AdornerIcons != null && this.element != null)
            {
                AdornerIcons.Visibility = Visibility.Visible;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (AdornerIcons != null)
            {
                AdornerIcons.Visibility = Visibility.Collapsed;
            }
            base.OnMouseLeave(e);
        }

    }
}
