// -----------------------------------------------------------------------
// <copyright file="DoubleSettingViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using System.Reflection;
    using CStreamer.Base.BaseElements;

    /// <summary>
    /// A Simple ViewModel that contains a Name and a Value.
    /// Used for presenting a simple Settings Control.
    /// </summary>
    internal class DoubleSettingViewModel : BaseSettingViewModel<double>
    {
        public DoubleSettingViewModel(IElement element, PropertyInfo property)
            : base(element, property)
        {
        }
    }
}
