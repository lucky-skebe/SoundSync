using SharPipes.Pipes.Base;
using SharPipes.Pipes.Basic;
using SharPipes.Pipes.Buttplug;
using SharPipes.Pipes.NAudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharPipes.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<IPipeElement> PipeElements
        {
            get;
            set;
        }

        ButtplugSink output;

        public PipeLine Pipeline { get; set; }

        public MainWindow()
        {
            PipeElements = new List<IPipeElement>
            {
                new MultiplyElement()
            };

            this.Pipeline = new PipeLine();

            var input = new LoopBackSrc();
            var avg = new TimedAVGElement();
            var multiply = new MultiplyElement { Multiplier = 10 };
            output = new ButtplugSink();

            Pipeline.Connect(input.Src, avg.Sink);
            Pipeline.Connect(avg.Src, multiply.Sink);
            Pipeline.Connect(multiply.Src, output.Sink);

            this.Pipeline.AddNode(input);
            this.Pipeline.AddNode(avg);
            this.Pipeline.AddNode(multiply);
            // TODO clamp
            this.Pipeline.AddNode(output);


            this.DataContext = this;
            InitializeComponent();

        }

        private void ToggleToolBar(object sender, RoutedEventArgs e)
        {
            if (ToolBox.Visibility == Visibility.Visible)
            {
                ToolBox.Visibility = Visibility.Collapsed;
            }else
            {
                ToolBox.Visibility = Visibility.Visible;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await this.Pipeline.Start();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await output.StartScanning();
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            await this.Pipeline.Stop();
            base.OnClosing(e);
        }
    }
}
