using CStreamer.Plugins.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    class ObjectSettingsViewModel : ViewModelBase, ISettingViewModel
    {
        private object value;

        public ObjectSettingsViewModel(IPropertyBinding binding)
        {

        }

        public object Value
        {
            get => this.value;
            private set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        public string Name
        {
            get;
        }
    }
}
