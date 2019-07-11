using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.Pipes.Base
{
    public class PropertySetter<TValue> : IPropertySetter
    {
        private readonly string name;
        private readonly Action<TValue> setValue;

        public PropertySetter(string Name, Action<TValue> setValue)
        {
            name = Name;
            this.setValue = setValue;
        }

        public PropertySetter(Expression<Func<TValue>> property)
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
            }
            else
            {
                throw new ArgumentException("invalid SetValue Expression.");
            }
        }

        public bool TrySetValue(PropertyValue propvalue)
        {
            if (this.name.Equals(propvalue.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                if (typeof(TValue).IsAssignableFrom(propvalue.Value.GetType()))
                {
                    this.setValue((TValue)propvalue.Value);
                    return true;
                }
            }
            return false;
        }
    }
}
