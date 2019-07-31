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
    internal class LocateViewAttribute : Attribute
    {
        public LocateViewAttribute(Type targetType)
        {
            this.TargetType = targetType;
        }

        public Type TargetType { get; }
    }
}
