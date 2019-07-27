using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    class NotificationAreaViewModel : ViewModelBase
    {
        private ObservableCollection<NotificationViewModel> notifications;

        public NotificationAreaViewModel()
        {
            this.notifications = new ObservableCollection<NotificationViewModel>();
            this.Notifications = new ReadOnlyObservableCollection<NotificationViewModel>(this.notifications);
        }

        public void AddNotification(Notification notification)
        {
            this.notifications.Add(new NotificationViewModel(notification, (notification) => { this.notifications.Remove(notification); }));
        }

        public ReadOnlyObservableCollection<NotificationViewModel> Notifications { get; }
    }
}
