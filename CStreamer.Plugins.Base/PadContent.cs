// -----------------------------------------------------------------------
// <copyright file="PadContent.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using CStreamer.Plugins.Interfaces;

    public static class PadContent
    {
        public static IPadContent Any()
        {
            return new PadAnyContent();
        }

        public static IPadContent OfName(string formatName)
        {
            return new PadNamedContent(formatName);
        }

    }
}
