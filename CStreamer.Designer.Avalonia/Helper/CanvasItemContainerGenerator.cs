using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.Helper
{
    public class CanvasItemContainerGenerator : ItemContainerGenerator
    {
        public CanvasItemContainerGenerator(IControl owner) : base(owner)
        {
        }

        public override Type ContainerType => typeof(ContentPresenter);

        protected override IControl CreateContainer(object item)
        {
            
            var container = base.CreateContainer(item);
            if (item is ElementViewModel vm)
            {
                container.Bind(Canvas.TopProperty, vm.WhenAnyValue(x => x.Y), BindingPriority.TemplatedParent);
                container.Bind(Canvas.LeftProperty, vm.WhenAnyValue(x => x.X), BindingPriority.TemplatedParent);

            }

            return container;
        }
    }
}
