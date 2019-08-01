// -----------------------------------------------------------------------
// <copyright file="PipeLineRenderer.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Shapes;
    using SharPipes.UI.GraphicalDecorators;
    using SharPipes.UI.Helpers;

    /// <summary>
    /// Draws a <see cref="GraphicalPipeline"/>.
    /// </summary>
    public class PipeLineRenderer : ItemsControl
    {
        /// <summary>
        /// Identifies the <see cref="SelectedElement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(GraphicalElement?), typeof(PipeLineRenderer), new PropertyMetadata(null));

        static PipeLineRenderer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PipeLineRenderer),
                new FrameworkPropertyMetadata(typeof(PipeLineRenderer)));
        }

        /// <summary>
        /// Gets or sets the selected Graphical Element.
        /// </summary>
        /// <value>
        /// The selected Graphical Element.
        /// </value>
        public GraphicalElement? SelectedElement
        {
            get { return (GraphicalElement?)this.GetValue(SelectedElementProperty); }
            set { this.SetValue(SelectedElementProperty, value); }
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PipeLineItem;
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PipeLineItem();
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.OriginalSource is DependencyObject originalSource)
            {
                if (!(originalSource is Path))
                {
                    PipeLineItem? item = originalSource.FindAnchestor<PipeLineItem>();
                    this.SelectedElement = item?.DataContext as GraphicalElement;
                    base.OnPreviewMouseLeftButtonUp(e);
                }
            }
        }
    }
}
