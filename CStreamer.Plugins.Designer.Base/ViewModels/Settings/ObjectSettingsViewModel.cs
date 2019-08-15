// -----------------------------------------------------------------------
// <copyright file="ObjectSettingsViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using CStreamer.Base;
    using ReactiveUI;

    /// <summary>
    /// A Simple ViewModel that contains a Name and a Value.
    /// Used for presenting a simple Settings Control.
    /// </summary>
    internal class ObjectSettingsViewModel : ViewModelBase, ISettingViewModel
    {
        private object? value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSettingsViewModel"/> class.
        /// </summary>
        /// <param name="name">The Name of the Property.</param>
        /// <param name="value">The Value to bind to.</param>
        public ObjectSettingsViewModel(string name, object? value)
        {
            this.value = value;
            this.Name = name;
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
