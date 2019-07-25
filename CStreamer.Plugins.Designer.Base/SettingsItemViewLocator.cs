using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Plugins.Designer.Base.ViewModels.Settings;
using System;

namespace CStreamer.Plugins.Designer.Base
{
    internal class SettingsItemViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            Type type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + data.GetType().Name };
            }
        }

        public bool Match(object data)
        {
            return data is ISettingViewModel;
        }
    }
}
