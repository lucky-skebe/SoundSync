using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Plugins.Designer.Base.ViewModels;
using CStreamer.Plugins.Designer.Base.Views;
using CStreamer.Plugins.Interfaces;
using System;

namespace CStreamer.Plugins.Designer.Base
{
    class FallbackDataTemplate : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            Type type = data.GetType();

            return new FallbackSettingsView() { DataContext = new FallbackSettingsViewModel(data as IElement) };
            
        }

        public bool Match(object data)
        {
            return data is IElement;
        }
    }
}
