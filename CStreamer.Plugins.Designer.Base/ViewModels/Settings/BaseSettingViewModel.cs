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

    /// <summary>
    /// A Simple ViewModel that contains a Name and a Value.
    /// Used for presenting a simple Settings Control.
    /// </summary>
    /// <typeparam name="TValue">The Type of the <see cref="Value"/> Property.</typeparam>
    internal abstract class BaseSettingViewModel<TValue> : ViewModelBase, ISettingViewModel
        where TValue : IEquatable<TValue>
    {
        private readonly IPropertyBinding<TValue> binding;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSettingViewModel{TValue}"/> class.
        /// </summary>
        /// <param name="binding">The underlying PropertyBinding.</param>
        protected BaseSettingViewModel(IPropertyBinding<TValue> binding)
        {
            this.binding = binding;

            this.binding.ValueChanged += this.Binding_ValueChanged;
            this.Name = binding.Name;
        }

        /// <summary>
        /// Gets the Value of the underlying PropertyBinding.
        /// </summary>
        /// <value>
        /// The Value of the underlying PropertyBinding.
        /// </value>
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

        /// <summary>
        /// Gets the name of the Property.
        /// Used to present a label in the UI.
        /// </summary>
        /// <value>
        /// The name of the Property.
        /// </value>
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
