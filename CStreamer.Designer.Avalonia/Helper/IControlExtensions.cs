// -----------------------------------------------------------------------
// <copyright file="IControlExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Helper
{
    using global::Avalonia.Controls;

    public static class IControlExtensions
    {
        public static T? FindAnchestor<T>(this IControl current)
            where T : class, IControl
        {
            if (current == null)
            {
                return null;
            }

            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = current.Parent;
            }
            while (current != null);
            return null;
        }
    }
}
