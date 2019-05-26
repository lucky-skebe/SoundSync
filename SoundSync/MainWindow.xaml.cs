using System;

using MahApps.Metro.Controls;
using SoundSync.models;

namespace SoundSync
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Global model = new Global();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = model;

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.Dispose();
        }
    }
}
