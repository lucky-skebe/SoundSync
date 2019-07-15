// -----------------------------------------------------------------------
// <copyright file="IInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.InteractionInfos
{
    /// <summary>
    /// Defines an Interaction with an Element.
    /// Examples:
    /// Properties
    /// Connecting to a Server.
    /// ...
    /// </summary>
    public interface IInteraction
    {
        /// <summary>
        /// Gets the name of the Interaction.
        /// This name could be used to Label a button for Example.
        /// </summary>
        /// <value>
        /// The name of the Interaction.
        /// </value>
        string Name { get; }
    }
}
