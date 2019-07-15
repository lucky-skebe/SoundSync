﻿// -----------------------------------------------------------------------
// <copyright file="IPipeSink.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Describes the basic interface of all elements that are a pure output.
    /// For custom sink elements you should usually use the <see cref="PipeSink"/> class.
    /// </summary>
    public interface IPipeSink : IPipeElement
    {
        /// <summary>
        /// Retrieves a Sink pad with a given name.
        /// </summary>
        /// <typeparam name="TValue">Type of the <see cref="PipeSinkPad{TValue}"/>.</typeparam>
        /// <param name="name">The name of the pad to retrieve.</param>
        /// <returns>The sink with the given name or null if either the name wasn't found or the type of the pad didn't match.</returns>
        PipeSinkPad<TValue>? GetSinkPad<TValue>(string name);
    }
}