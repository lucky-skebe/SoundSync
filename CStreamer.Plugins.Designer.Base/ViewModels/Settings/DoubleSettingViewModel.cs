// -----------------------------------------------------------------------
// <copyright file="DoubleSettingViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using CStreamer.Plugins.Interfaces;

    public class DoubleSettingViewModel : BaseSettingViewModel<double>
    {
        public DoubleSettingViewModel(IPropertyBinding<double> binding)
            : base(binding)
        {
        }
    }
}
