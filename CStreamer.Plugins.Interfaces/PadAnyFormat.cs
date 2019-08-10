// -----------------------------------------------------------------------
// <copyright file="PadAnyFormat.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    internal class PadAnyFormat : IPadFormat
    {
        public bool CanAccept(IPadFormat format)
        {
            return true;
        }

        public override string ToString()
        {
            return "*";
        }
    }
}
