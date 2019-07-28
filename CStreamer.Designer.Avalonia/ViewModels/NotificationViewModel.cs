// -----------------------------------------------------------------------
// <copyright file="NotificationViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    using Notification = CStreamer.Designer.Avalonia.Notification;

    public class NotificationViewModel : ViewModelBase
    {
        private readonly Notification notification;
        private double opacity = 0;

        public NotificationViewModel(Notification notification, Action<NotificationViewModel> cleanup)
        {
            this.notification = notification;
            this.KeepVisible = ReactiveCommand.Create(() => { });

            this.WhenActivated((disposables) =>
            {
                var created = Observable.Return(Unit.Default);
                var keepVisible = this.KeepVisible.AsObservable();

                var close = created.Merge(keepVisible).Throttle(TimeSpan.FromSeconds(5));

                var remove = close.Delay(TimeSpan.FromSeconds(1));

                close.Subscribe((unit) => { this.Opacity = 0; }).DisposeWith(disposables);
                remove.ObserveOn(RxApp.MainThreadScheduler).Subscribe((unit) => { cleanup(this); }).DisposeWith(disposables);
            });

            this.Opacity = 1;
        }

        public double Opacity { get => this.opacity; set => this.RaiseAndSetIfChanged(ref this.opacity, value); }

        public ReactiveCommand<Unit, Unit> KeepVisible { get; }

        public string Text => this.notification.Text;
    }
}
