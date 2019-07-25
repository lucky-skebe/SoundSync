using CStreamer.Plugins.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    public abstract class BaseSettingViewModel<TValue> : ViewModelBase, ISettingViewModel where TValue : IEquatable<TValue>
    {
        private readonly IPropertyBinding<TValue> binding;

        protected BaseSettingViewModel(IPropertyBinding<TValue> binding)
        {
            this.binding = binding;

            this.binding.ValueChanged += Binding_ValueChanged;
            this.Name = binding.Name;
        }

        private void Binding_ValueChanged(object sender, BindingValueChangedEventArgs<TValue> e)
        {
            this.RaisePropertyChanged(nameof(this.Value));
        }

        public TValue Value
        {
            get => this.binding.Value;
            private set
            {
                if(!Value.Equals(value))
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
    }
}
