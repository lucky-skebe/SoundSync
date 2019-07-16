// -----------------------------------------------------------------------
// <copyright file="SinkElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
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
    public abstract class SinkElement : Element, ISinkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeSink"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected SinkElement(string? name = null)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public abstract ISinkPad<TValue>? GetSinkPad<TValue>(string name);

        public abstract ISinkPad? GetSinkPad(string name);

        public abstract IEnumerable<ISinkPad> GetSinkPads();
    }
}
