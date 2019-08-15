// -----------------------------------------------------------------------
// <copyright file="FallbackDataTemplate.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using CStreamer.Base.BaseElements;
    using CStreamer.Plugins.Designer.Base.ViewModels;
    using CStreamer.Plugins.Designer.Base.Views;

    internal class FallbackDataTemplate : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
#pragma warning disable CS8604 // Mögliches Nullverweisargument.
            return new FallbackSettingsView() { DataContext = new FallbackSettingsViewModel(data as IElement) };
#pragma warning restore CS8604 // Mögliches Nullverweisargument.
        }

        public bool Match(object data)
        {
            return data is IElement;
        }
    }
}
