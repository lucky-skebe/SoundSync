using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia
{
    public struct Notification
    {
        public Notification(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}
