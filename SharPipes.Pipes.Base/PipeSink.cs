// -----------------------------------------------------------------------
// <copyright file="PipeSink.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Baseclass for all elements that only have sink pads.
    /// </summary>
    public abstract class PipeSink : PipeElement, IPipeSink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeSink"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected PipeSink(string? name = null)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public abstract PipeSinkPad<TValue>? GetSinkPad<TValue>(string name);

        /// <inheritdoc/>
        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            return Enumerable.Empty<IPipeSrcPad>();
        }

        /// <inheritdoc/>
        protected override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
