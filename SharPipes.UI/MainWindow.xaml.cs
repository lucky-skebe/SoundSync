using Newtonsoft.Json;
using SharPipes.Pipes.Base;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using SharPipes.Pipes.Basic;
using SharPipes.Pipes.Buttplug;
using SharPipes.Pipes.NAudio;
using SharPipes.UI.GraphicalDecorators;
using SharPipes.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Loader;
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

        private Point startPoint;
        private Vector offset;

        public GraphicalPipeline Pipeline { get; set; }

        public MainWindow()
        {
            PipeElements = new List<IPipeElement>
            {
            };

            foreach (var assembly in AssemblyLoadContext.Default.Assemblies)
            {
                foreach(var type in assembly.GetTypes())
                {
                    if(typeof(IPipeElement).IsAssignableFrom(type))
                    {
                        if(type.IsClass && !type.IsAbstract)
                        {
                            PipeElements.Add((IPipeElement)Activator.CreateInstance(type, "test"));
                        }
                    }
                }
            }

            this.startPoint = new Point();

            this.Pipeline = new GraphicalPipeline(new PipeLine());

            var input = new LoopBackSrc();
            var avg = new TimedAVGElement();
            var multiply = new MultiplyElement { Multiplier = 10 };
            var output = new ButtplugSink();

            this.Pipeline.AddNode(input, new Point(100, 100));
            this.Pipeline.AddNode(avg, new Point(250, 200));
            this.Pipeline.AddNode(multiply, new Point(400, 100));
            // TODO clamp
            this.Pipeline.AddNode(output, new Point(550, 100));

            Pipeline.Connect(input.Src, avg.Sink);
            Pipeline.Connect(avg.Src, multiply.Sink);
            Pipeline.Connect(multiply.Src, output.Sink);

            this.DataContext = this;
            InitializeComponent();
        }

        private void ToggleToolBar(object sender, RoutedEventArgs e)
        {
            if (ToolBox.Visibility == Visibility.Visible)
            {
                ToolBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                ToolBox.Visibility = Visibility.Visible;
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await this.Pipeline.Start();
        }


        protected override async void OnClosing(CancelEventArgs e)
        {
            await this.Pipeline.Stop();
            base.OnClosing(e);
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("fromToolBar"))
            {
                if (e.Data.GetData("fromToolBar") is IPipeElement template)
                {
                    this.Pipeline.CreateNodeFromTemplate(template, e.GetPosition(sender as IInputElement) - offset);
                }
            }
            else if (e.Data.GetDataPresent("drawEdge"))
            {
                if (e.Data.GetData("drawEdge") is GraphicalSrcPad src)
                {
                    PipeLineItem? listViewItem =
                        ((DependencyObject)e.OriginalSource).FindAnchestor<PipeLineItem>();

                    if (!(sender is PipeLineRenderer listView) || listViewItem == null)
                    {
                        return;
                    }

                    // Find the data behind the ListViewItem

                    if (listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem) is GraphicalSinkPad sink)
                    {
                        this.Pipeline.TryConnect(src, sink);
                    }
                }
            }
            else if (e.Data.GetDataPresent("moveElement"))
            {
                if (e.Data.GetData("moveElement") is GraphicalElement element)
                {
                    Point newPos = e.GetPosition(sender as IInputElement) - offset;

                    element.MoveTo(newPos);
                }

            }
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private bool isDragging = false;
        DataObject? dragData = null;
        DependencyObject? dragSource;

        private void HandleDragStart<TContainer, TItem, TValue>(MouseButtonEventArgs e, string format) where TContainer : ItemsControl where TItem : DependencyObject, IInputElement
        {
            TItem? listViewItem =
                ((DependencyObject)e.OriginalSource).FindAnchestor<TItem>();
            TContainer? listView = ((DependencyObject)e.OriginalSource).FindAnchestor<TContainer>();

            if (listViewItem == null || listView == null)
            {
                return;
            }

            startPoint = e.GetPosition(null);
            isDragging = false;

            var item = listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

            if (item is TValue element)
            {
                offset = e.GetPosition(listViewItem) - new Point();
                this.dragData = new DataObject(format, element);
                this.dragSource = listViewItem;
            }
        }

        private void HandlePreviewMouseMove(MouseEventArgs e)
        {
            if (isDragging || dragSource == null || dragData == null || this.startPoint == null)
            {
                return;
            }

            Point mousePos = e.GetPosition(null);
            
            Vector diff = this.startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed && 
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                isDragging = true;
                
                DragDrop.DoDragDrop(this.dragSource, dragData, DragDropEffects.Move);
            }
        }



        private void PipeLineRenderer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            dragSource = null;
            dragData = null;
            HandleDragStart<PipeLineRenderer, PipeLineItem, GraphicalElement>(e, "moveElement");
            HandleDragStart<PipeLineRenderer, PipeLineItem, GraphicalSrcPad>(e, "drawEdge");
        }

        private void PipeLineRenderer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            HandlePreviewMouseMove(e);
        }

        private void ToolBoxList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            dragSource = null;
            dragData = null;
            HandleDragStart<ListView, ListViewItem, IPipeElement>(e, "fromToolBar");
        }

        private void ToolBoxList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            HandlePreviewMouseMove(e);
        }

        private GraphicalPipeLineDefinition definition;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.definition = this.Pipeline.GetDefinition();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            this.Pipeline.FromDefinition(this.definition);
        }
    }
}
