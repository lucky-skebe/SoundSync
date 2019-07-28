// -----------------------------------------------------------------------
// <copyright file="ButtplugDataTemplate.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Buttplug
{
    using CStreamer.Plugins.Buttplug;
    using CStreamer.Plugins.Designer.Base;
    using global::Avalonia.Controls;

    public class ButtplugDataTemplate : ElementSettingsDataTemplate<ButtplugSink>
    {
        public override IControl Build(ButtplugSink element)
        {
            return new ButtPlugSettingsView { DataContext = new ButtplugSinkViewModel(element) };
        }
    }
}
