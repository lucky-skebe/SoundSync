// -----------------------------------------------------------------------
// <copyright file="PluginAssemblyAttribute.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using System;

    /// <summary>
    /// Marks an assembly as a plugin to reduce unneccesary loading of non plugin libraries on startup.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class PluginAssemblyAttribute : Attribute
    {
    }
}
