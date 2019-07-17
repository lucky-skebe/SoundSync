// -----------------------------------------------------------------------
// <copyright file="IPipeElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using Optional;
    using SharPipes.Pipes.Base.PipeLineDefinitions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines what members all elements need to implement.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        string Name { get; }

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
        /// Gets a list of all Property bindings that should be serialized/deserialized.
        /// </summary>
        /// <returns>List of all the PropertyBindings of hte element.</returns>
        IEnumerable<IPropertyBinding> GetPropertyBindings();

        /// <summary>
        /// Returns all pads of the element.
        /// </summary>
        /// <returns>All pads.</returns>
        IEnumerable<IPad> GetPads();
    }
}
