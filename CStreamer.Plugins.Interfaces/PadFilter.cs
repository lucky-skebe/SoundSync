// -----------------------------------------------------------------------
// <copyright file="PadFilter.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    public class PadFilter
    {
        public IPadFormat Format { get; set; }

        public IPadContent Content { get; set; }

        public bool CanAccept(PadOutput padOutput)
        {
            return Format.CanAccept(padOutput.Format) && Content.CanAccept(padOutput.Content);
        }

        public override string ToString()
        {
            return $"{Format.ToString()}/{Content.ToString()}";
        }
    }
}
