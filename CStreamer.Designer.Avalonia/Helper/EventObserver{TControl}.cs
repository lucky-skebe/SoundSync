// -----------------------------------------------------------------------
// <copyright file="EventObserver{TControl}.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Helper
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using global::Avalonia.Controls;
    using global::Avalonia.Input;

    public class EventObserver<TControl>
        where TControl : IControl
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
