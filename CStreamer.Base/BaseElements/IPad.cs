// -----------------------------------------------------------------------
// <copyright file="IPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.BaseElements
{
    using Optional;

    /// <summary>
    /// Describes the minimal requirements of a pad.
    /// </summary>
    public interface IPad
    {
        /// <summary>
        /// Gets the element this pad is a part of.
        /// </summary>
        /// <value>
        /// The element this pad is a part of.
        /// </value>
        IElement Parent
        {
            get;
        }

        /// <summary>
        /// Gets the Name of the pad.
        /// </summary>
        /// <value>
        /// The Name of the pad.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the Capabilities of the Pad.
        /// </summary>
        /// <value>
        /// The Capabilities of the Pad.
        /// </value>
        string Caps { get; }

        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        IPad? Peer { get; }

        /// <summary>
        /// Gets a value indicating whether the Pad needs to be linked for the element to be functional.
        /// </summary>
        /// <value>
        /// A value indicating whether the Pad needs to be linked for the element to be functional.
        /// </value>
        bool Mandatory { get; }

        /// <summary>
        /// Returns the linking status of the pad.
        /// </summary>
        /// <returns>true if the pad is linked, false otherwise.</returns>
        bool IsLinked();

        /// <summary>
        /// Unliks the pad from it's peer pad.
        /// </summary>
        void Unlink();

        /// <summary>
        /// Links this Pad to the provided peer pad.
        /// </summary>
        /// <param name="peer">the peerpad to link to.</param>
        /// <returns>An <see cref="Option{T, TException}"/> contianing either the new peer or an error message.</returns>
        Option<IPad, string> Link(IPad peer);
    }
}