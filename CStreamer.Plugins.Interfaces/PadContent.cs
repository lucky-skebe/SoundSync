// -----------------------------------------------------------------------
// <copyright file="PadContent.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    /// <summary>
    /// Describes a content type a pad can either consume of produce.
    /// </summary>
    public static class PadContent
    {
        /// <summary>
        /// Creates a non specified content type.
        /// </summary>
        /// <returns>The created content type.</returns>
        public static IPadContent Any()
        {
            return new PadAnyContent();
        }

        /// <summary>
        /// Creates a content type with a specific name.
        /// </summary>
        /// <param name="formatName">Name of the content type.</param>
        /// <returns>The created Format.</returns>
        public static IPadContent OfName(string formatName)
        {
            return new PadNamedContent(formatName);
        }
    }
}
