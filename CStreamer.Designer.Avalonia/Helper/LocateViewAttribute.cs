// -----------------------------------------------------------------------
// <copyright file="LocateViewAttribute.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Helper
{
    using System;

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class LocateViewAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocateViewAttribute"/> class.
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
