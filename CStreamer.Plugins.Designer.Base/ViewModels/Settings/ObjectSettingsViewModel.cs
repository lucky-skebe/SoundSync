// -----------------------------------------------------------------------
// <copyright file="ObjectSettingsViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using CStreamer.Plugins.Interfaces;
    using ReactiveUI;

    public class ObjectSettingsViewModel : ViewModelBase, ISettingViewModel
    {
        private object? value;

        public ObjectSettingsViewModel(IPropertyBinding binding)
        {
            this.value = binding.GetValue().Value;
            this.Name = binding.Name;
        }

        public object? Value
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
