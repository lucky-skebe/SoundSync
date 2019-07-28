// -----------------------------------------------------------------------
// <copyright file="PipelineItemsControl.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Controls
{
    using CStreamer.Designer.Avalonia.Helper;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Generators;

    public class PipelineItemsControl : ItemsControl
    {
        public PipelineItemsControl()
            : base()
        {
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new CanvasItemContainerGenerator(this);
        }
    }
}
