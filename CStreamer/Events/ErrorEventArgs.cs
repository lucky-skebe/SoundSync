// -----------------------------------------------------------------------
// <copyright file="ErrorEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Events
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="PipeLine.Error" /> event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ErrorEventArgs(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; }
    }
}
