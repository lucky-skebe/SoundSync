// -----------------------------------------------------------------------
// <copyright file="IBin.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.BaseElements
{
    using CStreamer.Base.Messages;

    /// <summary>
    /// A Collection of Elements wich can be used like one complete element.
    /// </summary>
    public interface IBin
    {
        /// <summary>
        /// Receives a message.
        /// </summary>
        /// <param name="message">The message.</param>
        void ReceiveMessage(Message message);
    }
}
