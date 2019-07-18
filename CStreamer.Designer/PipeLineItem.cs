// -----------------------------------------------------------------------
// <copyright file="PipeLineItem.cs" company="LuckySkebe (fmann12345@gmail.com)">
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

    /// <summary>
    /// Draws a sigle element, pad or link of a <see cref="GraphicalPipeline" />.
    /// </summary>
    public class PipeLineItem : Control
    {
        private GraphicalElement? element;
        private StackPanel? adornerIcons;
        private Path? deleteIcon;

        static PipeLineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PipeLineItem),
                new FrameworkPropertyMetadata(typeof(PipeLineItem)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeLineItem"/> class.
        /// </summary>
        public PipeLineItem()
        {
            this.DataContextChanged += this.PipeLineItem_DataContextChanged;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.adornerIcons = this.GetTemplateChild("AdornerIcons") as StackPanel;

            if (this.deleteIcon != null)
            {
                this.deleteIcon.PreviewMouseLeftButtonUp -= this.DeleteIcon_PreviewMouseLeftButtonUp;
            }

            this.deleteIcon = this.GetTemplateChild("DeleteIcon") as Path;
            if (this.deleteIcon != null)
            {
                this.deleteIcon.PreviewMouseLeftButtonUp += this.DeleteIcon_PreviewMouseLeftButtonUp;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (this.adornerIcons != null && this.element != null)
            {
                this.adornerIcons.Visibility = Visibility.Visible;
            }

            base.OnMouseEnter(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.adornerIcons != null)
            {
                this.adornerIcons.Visibility = Visibility.Collapsed;
            }

            base.OnMouseLeave(e);
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
    }
}
