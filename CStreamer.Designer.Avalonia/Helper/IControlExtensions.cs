using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CStreamer.Designer.Avalonia.Helper
{
    static class IControlExtensions
    {
        public static T? FindAnchestor<T>(this IControl current)
            where T : class, IControl
        {
            do
            {
                Debug.WriteLine(current);
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
