using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace CStreamer.Designer.Avalonia.Views
{
    public class ToolBarView : ReactiveUserControl<ToolBarViewModel>
    {
        public class EventObserver
        {
            private readonly ToolBarView parent;


            public EventObserver(ToolBarView parent)
            {
                this.parent = parent;
            }

            public IObservable<EventPattern<PointerPressedEventArgs>> PointerPressed => Observable.FromEventPattern<PointerPressedEventArgs>(handler => this.parent.PointerPressed += handler, handler => this.parent.PointerPressed -= handler);
            public IObservable<EventPattern<PointerEventArgs>> PointerMoved => Observable.FromEventPattern<PointerEventArgs>(handler => this.parent.PointerMoved += handler, handler => this.parent.PointerMoved -= handler);
            public IObservable<EventPattern<PointerReleasedEventArgs>> PointerReleased => Observable.FromEventPattern<PointerReleasedEventArgs>(handler => this.parent.PointerReleased += handler, handler => this.parent.PointerReleased -= handler);

        }

        public ToolBarView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public EventObserver Events()
        {
            return new EventObserver(this);
        }
    }
}
