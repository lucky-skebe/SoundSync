using SharPipes.UI.GraphicalDecorators;
using SharPipes.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SharPipes.UI
{
    class PipeLineRenderer : ItemsControl
    {
        public GraphicalElement? SelectedElement
        {
            get { return (GraphicalElement?)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(GraphicalElement?), typeof(PipeLineRenderer), new PropertyMetadata(null));



        static PipeLineRenderer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PipeLineRenderer),
                new FrameworkPropertyMetadata(typeof(PipeLineRenderer)));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is PipeLineItem);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PipeLineItem();
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            var originalSource = ((DependencyObject)e.OriginalSource);
            if(!(originalSource is Path))
            {
                PipeLineItem? item = originalSource.FindAnchestor<PipeLineItem>();
                this.SelectedElement = item?.DataContext as GraphicalElement;
                base.OnPreviewMouseLeftButtonUp(e);
            }
            
        }
    }
}
