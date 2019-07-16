// -----------------------------------------------------------------------
// <copyright file="ITransformElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Defines what members element with both src and sink pads should implement.
    /// </summary>
    public interface ITransformElement : ISinkElement, ISrcElement
    {
    }
}