using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Media;
using CStreamer.Designer.Avalonia.Helper;

namespace CStreamer.Designer.Avalonia.Controls
{
    public class PipelineItemsControl : ItemsControl
    {
        public PipelineItemsControl() : base()
        {
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new CanvasItemContainerGenerator(this);
        }
    }
}
