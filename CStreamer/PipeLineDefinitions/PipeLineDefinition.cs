// -----------------------------------------------------------------------
// <copyright file="PipeLineDefinition.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.PipeLineDefinitions
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes the structure of an entire <see cref="PipeLine"/>.
    /// Used for serialization/deserialization.
    /// </summary>
    public class PipeLineDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeLineDefinition"/> class.
        /// </summary>
        public PipeLineDefinition()
            : this(new List<ElementDefinition>(), new List<LinkDefinition>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeLineDefinition"/> class.
        /// </summary>
        /// <param name="elements">All element the described pipeline contains.</param>
        /// <param name="links">All links the described pipeline contains.</param>
        public PipeLineDefinition(IList<ElementDefinition> elements, IList<LinkDefinition> links)
        {
            this.Elements = elements;
            this.Links = links;
        }

        /// <summary>
        /// Gets all element the described pipeline contains.
        /// </summary>
        /// <value>
        /// All element the described pipeline contains.
        /// </value>
        public IList<ElementDefinition> Elements { get; }

        /// <summary>
        /// Gets all links the described pipeline contains.
        /// </summary>
        /// <value>
        /// All links the described pipeline contains.
        /// </value>
        public IList<LinkDefinition> Links { get; }
    }
}
