// -----------------------------------------------------------------------
// <copyright file="ElementSettingsDataTemplate.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using CStreamer.Base.BaseElements;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    /// <summary>
    /// A Baseclass used for all simple Properties.
    /// Inherit this if your element uses a property that isn't Covered by the pre implemented SettingViewModels.
    /// </summary>
    public abstract class ElementSettingsDataTemplate : IDataTemplate
    {
        /// <inheritdoc/>
        public bool SupportsRecycling => false;

        /// <inheritdoc/>
        public abstract IControl Build(object element);

        /// <inheritdoc/>
        public virtual bool Match(object data)
        {
            return data is IElement;
        }
    }
}
