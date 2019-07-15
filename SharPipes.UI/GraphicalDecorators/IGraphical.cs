// -----------------------------------------------------------------------
// <copyright file="IGraphical.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Describes Something that can be positioned inside a Canvas.
    /// </summary>
    public interface IGraphical : IEquatable<IGraphical>
    {
        /// <summary>
        /// Gets an unique ID used for identifying IGraphicals.
        /// </summary>
        /// <value>
        /// An unique ID used for identifying IGraphicals.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the Y position of the control.
        /// </summary>
        /// <value>
        /// The Y position of the control.
        /// </value>
        double X { get; }

        /// <summary>
        /// Gets the X position of the control.
        /// </summary>
        /// <value>
        /// The X position of the control.
        /// </value>
        double Y { get; }

        /// <summary>
        /// Gets the <see cref="Panel.ZIndexProperty" /> this element should be rendered at.
        /// </summary>
        /// <value>
        /// The <see cref="Panel.ZIndexProperty" /> this element should be rendered at.
        /// </value>
        int ZIndex { get; }
    }
}
