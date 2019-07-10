using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaktionslogik für ElementRenderer.xaml
    /// </summary>
    public partial class ElementRenderer : UserControl
    {
        public IPipeElement Element
        {
            get { return (IPipeElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(IPipeElement), typeof(ElementRenderer), new PropertyMetadata(null));



        public ElementRenderer()
        {
            InitializeComponent();
        }
    }
}
