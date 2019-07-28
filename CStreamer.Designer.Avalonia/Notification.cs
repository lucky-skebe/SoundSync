// -----------------------------------------------------------------------
// <copyright file="Notification.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public struct Notification : IEquatable<Notification>
    {
        public Notification(string text)
        {
            this.Text = text;
        }

        public string Text { get; }

        public static bool operator ==(Notification left, Notification right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Notification left, Notification right)
        {
            return !(left == right);
        }

        public bool Equals([AllowNull] Notification other)
        {
            return this.Text.Equals(other.Text, StringComparison.InvariantCulture);
        }

        public override bool Equals([AllowNull] object? obj)
        {
            if (obj is Notification other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode(StringComparison.InvariantCulture);
        }
    }
}
