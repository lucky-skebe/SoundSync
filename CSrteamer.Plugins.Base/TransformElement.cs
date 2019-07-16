// -----------------------------------------------------------------------
// <copyright file="TransformElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Base class for elemnts that have both sink and src pads.
    /// </summary>
    public abstract class TransformElement : Element, ITransformElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeTransform"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected TransformElement(string? name = null)
            : base(name)
        {
        }

        public abstract ISinkPad<TValue>? GetSinkPad<TValue>(string name);
        public abstract ISinkPad? GetSinkPad(string name);
        public abstract IEnumerable<ISinkPad> GetSinkPads();
        public abstract ISrcPad<TValue>? GetSrcPad<TValue>(string name);
        public abstract ISrcPad? GetSrcPad(string name);
        public abstract IEnumerable<ISrcPad> GetSrcPads();
    }
}
