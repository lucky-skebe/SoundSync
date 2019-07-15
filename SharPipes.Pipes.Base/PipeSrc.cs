// -----------------------------------------------------------------------
// <copyright file="PipeSrc.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Baseclass for all elements that only have src pads.
    /// </summary>
    public abstract class PipeSrc : PipeElement, IPipeSrc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeSrc"/> class.
        /// </summary>
        /// <param name="name">the name of the element.</param>
        protected PipeSrc(string? name = null)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public override IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            return Enumerable.Empty<IPipeSinkPad>();
        }

        /// <inheritdoc/>
        public abstract PipeSrcPad<TValue>? GetSrcPad<TValue>(string name);

        /// <inheritdoc/>
        protected override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
