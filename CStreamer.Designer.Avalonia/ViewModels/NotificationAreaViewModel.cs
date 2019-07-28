// -----------------------------------------------------------------------
// <copyright file="NotificationAreaViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System.Collections.ObjectModel;

    public class NotificationAreaViewModel : ViewModelBase
    {
        private readonly ObservableCollection<NotificationViewModel> notifications;

        public NotificationAreaViewModel()
        {
            this.notifications = new ObservableCollection<NotificationViewModel>();
            this.Notifications = new ReadOnlyObservableCollection<NotificationViewModel>(this.notifications);
        }

        public ReadOnlyObservableCollection<NotificationViewModel> Notifications { get; }

        public void AddNotification(Notification notification)
        {
            this.notifications.Add(new NotificationViewModel(notification, (notification) => { this.notifications.Remove(notification); }));
        }
    }
}
