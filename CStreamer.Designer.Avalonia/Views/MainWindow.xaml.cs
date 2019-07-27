﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;
using CStreamer.Plugins.Designer.Base;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CStreamer.Designer.Avalonia.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
        }



        private void InitializeComponent()
        {
            this.WhenActivated(Enumerable.Empty<IDisposable>);
            AvaloniaXamlLoader.Load(this);
        }
    }
}
