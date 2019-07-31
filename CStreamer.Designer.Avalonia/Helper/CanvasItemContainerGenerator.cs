// -----------------------------------------------------------------------
// <copyright file="CanvasItemContainerGenerator.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Helper
{
    using System;
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Generators;
    using global::Avalonia.Controls.Presenters;
    using global::Avalonia.Data;
    using ReactiveUI;

    internal class CanvasItemContainerGenerator : ItemContainerGenerator
    {
        public CanvasItemContainerGenerator(IControl owner)
            : base(owner)
        {
        }

        public override Type ContainerType => typeof(ContentPresenter);

        protected override IControl CreateContainer(object item)
        {
            var container = base.CreateContainer(item);
            if (item is ICStreamerViewModel vm)
            {
                container.Bind(Canvas.TopProperty, vm.WhenAnyValue(x => x.Y), BindingPriority.TemplatedParent);
                container.Bind(Canvas.LeftProperty, vm.WhenAnyValue(x => x.X), BindingPriority.TemplatedParent);
            }

            return container;
        }
    }
}
