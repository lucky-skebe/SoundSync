using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Plugins.Designer.Base.ViewModels;
using CStreamer.Plugins.Designer.Base.Views;
using CStreamer.Plugins.Interfaces;
using System;

namespace CStreamer.Plugins.Designer.Base
{
    public class SettingsViewLocator : IDataTemplate
    {
        public static SettingsViewLocator Instance {get;} = new SettingsViewLocator();

        public bool SupportsRecycling => false;

        private SettingsViewLocator()
        {

        }

        public IControl Build(object data)
        {
            Type type = data.GetType();

            //if (type != null )
            //{
            //    return (Control)Activator.CreateInstance(type);
            //}
            //else
            //{
                return new FallbackSettingsView() { DataContext = new FallbackSettingsViewModel(data as IElement) };
            // }
        }

        public bool Match(object data)
        {
            return data is IElement;
        }
    }
}
