// -----------------------------------------------------------------------
// <copyright file="IPadFormat.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.Filter
{
    /// <summary>
    /// Interface for all Pad Formats.
    /// </summary>
    public interface IPadFormat
    {
        /// <summary>
        /// Determines whether this instance can accept the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   <c>true</c> if this instance can accept the specified format; otherwise, <c>false</c>.
        /// </returns>
        bool CanAccept(IPadFormat format);
    }
}
