// -----------------------------------------------------------------------
// <copyright file="PropertyBinding.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using CStreamer.Plugins.Interfaces;
    using Optional;

    /// <summary>
    /// Describes how to set/get a certain properties value.
    /// </summary>
    /// <typeparam name="TValue">The Type of the underlying property.</typeparam>
    public class PropertyBinding<TValue> : IPropertyBinding<TValue>
    {
        private readonly Action<TValue> setValue;
        private readonly Func<TValue> getValue;
        private readonly Func<object?, Option<TValue>>? convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBinding{TValue}"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="setValue">How to set the properties value.</param>
        /// <param name="getValue">How to get teh properties value.</param>
        /// <param name="convert">a Custom conversion logic if no generic cast can be determined.</param>
        public PropertyBinding(string name, Action<TValue> setValue, Func<TValue> getValue, Func<object?, Option<TValue>>? convert = null)
        {
            this.Name = name;
            this.setValue = setValue;
            this.getValue = getValue;
            this.convert = convert;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBinding{TValue}"/> class.
        /// </summary>
        /// <param name="property">An expression thast is used to automatically create a nes proprety binding.</param>
        public PropertyBinding(Expression<Func<TValue>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!(property.Body is MemberExpression body))
            {
                throw new ArgumentException("Lambda must return a property.");
            }

            var caller = (body.Expression as ConstantExpression)?.Value;
            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo != null && caller != null)
            {
                this.Name = body.Member.Name;

                this.setValue = (v) => propertyInfo.SetValue(caller, v, null);
                this.getValue = property.Compile();
            }
            else
            {
                throw new ArgumentException("Invalid SetValue Expression.");
            }
        }

        /// <inheritdoc/>
        public event EventHandler<BindingValueChangedEventArgs<TValue>> ValueChanged;

        event EventHandler<BindingValueChangedEventArgs> IPropertyBinding.ValueChanged
        {
            add
            {
                this.ValueChanged += (sender, args) => value(sender, args);
            }

            remove
            {
                this.ValueChanged -= (sender, args) => value(sender, args);
            }
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public TValue Value { get => this.getValue(); set => this.setValue(value); }

        /// <summary>
        /// Raises a <see cref="ValueChanged"/> Event.
        /// Use this in your Elements when property values change.
        /// </summary>
        /// <param name="value">The new value of the Property.</param>
        public void RaiseValueChanged(TValue value)
        {
            this.ValueChanged?.Invoke(this, new BindingValueChangedEventArgs<TValue>(value));
        }

        /// <inheritdoc/>
        public PropertyValue GetValue()
        {
            if (this.getValue == null)
            {
                throw new ArgumentNullException(this.Name);
            }
            else
            {
                return new PropertyValue(this.Name, this.getValue());
            }
        }

        /// <inheritdoc/>
        public Option<object?, string> TrySetValue(PropertyValue propvalue)
        {
            if (propvalue == null)
            {
                throw new ArgumentNullException(nameof(propvalue));
            }

            if (this.Name.Equals(propvalue.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                if (this.convert != null)
                {
                    var option = this.convert(propvalue.Value);
                    option.MatchSome(this.setValue);
                    return Option.Some<object?, string>(propvalue.Value);
                }
                else if (propvalue.Value == null)
                {
                    return Option.None<object?, string>("No Name provided");
                }
                else if (typeof(TValue).IsAssignableFrom(propvalue.Value.GetType()))
                {
                    this.setValue((TValue)propvalue.Value);
                    return Option.Some<object?, string>(propvalue.Value);
                }
                else
                {
                    try
                    {
                        TValue val = (TValue)Convert.ChangeType(propvalue.Value, typeof(TValue), CultureInfo.InvariantCulture);
                        this.setValue((TValue)val);
                        return Option.Some<object?, string>(propvalue.Value);
                    }
                    catch (InvalidCastException)
                    {
                        return Option.None<object?, string>($"Could not convert value {propvalue.Value} to type {typeof(TValue).Name}");
                    }
                }
            }

            return Option.None<object?, string>($"Name of the Provided Value did not match propertyname \"{propvalue.PropertyName}\" != \"{this.Name}\"");
        }
    }
}
