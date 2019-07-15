// -----------------------------------------------------------------------
// <copyright file="State.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// State of the Pipeline or Element.
    /// Pretty Similar to the GStreamer State.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Playlist/Element is playing.
        /// Data is getting pushed throu it.
        /// </summary>
        Playing,

        /// <summary>
        /// Playlist/Element is ready.
        /// All Elements have finished their initalization.
        /// But no data is flowing yet.
        /// </summary>
        Ready,

        /// <summary>
        /// Playlist/Element is stopped.
        /// No data is flowing
        /// Elements may not be initialized yet or may have releases their resources.
        /// </summary>
        Stopped,
    }
}
