using System.Collections.ObjectModel;
using System.Windows.Threading;
using SharPipes.Pipes.Buttplug;

namespace SharPipes.UI.Helpers
{
    public class DispatchedDeviceList : DispatchedObservableCollection<ButtPlugClientDeviceWrapper>
    {
        public DispatchedDeviceList(ObservableCollection<ButtPlugClientDeviceWrapper> collection, Dispatcher currentDispatcher) : base(collection, currentDispatcher)
        {
        }
    }
}
