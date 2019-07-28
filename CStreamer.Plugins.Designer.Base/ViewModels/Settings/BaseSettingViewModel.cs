// -----------------------------------------------------------------------
// <copyright file="BaseSettingViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using System;
    using CStreamer.Plugins.Interfaces;
    using ReactiveUI;

    public abstract class BaseSettingViewModel<TValue> : ViewModelBase, ISettingViewModel
        where TValue : IEquatable<TValue>
    {
        private readonly IPropertyBinding<TValue> binding;

        protected BaseSettingViewModel(IPropertyBinding<TValue> binding)
        {
            this.binding = binding;

            this.binding.ValueChanged += this.Binding_ValueChanged;
            this.Name = binding.Name;
        }

        public TValue Value
        {
            get => this.binding.Value;
            private set
            {
                if (!this.Value.Equals(value))
                {
                    this.RaisePropertyChanging();
                    this.binding.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Name
        {
            get;
        }

        private void Binding_ValueChanged(object sender, BindingValueChangedEventArgs<TValue> e)
        {
            this.RaisePropertyChanged(nameof(this.Value));
        }
    }
}
