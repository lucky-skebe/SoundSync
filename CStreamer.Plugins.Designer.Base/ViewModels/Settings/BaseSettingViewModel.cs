// -----------------------------------------------------------------------
// <copyright file="BaseSettingViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reflection;
    using CStreamer.Base.BaseElements;
    using CStreamer.Base.Events;
    using ReactiveUI;

    /// <summary>
    /// A Simple ViewModel that contains a Name and a Value.
    /// Used for presenting a simple Settings Control.
    /// </summary>
    /// <typeparam name="TValue">The Type of the <see cref="Value"/> Property.</typeparam>
    internal abstract class BaseSettingViewModel<TValue> : ViewModelBase, ISettingViewModel
        where TValue : IEquatable<TValue>
    {
        private readonly IElement element;
        private readonly PropertyInfo property;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSettingViewModel{TValue}"/> class.
        /// </summary>
        /// <param name="element">The underlying CStreamer Element.</param>
        /// <param name="property">The Property this ViewModel binds to.</param>
        protected BaseSettingViewModel(IElement element, PropertyInfo property)
        {
            this.element = element;
            this.property = property;

            this.Name = property.Name;

            this.WhenActivated(disposables =>
            {
                Observable
                    .FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(handler => element.PropertyChanged += handler, handler => element.PropertyChanged -= handler)
                    .Where(args => args.PropertyName == string.Empty || args.PropertyName == this.Name)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => this.RaisePropertyChanged("Value"))
                    .DisposeWith(disposables);
            });
        }

        /// <summary>
        /// Gets or sets the Value of the underlying PropertyBinding.
        /// </summary>
        /// <value>
        /// The Value of the underlying PropertyBinding.
        /// </value>
        public TValue Value
        {
            get => (TValue)this.property.GetValue(this.element);
            set
            {
                if (!this.Value.Equals(value))
                {
                    this.property.SetValue(this.element, value);
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
    }
}
