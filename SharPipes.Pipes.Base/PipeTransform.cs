// -----------------------------------------------------------------------
// <copyright file="PipeTransform.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Base class for elemnts that have both sink and src pads.
    /// </summary>
    public abstract class PipeTransform : PipeElement, IPipeTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeTransform"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected PipeTransform(string? name = null)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public abstract PipeSinkPad<TValue>? GetSinkPad<TValue>(string name);

        /// <inheritdoc/>
        public abstract PipeSrcPad<TValue>? GetSrcPad<TValue>(string name);
    }
}
