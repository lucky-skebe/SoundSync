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

    /// <summary>
    /// A Baseclass to generate custom settingsviews for entire elements.
    ///
    /// Use this if you need a more complex settings view than a list of textboxes/numericinputs.
    /// </summary>
    /// <typeparam name="TElement">The type of Element this creates SettingsViews for.</typeparam>
    public abstract class ElementSettingsDataTemplate<TElement> : ElementSettingsDataTemplate
        where TElement : IElement
    {
        /// <summary>
        /// Creates a SettingView given an Element.
        /// </summary>
        /// <param name="element">The Element to create teh SettingView for.</param>
        /// <returns>The created SettingsView.</returns>
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
