// -----------------------------------------------------------------------
// <copyright file="IPipeElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SharPipes.Pipes.Base.InteractionInfos;
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    /// <summary>
    /// Defines what members all elements need to implement.
    /// </summary>
    public interface IPipeElement
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
        /// Gets the name of this kind of element.
        /// </summary>
        /// <value>
        /// The name of this kind of element.
        /// </value>
        [Obsolete("Use the ElementNameAttribute instead.")]
        string TypeName { get; }

        /// <summary>
        /// Gets the interactions this element supports.
        /// </summary>
        /// <value>
        /// The interactions this element supports.
        /// </value>
        IEnumerable<IInteraction> Interactions { get; }

        /// <summary>
        /// Tells the element to change to a specific State.
        /// Changed to all the inbetween states as well.
        /// </summary>
        /// <param name="newState">The State to change to.</param>
        /// <returns>A task that represents the state change operation.</returns>
        Task GoToState(State newState);

        /// <summary>
        /// Checks if the element can be started.
        /// </summary>
        /// <returns>The connectionstate of the element.
        /// <see cref="GraphState.OK"/> if all necessary pads are linked.
        /// <see cref="GraphState.INCOMPLETE"/> otherwise.</returns>
        [Obsolete("logic should be moved to pipeline.")]
        GraphState Check();

        /// <summary>
        /// Gets all the elements feeding into this one.
        /// </summary>
        /// <returns>All the elements feeding data into this one.</returns>
        IEnumerable<IPipeElement> GetPrevNodes();

        /// <summary>
        /// Returns all sink pads of the element.
        /// </summary>
        /// <returns>All sink pads.</returns>
        IEnumerable<IPipeSinkPad> GetSinkPads();

        /// <summary>
        /// Returns all src pads of the element.
        /// </summary>
        /// <returns>All src pads.</returns>
        IEnumerable<IPipeSrcPad> GetSrcPads();

        /// <summary>
        /// Gets all property values that should be serialized.
        /// </summary>
        /// <returns>All property values to serialize.</returns>
        IEnumerable<PropertyValue> GetPropertyValues();

        /// <summary>
        /// Sets a property based on a <see cref="PropertyValue"/>.
        /// </summary>
        /// <param name="propertyValze">The property value providing the value.</param>
        /// <returns>True if the value was able to be set. False otherwise.</returns>
        bool SetPropertyValue(PropertyValue propertyValze);

        /// <summary>
        /// Retrieves a Src pad with a given name.
        /// </summary>
        /// <param name="name">The name of the pad to retrieve.</param>
        /// <returns>The src with the given name or null if either the name wasn't found.</returns>
        IPipeSrcPad? GetSrcPad(string name);

        /// <summary>
        /// Retrieves a Sink pad with a given name.
        /// </summary>
        /// <param name="name">The name of the pad to retrieve.</param>
        /// <returns>The sink with the given name or null if either the name wasn't found.</returns>
        IPipeSinkPad? GetSinkPad(string name);
    }
}
