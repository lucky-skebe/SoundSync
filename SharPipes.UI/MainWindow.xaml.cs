// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Loader;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Newtonsoft.Json;
    using SharPipes.Pipes.Base;
    using SharPipes.UI.GraphicalDecorators;
    using SharPipes.UI.Helpers;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private Vector offset;
        private bool isDragging = false;
        private DataObject? dragData = null;
        private DependencyObject? dragSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.PipeElements = new List<IElement>
            {
            };

            foreach (string factoryTypeName in PipeElementFactory.GetFactoryTypes())
            {
                var element = PipeElementFactory.Make(factoryTypeName, "template");
                if (element != null)
                {
                    this.PipeElements.Add(element);
                }
            }

            this.startPoint = default;

            this.Pipeline = new GraphicalPipeline(new PipeLine());

            this.DataContext = this;
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets a list of <see cref="IElement"/> to use as templates.
        /// </summary>
        /// <value>
        /// A list of <see cref="IElement"/> to use as templates.
        /// </value>
        public List<IElement> PipeElements
        {
            get;
        }

        /// <summary>
        /// Gets or sets the graphical pipeline used.
        /// </summary>
        /// <value>
        /// The graphical pipeline used.
        /// </value>
        public GraphicalPipeline Pipeline { get; set; }

        /// <inheritdoc/>
        protected override async void OnClosing(CancelEventArgs e)
        {
            await this.Pipeline.Stop().ConfigureAwait(true);
            base.OnClosing(e);
        }

        private void ToggleToolBar(object sender, RoutedEventArgs e)
        {
            if (this.ToolBox.Visibility == Visibility.Visible)
            {
                this.ToolBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ToolBox.Visibility = Visibility.Visible;
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await this.Pipeline.Start().ConfigureAwait(true);
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("fromToolBar"))
            {
                if (e.Data.GetData("fromToolBar") is IElement template)
                {
                    this.Pipeline.CreateNodeFromTemplate(template, e.GetPosition(sender as IInputElement) - this.offset);
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
                    Point newPos = e.GetPosition(sender as IInputElement) - this.offset;

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

        private void HandleDragStart<TContainer, TItem, TValue>(MouseButtonEventArgs e, string format)
            where TContainer : ItemsControl
            where TItem : DependencyObject, IInputElement
        {
            TItem? listViewItem =
                ((DependencyObject)e.OriginalSource).FindAnchestor<TItem>();
            TContainer? listView = ((DependencyObject)e.OriginalSource).FindAnchestor<TContainer>();

            if (listViewItem == null || listView == null)
            {
                return;
            }

            this.startPoint = e.GetPosition(null);
            this.isDragging = false;

            var item = listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

            if (item is TValue element)
            {
                this.offset = e.GetPosition(listViewItem) - default(Point);
                this.dragData = new DataObject(format, element);
                this.dragSource = listViewItem;
            }
        }

        private void HandlePreviewMouseMove(MouseEventArgs e)
        {
            if (this.isDragging || this.dragSource == null || this.dragData == null || this.startPoint == null)
            {
                return;
            }

            Point mousePos = e.GetPosition(null);

            Vector diff = this.startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                this.isDragging = true;

                DragDrop.DoDragDrop(this.dragSource, this.dragData, DragDropEffects.Move);
            }
        }

        private void PipeLineRenderer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            this.dragSource = null;
            this.dragData = null;
            this.HandleDragStart<PipeLineRenderer, PipeLineItem, GraphicalElement>(e, "moveElement");
            this.HandleDragStart<PipeLineRenderer, PipeLineItem, GraphicalSrcPad>(e, "drawEdge");
        }

        private void PipeLineRenderer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.HandlePreviewMouseMove(e);
        }

        private void ToolBoxList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            this.dragSource = null;
            this.dragData = null;
            this.HandleDragStart<ListView, ListViewItem, IElement>(e, "fromToolBar");
        }

        private void ToolBoxList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.HandlePreviewMouseMove(e);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog
            {
                FileName = "pipeline",
                Filter = Properties.strings.FileFilter,
                InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = "json",
            })
            {
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using Stream fileStream = sfd.OpenFile();
                    using var streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(JsonConvert.SerializeObject(this.Pipeline.GetDefinition(), Formatting.Indented));
                }
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
            {
                FileName = "pipeline",
                Filter = Properties.strings.FileFilter,
                InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = "json",
            })
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using Stream fileStream = ofd.OpenFile();
                    using var streamReader = new StreamReader(fileStream);
                    GraphicalPipeLineDefinition? pdef = JsonConvert.DeserializeObject<GraphicalPipeLineDefinition>(streamReader.ReadToEnd());
                    if (pdef != null)
                    {
                        this.Pipeline.FromDefinition(pdef);
                    }
                }
            }
        }
    }
}
