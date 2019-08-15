// -----------------------------------------------------------------------
// <copyright file="PadFilter.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.Filter
{
    /// <summary>
    /// Describes a filter on a format / contenttype pair.
    /// </summary>
    public class PadFilter
    {
        /// <summary>
        /// Gets or sets the format filter.
        /// </summary>
        /// <value>
        /// The format filter.
        /// </value>
        public IPadFormat Format { get; set; } = new PadAnyFormat();

        /// <summary>
        /// Gets or sets the content filter.
        /// </summary>
        /// <value>
        /// The content filter.
        /// </value>
        public IPadContent Content { get; set; } = new PadAnyContent();

        /// <summary>
        /// Determines whether this instance can accept the specified pad output.
        /// </summary>
        /// <param name="padOutput">The pad output.</param>
        /// <returns>
        ///   <c>true</c> if this instance can accept the specified pad output; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAccept(PadOutput padOutput)
        {
            return this.Format.CanAccept(padOutput.Format) && this.Content.CanAccept(padOutput.Content);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Format.ToString()}/{this.Content.ToString()}";
        }
    }
}
