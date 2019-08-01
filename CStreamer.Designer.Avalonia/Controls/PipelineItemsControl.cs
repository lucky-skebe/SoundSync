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

#pragma warning disable CA1812 // Avoid uninstantiated internal classes.
    internal class PipelineItemsControl : ItemsControl

#pragma warning restore CA1812 // Avoid uninstantiated internal classes.
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
