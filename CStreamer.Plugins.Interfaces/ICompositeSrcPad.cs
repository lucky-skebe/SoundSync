// -----------------------------------------------------------------------
// <copyright file="ICompositeSrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System.Collections.Generic;

    public interface ICompositeSrcPad : ISrcPad
    {
        public List<ISrcPad> ChildPads { get; }
    }
}
