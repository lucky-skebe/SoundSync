﻿// -----------------------------------------------------------------------
// <copyright file="ButtPlugSettingsView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Buttplug
{
    using System;
    using System.Linq;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;
    using ReactiveUI;

    /// <summary>
    /// A SettingsView that let'S you controll the connection to a Buttplug Server.
    /// </summary>
    public class ButtPlugSettingsView : ReactiveUserControl<ButtplugSinkViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtPlugSettingsView"/> class.
        /// </summary>
        public ButtPlugSettingsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(() =>
            {
                return Enumerable.Empty<IDisposable>();
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
