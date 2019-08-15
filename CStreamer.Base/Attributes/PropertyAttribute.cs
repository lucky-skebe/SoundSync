// -----------------------------------------------------------------------
// <copyright file="PropertyAttribute.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.Attributes
{
    using System;

    [AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
    }
}
