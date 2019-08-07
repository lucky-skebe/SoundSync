// -----------------------------------------------------------------------
// <copyright file="PadFormat.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using CStreamer.Plugins.Interfaces;

    public static class PadFormat
    {
        public static IPadFormat Any()
        {
            return new PadAnyFormat();
        }

        public static IPadFormat OfName(string formatName)
        {
            return new PadNamedFormat(formatName);
        }

    }
}
