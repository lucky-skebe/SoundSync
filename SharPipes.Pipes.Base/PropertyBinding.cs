// -----------------------------------------------------------------------
// <copyright file="PropertyBinding.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using Optional;
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    /// <summary>
    /// Describes how to set/get a certain properties value.
    /// </summary>
    /// <typeparam name="TValue">The Type of the underlying property.</typeparam>
    public class PropertyBinding<TValue> : IPropertyBinding
    {
        private readonly string name;
        private readonly Action<TValue> setValue;
        private readonly Func<TValue> getValue;
        private readonly Func<object?, Option<TValue>>? convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBinding{TValue}"/> class.
        /// </summary>
        /// <param name="name">than name of the property.</param>
        /// <param name="setValue">How to set the properties value.</param>
        /// <param name="getValue">How to get teh properties value.</param>
        /// <param name="convert">a Custom conversion logic if no generic cast can be determined.</param>
        public PropertyBinding(string name, Action<TValue> setValue, Func<TValue> getValue, Func<object?, Option<TValue>>? convert = null)
        {
            this.name = name;
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

            var body = property.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException(Properties.strings.LambdaMustReturnAProperty);
            }

            var caller = (body.Expression as ConstantExpression)?.Value;
            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo != null && caller != null)
            {
                this.name = body.Member.Name;

                this.setValue = (v) => propertyInfo.SetValue(caller, v, null);
                this.getValue = property.Compile();
            }
            else
            {
                throw new ArgumentException(Properties.strings.InvalidSetValueExpression);
            }
        }

        /// <inheritdoc/>
        public PropertyValue GetValue()
        {
            if (this.getValue == null)
            {
                throw new ArgumentNullException(this.name);
            }
            else
            {
                return new PropertyValue(this.name, this.getValue());
            }
        }

        /// <inheritdoc/>
        public bool TrySetValue(PropertyValue propvalue)
        {
            if (propvalue == null)
            {
                throw new ArgumentNullException(nameof(propvalue));
            }

            if (this.name.Equals(propvalue.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                if (this.convert != null)
                {
                    var option = this.convert(propvalue.Value);
                    option.MatchSome(this.setValue);
                }
                else if (propvalue.Value == null)
                {
                    return false;
                }
                else if (typeof(TValue).IsAssignableFrom(propvalue.Value.GetType()))
                {
                    this.setValue((TValue)propvalue.Value);
                    return true;
                }
                else
                {
                    try
                    {
                        TValue val = (TValue)Convert.ChangeType(propvalue.Value, typeof(TValue), CultureInfo.InvariantCulture);
                        this.setValue((TValue)val);
                        return true;
                    }
                    catch (InvalidCastException)
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
