// -----------------------------------------------------------------------
// <copyright file="LinkDefinition.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    /// <summary>
    /// Describes a link between two pads.
    /// Used for serialization / deserialization.
    /// </summary>
    public class LinkDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkDefinition"/> class.
        /// </summary>
        /// <param name="fromElement">The name of the element this link connects from.</param>
        /// <param name="fromPad">The name of the <see cref="FromElement"/> pad.</param>
        /// <param name="toElement">The name of the element this link connects to.</param>
        /// <param name="toPad">The name of the <see cref="ToElement"/> element pad.</param>
        public LinkDefinition(string fromElement, string fromPad, string toElement, string toPad)
        {
            this.FromElement = fromElement;
            this.FromPad = fromPad;
            this.ToElement = toElement;
            this.ToPad = toPad;
        }

        /// <summary>
        /// Gets the name of the element this link connects from.
        /// </summary>
        /// <value>
        /// The name of the element this link connects from.
        /// </value>
        public string FromElement { get; }

        /// <summary>
        /// Gets the name of the <see cref="FromElement"/> pad.
        /// </summary>
        /// <value>
        /// The name of the <see cref="FromElement"/> pad.
        /// </value>
        public string FromPad { get; }

        /// <summary>
        /// Gets the name of the element this link connects to.
        /// </summary>
        /// <value>
        /// The name of the element this link connects to.
        /// </value>
        public string ToElement { get; }

        /// <summary>
        /// Gets the name of the <see cref="ToElement"/> element pad.
        /// </summary>
        /// <value>
        /// The name of the <see cref="ToElement"/> element pad.
        /// </value>
        public string ToPad { get; }
    }
}
