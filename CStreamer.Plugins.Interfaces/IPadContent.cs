// -----------------------------------------------------------------------
// <copyright file="IPadContent.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    /// <summary>
    /// Interface for all pad contents.
    /// </summary>
    public interface IPadContent
    {
        /// <summary>
        /// Determines whether this instance can accept the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if this instance can accept the specified input; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAccept(IPadContent input);
    }
}
