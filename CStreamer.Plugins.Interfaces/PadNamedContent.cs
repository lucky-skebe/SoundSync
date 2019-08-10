// -----------------------------------------------------------------------
// <copyright file="PadNamedContent.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    internal class PadNamedContent : IPadContent
    {
        private readonly string contentName;

        public PadNamedContent(string contentName)
        {
            this.contentName = contentName;
        }

        public bool CanAccept(IPadContent content)
        {
            return content switch
            {
                PadNamedContent named => named.contentName == this.contentName,
                _ => false
            };
        }

        public override string ToString()
        {
            return this.contentName;
        }
    }
}