using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalEdge : IGraphical, INotifyPropertyChanged
    {

        public GraphicalEdge(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            Src = src;
            Sink = sink;

            this.Src.PropertyChanged += Dependant_PropertyChanged;
            this.Sink.PropertyChanged += Dependant_PropertyChanged;
        }

        private void Dependant_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "X")
            {
                OnPropertyChanged("X");
                OnPropertyChanged("Width");
                OnPropertyChanged("Start");
                OnPropertyChanged("End");
                OnPropertyChanged("Control1");
                OnPropertyChanged("Control2");
            }
            else if (e.PropertyName == "Y")
            {
                OnPropertyChanged("Y");
                OnPropertyChanged("Height");
                OnPropertyChanged("Start");
                OnPropertyChanged("End");
                OnPropertyChanged("Control1");
                OnPropertyChanged("Control2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        public bool Equals(IGraphical other)
        {
            return this.Id == other.Id;
        }

        public double X => Math.Min(Src.X, Sink.X);

        public double Y => Math.Min(Src.Y, Sink.Y);

        public double Width => Math.Max(1, Math.Abs(Sink.X - Src.X)) + 10;
        public double Height => Math.Max(1, Math.Abs(Sink.Y - Src.Y)) + 10;

        public Point Start => new Point(Src.X - X + 5, Src.Y - Y + 5);
        public Point End => new Point(Sink.X - X + 5, Sink.Y - Y + 5);

        public Point Control1 => Start + new Vector(Math.Max(this.Width / 2, 25), 0);
        public Point Control2 => End + new Vector(-Math.Max(this.Width / 2, 25), 0);

        public GraphicalSrcPad Src { get; }
        public GraphicalSinkPad Sink { get; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
