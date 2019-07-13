using System;
using System.Linq.Expressions;
using System.Reflection;
using Optional;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.Pipes.Base
{
    public class PropertyBinding<TValue> : IPropertyBinding
    {
        private readonly string name;
        private readonly Action<TValue> setValue;
        private readonly Func<TValue> getValue;
        private readonly Func<object?, Option<TValue>>? convert;

        public PropertyBinding(string Name, Action<TValue> setValue, Func<TValue> getValue, Func<object?, Option<TValue>>? convert = null)
        {
            name = Name;
            this.setValue = setValue;
            this.getValue = getValue;
            this.convert = convert;
        }

        public PropertyBinding(Expression<Func<TValue>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("setValue");
            }
            var body = property.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Lambda must return a property.");
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
                throw new ArgumentException("invalid SetValue Expression.");
            }
        }

        public PropertyValue GetValue()
        {
            if(this.getValue == null)
            {
                throw new ArgumentNullException($"Could not get Value for Property {this.name}");
            }
            else
            {
                return new PropertyValue(this.name, this.getValue());
            }
        }

        public bool TrySetValue(PropertyValue propvalue)
        {
            if (this.name.Equals(propvalue.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                if(convert != null)
                {
                    var option = convert(propvalue.Value);
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
                        TValue val = (TValue)Convert.ChangeType(propvalue.Value, typeof(TValue));
                        this.setValue((TValue)val);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
