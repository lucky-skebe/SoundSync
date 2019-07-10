using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SharPipes.UI
{
    class SettingsControl : Control
    {
        static SettingsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SettingsControl),
                new FrameworkPropertyMetadata(typeof(SettingsControl)));
        }
    }
}
