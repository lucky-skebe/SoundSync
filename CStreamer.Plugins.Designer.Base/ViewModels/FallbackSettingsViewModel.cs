// -----------------------------------------------------------------------
// <copyright file="FallbackSettingsViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using CStreamer.Base;
    using CStreamer.Base.BaseElements;
    using CStreamer.Plugins.Designer.Base.ViewModels.Settings;

    internal class FallbackSettingsViewModel
    {
        public FallbackSettingsViewModel(IElement element)
        {
            this.Element = element;

            this.Settings = new ObservableCollection<ISettingViewModel>(element.GetProperties().Select<PropertyInfo, ISettingViewModel>(
                property =>
                 {
                     Type propertyType = property.PropertyType;
                     if (propertyType == typeof(string))
                     {
                         return new StringSettingViewModel(element, property);
                     }
                     else if (propertyType == typeof(double))
                     {
                         return new DoubleSettingViewModel(element, property);
                     }
                     else if (propertyType == typeof(int))
                     {
                         return new IntSettingViewModel(element, property);
                     }
                     else
                     {
                         return new ObjectSettingsViewModel(property.Name, property.GetValue(element) as object);
                     }
                 }));
        }

        public IElement Element { get; }

        public ObservableCollection<ISettingViewModel> Settings { get; }
    }
}
