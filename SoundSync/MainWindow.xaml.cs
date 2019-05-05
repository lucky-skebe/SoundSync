using System;

using MahApps.Metro.Controls;

namespace SoundSync
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Model model = new Model();

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
