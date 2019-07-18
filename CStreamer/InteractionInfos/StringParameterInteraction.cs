// -----------------------------------------------------------------------
// <copyright file="StringParameterInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.InteractionInfos
{
    using System;

    /// <summary>
    /// Describes the Interaction with an Elements Property of type <see cref="string"/>.
    /// </summary>
    public class StringParameterInteraction : ValuePropertyInteraction<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringParameterInteraction"/> class.
        /// </summary>
        /// <param name="name">The name of the Interaction.</param>
        /// <param name="getValue">The method get get the propertys value.</param>
        /// <param name="setValue">The method get set the propertys value.</param>
        public StringParameterInteraction(string name, Func<string> getValue, Action<string> setValue)
            : base(name, getValue, setValue)
        {
        }
    }
}
