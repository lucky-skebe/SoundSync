// -----------------------------------------------------------------------
// <copyright file="PadNamedFormat.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using CStreamer.Base.Filter;

    internal class PadNamedFormat : IPadFormat
    {
        private readonly string formatName;

        public PadNamedFormat(string formatName)
        {
            this.formatName = formatName;
        }

        public bool CanAccept(IPadFormat format)
        {
            return format switch
            {
                PadNamedFormat named => named.formatName == this.formatName,
                _ => false
            };
        }

        public override string ToString()
        {
            return this.formatName;
        }
    }
}