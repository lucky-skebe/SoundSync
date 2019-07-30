// -----------------------------------------------------------------------
// <copyright file="ElementSettingsDataTemplate{TElement}.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using CStreamer.Plugins.Interfaces;
    using global::Avalonia.Controls;

    public abstract class ElementSettingsDataTemplate<TElement> : ElementSettingsDataTemplate
        where TElement : IElement
    {
        public abstract IControl Build(TElement element);

        /// <inheritdoc/>
        public override IControl Build(object param)
        {
            return this.Build((TElement)param);
        }

        /// <inheritdoc/>
        public override bool Match(object data)
        {
            return data is TElement;
        }
    }
}
