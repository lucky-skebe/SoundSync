// -----------------------------------------------------------------------
// <copyright file="SettingsControl.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A Controls that shows Settings/Options for all Interactions of a given element.
    /// </summary>
    public class SettingsControl : Control
    {
        static SettingsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SettingsControl),
                new FrameworkPropertyMetadata(typeof(SettingsControl)));
        }
    }
}
