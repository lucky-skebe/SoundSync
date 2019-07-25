using CStreamer.Plugins.Designer.Base.ViewModels.Settings;
using CStreamer.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CStreamer.Plugins.Designer.Base.ViewModels
{
    public class FallbackSettingsViewModel
    {

        public FallbackSettingsViewModel(IElement element)
        {
            Element = element;

            this.Settings = new ObservableCollection<ISettingViewModel>(element.GetPropertyBindings().Select<IPropertyBinding, ISettingViewModel>(
                bind =>
                    bind switch
                    {
                        IPropertyBinding<string> b => (ISettingViewModel)new StringSettingViewModel(b),
                        IPropertyBinding b => new ObjectSettingsViewModel(b)
                    }
                ));
        }

        public IElement Element { get; }

        ObservableCollection<ISettingViewModel> Settings { get; }
    }
}
