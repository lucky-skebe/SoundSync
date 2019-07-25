using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.ViewModels;

namespace CStreamer.Designer.Avalonia
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            Type type;
            if (data.GetType().GetCustomAttributes().OfType<LocateViewAttribute>().SingleOrDefault() is LocateViewAttribute attribute)
            {
                type = attribute.TargetType;
            } else
            {
                var name = data.GetType().FullName.Replace("ViewModel", "View");
                type = Type.GetType(name);
            }

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
            return data is ViewModelBase;
        }
    }
}