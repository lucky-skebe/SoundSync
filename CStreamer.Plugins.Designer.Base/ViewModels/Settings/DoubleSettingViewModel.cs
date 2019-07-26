using System;
using System.Collections.Generic;
using System.Text;
using CStreamer.Plugins.Interfaces;

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    public class DoubleSettingViewModel : BaseSettingViewModel<double>
    {
        public DoubleSettingViewModel(IPropertyBinding<double> binding) : base(binding)
        {

        }
    }
}
