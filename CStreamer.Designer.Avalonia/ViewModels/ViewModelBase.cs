using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class ViewModelBase : ReactiveObject, ISupportsActivation
    {
        public ViewModelActivator Activator { get; }

        public ViewModelBase()
        {
            this.Activator = new ViewModelActivator();
        }
    }
}
