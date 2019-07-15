// -----------------------------------------------------------------------
// <copyright file="GraphState.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// The state teh pipelines Graph is in.
    ///
    /// If the graph is either incomplete or containes cycles.
    /// </summary>
    public enum GraphState
    {
        /// <summary>
        /// The Graph is fully linked and non cyclical
        /// </summary>
        OK,

        /// <summary>
        /// The Graph is not completely linked
        /// </summary>
        INCOMPLETE,

        /// <summary>
        /// The graph contains a cycle that needs to be resolved
        /// </summary>
        CYCLE,
    }
}
