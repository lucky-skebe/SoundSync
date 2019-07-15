// -----------------------------------------------------------------------
// <copyright file="IPipeLink.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Describes the link between two pads.
    /// </summary>
    public interface IPipeLink
    {
        /// <summary>
        /// Gets the srs side pad of the link.
        /// </summary>
        /// <value>
        /// The srs side pad of the link.
        /// </value>
        public IPipeSrcPad Src { get; }

        /// <summary>
        /// Gets the sink side pad of the link.
        /// </summary>
        /// <value>
        /// The sink side pad of the link.
        /// </value>
        public IPipeSinkPad Sink { get; }

        /// <summary>
        /// Unlinks the elements connected by this link.
        /// </summary>
        public void Unlink();
    }
}
