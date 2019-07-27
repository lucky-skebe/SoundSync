using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private readonly Notification notification;
        private readonly Action<NotificationViewModel> cleanup;
        private double opacity = 0;

        public NotificationViewModel(Notification notification, Action<NotificationViewModel> cleanup)
        {
            this.notification = notification;
            this.cleanup = cleanup;
            this.KeepVisible = ReactiveCommand.Create(() => { });

            this.WhenActivated((disposables) =>
            {
                var created = Observable.Return(Unit.Default);
                var keepVisible = this.KeepVisible.AsObservable();

                var close = created.Merge(keepVisible).Throttle(TimeSpan.FromSeconds(5));

                var remove = close.Delay(TimeSpan.FromSeconds(1));

                close.Subscribe((_unit) => { this.Opacity = 0; }).DisposeWith(disposables);
                remove.ObserveOn(RxApp.MainThreadScheduler).Subscribe((_unit) => { cleanup(this); }).DisposeWith(disposables);
            });

            this.Opacity = 1;
        }

        public double Opacity { get => this.opacity; set => this.RaiseAndSetIfChanged(ref this.opacity, value); }

        public ReactiveCommand<Unit,Unit> KeepVisible { get; }

        public string Text => notification.Text;
    }
}
