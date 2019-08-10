// -----------------------------------------------------------------------
// <copyright file="PadOutput.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    /// <summary>
    /// Describes a Output format / content a pad can produce.
    /// </summary>
    public class PadOutput
    {
        /// <summary>
        /// Gets or sets the format description.
        /// </summary>
        /// <value>
        /// The format description.
        /// </value>
        public IPadFormat Format { get; set; } = PadFormat.Any();

        /// <summary>
        /// Gets or sets the content description.
        /// </summary>
        /// <value>
        /// The content description.
        /// </value>
        public IPadContent Content { get; set; } = PadContent.Any();

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Format.ToString()}/{this.Content.ToString()}";
        }
    }
}