using System;
using System.Collections.Generic;
using System.Text;
using CStreamer.Plugins.Interfaces;

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    public class StringSettingViewModel : BaseSettingViewModel<string>
    {
        public StringSettingViewModel(IPropertyBinding<string> binding) : base(binding)
        {

        }
    }
}
