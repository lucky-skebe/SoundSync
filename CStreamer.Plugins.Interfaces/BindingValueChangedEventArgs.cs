// -----------------------------------------------------------------------
// <copyright file="BindingValueChangedEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BindingValueChangedEventArgs
    {
        public BindingValueChangedEventArgs(object? newValue)
        {
            this.NewValue = newValue;
        }

        public object? NewValue { get; }
    }
}
