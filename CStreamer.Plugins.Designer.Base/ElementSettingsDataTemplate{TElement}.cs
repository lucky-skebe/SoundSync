﻿// -----------------------------------------------------------------------
// <copyright file="ElementSettingsDataTemplate.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using CStreamer.Plugins.Interfaces;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    public abstract class ElementSettingsDataTemplate<TElement> : ElementSettingsDataTemplate
        where TElement : IElement
    {
        public abstract IControl Build(TElement element);

        public override IControl Build(object param)
        {
            return this.Build((TElement)param);
        }

        public override bool Match(object data)
        {
            return data is TElement;
        }
    }
}
