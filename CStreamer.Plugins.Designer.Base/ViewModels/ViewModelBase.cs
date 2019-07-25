using ReactiveUI;

namespace CStreamer.Plugins.Designer.Base.ViewModels
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