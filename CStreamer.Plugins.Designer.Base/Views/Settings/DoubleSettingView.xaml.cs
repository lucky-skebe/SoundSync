// -----------------------------------------------------------------------
// <copyright file="DoubleSettingView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.Views.Settings
{
    using global::Avalonia.Controls;
    using global::Avalonia.Markup.Xaml;

    /// <summary>
    /// A simple view for changing <see cref="double"/> values.
    /// </summary>
    public class DoubleSettingView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSettingView"/> class.
        /// </summary>
        public DoubleSettingView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
