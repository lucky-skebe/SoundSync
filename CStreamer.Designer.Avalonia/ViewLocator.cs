namespace CStreamer.Designer.Avalonia
{
    using System;
    using System.Linq;
    using System.Reflection;
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl? Build(object data)
        {
            Type? type = null;
            if (data?.GetType()?.GetCustomAttributes()?.OfType<LocateViewAttribute>()?.SingleOrDefault() is LocateViewAttribute attribute)
            {
                type = attribute.TargetType;
            }
            else
            {
                var name = data?.GetType()?.FullName?.Replace("ViewModel", "View", StringComparison.InvariantCulture);
                if (name != null)
                {
                    type = Type.GetType(name);
                }
            }

            if (type != null)
            {
                return (IControl?)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + data?.GetType()?.Name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}