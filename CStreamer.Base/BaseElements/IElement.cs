// -----------------------------------------------------------------------
// <copyright file="IElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base.BaseElements
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using CStreamer.Base.Messages;

    /// <summary>
    /// Defines what members all elements need to implement.
    /// </summary>
    public interface IElement : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the parent bin / pipeline of this element.
        /// </summary>
        /// <value>
        /// The parent bin / pipeline of this element.
        /// </value>
        IBin? Parent { get; set; }

        /// <summary>
        /// Gets the current state of the element.
        /// </summary>
        /// <value>
        /// The current state of the element.
        /// </value>
        State CurrentState { get; }

        /// <summary>
        /// Tells the element to change to a specific State.
        /// Changed to all the inbetween states as well.
        /// </summary>
        /// <param name="newState">The State to change to.</param>
        /// <returns>A task that represents the state change operation.</returns>
        Task GoToState(State newState);

        /// <summary>
        /// Returns all pads of the element.
        /// </summary>
        /// <returns>All pads.</returns>
        IEnumerable<IPad> GetPads();

        /// <summary>
        /// Sends a Message to the Containing Bin/Pipeline.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(Message message);
    }
}
