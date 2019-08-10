// -----------------------------------------------------------------------
// <copyright file="ErrorMessage.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces.Messages
{
    /// <summary>
    /// A <see cref="Message"/> that some kind of Error has occured.
    /// </summary>
    /// <seealso cref="CStreamer.Plugins.Interfaces.Messages.Message" />
    public class ErrorMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="errorText">The error text.</param>
        public ErrorMessage(string errorText)
        {
            this.ErrorText = errorText;
        }

        /// <summary>
        /// Gets the error text.
        /// </summary>
        /// <value>
        /// The error text.
        /// </value>
        public string ErrorText { get; }
    }
}
