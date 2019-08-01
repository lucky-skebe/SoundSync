// -----------------------------------------------------------------------
// <copyright file="StringSettingViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels.Settings
{
    using CStreamer.Plugins.Interfaces;

    /// <summary>
    /// A Simple ViewModel that contains a Name and a Value.
    /// Used for presenting a simple Settings Control.
    /// </summary>
    internal class StringSettingViewModel : BaseSettingViewModel<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSettingViewModel"/> class.
        /// </summary>
        /// <param name="binding">The underlying PropertyBinding.</param>
        public StringSettingViewModel(IPropertyBinding<string> binding)
            : base(binding)
        {
        }
    }
}
