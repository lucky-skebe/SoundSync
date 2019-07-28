using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CStreamer.Designer.Avalonia
{
    public struct Notification : IEquatable<Notification>
    {
        public Notification(string text)
        {
            this.Text = text;
        }

        public string Text { get; }

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
            return (this.Text).GetHashCode(StringComparison.InvariantCulture);
        }

        public static bool operator ==(Notification left, Notification right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Notification left, Notification right)
        {
            return !(left == right);
        }
    }
}
