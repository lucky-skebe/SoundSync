// -----------------------------------------------------------------------
// <copyright file="CompositeSrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CStreamer.Plugins.Interfaces;
    using CStreamer.Plugins.Interfaces.Messages;
    using Optional;

    /// <summary>
    /// A SrcPad that Consists of multiple Childpads.
    /// </summary>
    /// <remarks>
    /// This Pad is used to support more than one InputType / Format.
    /// To do so add pads for all supportrd InputTypes / Formats indo the ChildPads list.
    /// The Childpads chould be ordered from least taxing conversion to most taxing conversion since the Pads will be tried during linking in the order of the list.
    /// </remarks>
    public class CompositeSrcPad : ICompositeSrcPad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSrcPad"/> class.
        /// </summary>
        /// <param name="parent">the element this pad is connected to.</param>
        /// <param name="name">the name of the pad.</param>
        /// <param name="childPads">A list of acceptable Childpads.</param>
        /// <param name="mandatory">A value indicating whether the Pad needs to be linked for the element to be functional.</param>
        public CompositeSrcPad(IElement parent, string name, List<ISrcPad> childPads, bool mandatory)
        {
            this.Name = name;
            this.ChildPads = childPads;
            this.Parent = parent;
            this.Mandatory = mandatory;
        }

        /// <inheritdoc/>
        public List<ISrcPad> ChildPads { get; }

        /// <inheritdoc/>
        public ISinkPad? Peer => this.ChildPads.Select(pad => pad.Peer).FirstOrDefault(pad => pad != null);

        /// <inheritdoc/>
        public IElement Parent
        {
            get;
            protected set;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public bool Mandatory { get; }

        IPad? IPad.Peer => this.Peer;

        /// <inheritdoc/>
        public string Caps => this.ChildPads.Select(pad => pad.Caps).Aggregate<string, StringBuilder>(new StringBuilder(), (sb, s) => sb.AppendLine(s)).ToString();

        /// <inheritdoc/>
        public bool Equals(ISrcPad other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Parent.Equals(other.Parent) && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <inheritdoc/>
        public bool IsLinked()
        {
            return this.ChildPads.Any(pad => pad.IsLinked());
        }

        /// <inheritdoc/>
        public Option<ISinkPad, string> Link(ISinkPad peer)
        {
            foreach (var childPad in this.ChildPads)
            {
                var result = childPad.Link(peer);
                if (result.HasValue)
                {
                    this.Parent.SendMessage(new PadsLinkedMessage(this, peer));
                    return result;
                }
            }

            return Option.None<ISinkPad, string>("No matching Pad could be found");
        }

        /// <inheritdoc/>
        public Option<IPad, string> Link(IPad peer)
        {
            foreach (var childPad in this.ChildPads)
            {
                var result = childPad.Link(peer);
                if (result.HasValue)
                {
                    this.Parent.SendMessage(new PadsLinkedMessage(this, (ISinkPad)peer));
                    return result;
                }
            }

            return Option.None<IPad, string>("No matching Pad could be found");
        }

        /// <inheritdoc/>
        public void Unlink()
        {
            foreach (var pad in this.ChildPads)
            {
                var peer = pad.Peer;
                if (peer != null)
                {
                    pad.Unlink();
                    this.Parent.SendMessage(new PadsUnlinkedMessage(this, peer));
                }
            }
        }
    }
}
