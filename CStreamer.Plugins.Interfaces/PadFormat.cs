// -----------------------------------------------------------------------
// <copyright file="PadFormat.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    /// <summary>
    /// Describes a format a pad can either consume of produce.
    /// </summary>
    public static class PadFormat
    {
        /// <summary>
        /// Creates a non specified format.
        /// </summary>
        /// <returns>The created Format.</returns>
        public static IPadFormat Any()
        {
            return new PadAnyFormat();
        }

        /// <summary>
        /// Creates a format with a specific name.
        /// </summary>
        /// <param name="formatName">Name of the format.</param>
        /// <returns>The created Format.</returns>
        public static IPadFormat OfName(string formatName)
        {
            return new PadNamedFormat(formatName);
        }
    }
}
