using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace CStreamer.Designer.Avalonia.Helper
{
    public class EventObserver<TControl> where TControl: IControl
    {
        
        private readonly TControl parent;

        public EventObserver(TControl parent)
        {
            this.parent = parent;
        }

        public IObservable<EventPattern<PointerPressedEventArgs>> PointerPressed => Observable.FromEventPattern<PointerPressedEventArgs>(handler => this.parent.PointerPressed += handler, handler => this.parent.PointerPressed -= handler);

        public IObservable<EventPattern<PointerEventArgs>> PointerMoved => Observable.FromEventPattern<PointerEventArgs>(handler => this.parent.PointerMoved += handler, handler => this.parent.PointerMoved -= handler);

        public IObservable<EventPattern<PointerReleasedEventArgs>> PointerReleased => Observable.FromEventPattern<PointerReleasedEventArgs>(handler => this.parent.PointerReleased += handler, handler => this.parent.PointerReleased -= handler);

        public IObservable<EventPattern<DragEventArgs>> Drop => Observable.FromEventPattern<DragEventArgs>(handler => this.parent.AddHandler<DragEventArgs>(DragDrop.DropEvent, handler), handler => this.parent.RemoveHandler<DragEventArgs>(DragDrop.DropEvent, handler));


    }
}
