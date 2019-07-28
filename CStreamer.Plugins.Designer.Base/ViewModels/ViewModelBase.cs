﻿// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.ViewModels
{
    using ReactiveUI;

    public class ViewModelBase : ReactiveObject, ISupportsActivation
    {
        public ViewModelBase()
        {
            this.Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}