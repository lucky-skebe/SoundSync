// -----------------------------------------------------------------------
// <copyright file="ICStreamerViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    internal interface ICStreamerViewModel
    {
        double X { get; }

        double Y { get; }

        int ZIndex { get; }
    }
}
