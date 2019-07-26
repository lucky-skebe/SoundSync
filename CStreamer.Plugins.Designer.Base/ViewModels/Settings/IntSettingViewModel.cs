using System;
using System.Collections.Generic;
using System.Text;
using CStreamer.Plugins.Interfaces;

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    public class IntSettingViewModel : BaseSettingViewModel<int>
    {
        public IntSettingViewModel(IPropertyBinding<int> binding) : base(binding)
        {

        }
    }
}
