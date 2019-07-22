using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public interface ICStreamerViewModel
    {
        double X { get; }

        double Y { get; }


        int ZIndex { get; }
    }
}
