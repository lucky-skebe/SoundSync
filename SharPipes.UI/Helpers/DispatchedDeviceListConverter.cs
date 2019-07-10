using System.Collections.ObjectModel;
using System.Windows.Threading;
using SharPipes.Pipes.Buttplug;

namespace SharPipes.UI.Helpers
{
    public class DispatchedDeviceListConverter : DispatchedObservableCollectionConverter<ButtPlugClientDeviceWrapper>
    {
        protected override DispatchedObservableCollection<ButtPlugClientDeviceWrapper> GetWrapper(ObservableCollection<ButtPlugClientDeviceWrapper> collection, Dispatcher dispatcher)
        {
            return new DispatchedDeviceList(collection, dispatcher);
        }
    }
}
