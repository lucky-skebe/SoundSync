// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels
{
    using ReactiveUI;

    internal class ViewModelBase : ReactiveObject, ISupportsActivation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        public ViewModelBase()
        {
            this.Activator = new ViewModelActivator();
        }

        /// <inheritdoc/>
        public ViewModelActivator Activator { get; }
    }
}