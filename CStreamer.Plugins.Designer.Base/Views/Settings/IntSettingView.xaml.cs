﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CStreamer.Plugins.Designer.Base.Views.Settings
{
    public class IntSettingView : UserControl
    {
        public IntSettingView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}