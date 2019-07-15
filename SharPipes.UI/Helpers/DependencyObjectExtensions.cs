// -----------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides extensionmethods for <see cref="DependencyObject"/> objects.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Finds the first visual parent of a certain type.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="current">The visual element to start the search at.</param>
        /// <returns>The first parent of a given type or null if no element was found.</returns>
        public static T? FindAnchestor<T>(this DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
