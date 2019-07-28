// -----------------------------------------------------------------------
// <copyright file="FallbackSettingsViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CStreamer.Plugins.Designer.Base.ViewModels.Settings;
    using CStreamer.Plugins.Interfaces;

    public class FallbackSettingsViewModel
    {
        public FallbackSettingsViewModel(IElement element)
        {
            this.Element = element;

            this.Settings = new ObservableCollection<ISettingViewModel>(element.GetPropertyBindings().Select<IPropertyBinding, ISettingViewModel>(
                bind =>
                    bind switch
                    {
                        IPropertyBinding<string> b => (ISettingViewModel)new StringSettingViewModel(b),
                        IPropertyBinding<double> b => (ISettingViewModel)new DoubleSettingViewModel(b),
                        IPropertyBinding<int> b => (ISettingViewModel)new IntSettingViewModel(b),
                        IPropertyBinding b => new ObjectSettingsViewModel(b)
                    }));
        }

        public IElement Element { get; }

        public ObservableCollection<ISettingViewModel> Settings { get; }
    }
}
