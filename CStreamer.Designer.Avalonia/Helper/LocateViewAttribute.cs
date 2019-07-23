using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.Helper
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class LocateViewAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name to register under.</param>
        public LocateViewAttribute(Type targetType)
        {
            this.TargetType = targetType;
        }

        /// <summary>
        /// Gets the name to register under.
        /// </summary>
        /// <value>
        /// The name to register under.
        /// </value>
        public Type TargetType { get; }
    }
}
