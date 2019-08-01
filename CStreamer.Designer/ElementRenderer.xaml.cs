// -----------------------------------------------------------------------
// <copyright file="ElementRenderer.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using SharPipes.Pipes.Base;

    /// <summary>
    /// Interaktionslogik für ElementRenderer.xaml.
    /// </summary>
    public partial class ElementRenderer : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="Element"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(IElement), typeof(ElementRenderer), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRenderer"/> class.
        /// </summary>
        public ElementRenderer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the element to render.
        /// </summary>
        /// <value>
        /// The element to render.
        /// </value>
        public IElement Element
        {
            get { return (IElement)this.GetValue(ElementProperty); }
            set { this.SetValue(ElementProperty, value); }
        }
    }
}
