// -----------------------------------------------------------------------
// <copyright file="SrcElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System.Collections.Generic;

    /// <summary>
    /// Baseclass for all elements that only have src pads.
    /// </summary>
    public abstract class SrcElement : Element, ISrcElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeSrc"/> class.
        /// </summary>
        /// <param name="name">the name of the element.</param>
        protected SrcElement(string? name = null)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public abstract ISrcPad<TValue>? GetSrcPad<TValue>(string name);

        public abstract ISrcPad? GetSrcPad(string name);

        public abstract IEnumerable<ISrcPad> GetSrcPads();
    }
}
