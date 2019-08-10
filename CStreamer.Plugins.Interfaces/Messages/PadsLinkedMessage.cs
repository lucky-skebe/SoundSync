﻿// -----------------------------------------------------------------------
// <copyright file="PadsLinkedMessage.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces.Messages
{
    /// <summary>
    /// A Message signaling two pads being linked.
    /// </summary>
    /// <seealso cref="CStreamer.Plugins.Interfaces.Messages.Message" />
    public class PadsLinkedMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PadsLinkedMessage"/> class.
        /// </summary>
        /// <param name="srcPad">The source pad.</param>
        /// <param name="sinkPad">The sink pad.</param>
        public PadsLinkedMessage(ISrcPad srcPad, ISinkPad sinkPad)
        {
            this.SrcPad = srcPad;
            this.SinkPad = sinkPad;
        }

        /// <summary>
        /// Gets the source pad.
        /// </summary>
        /// <value>
        /// The source pad.
        /// </value>
        public ISrcPad SrcPad { get; }

        /// <summary>
        /// Gets the sink pad.
        /// </summary>
        /// <value>
        /// The sink pad.
        /// </value>
        public ISinkPad SinkPad { get; }
    }
}
