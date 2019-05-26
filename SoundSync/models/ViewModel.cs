using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SoundSync.models
{
    public abstract class ViewModel: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var PropertyChanged = this.PropertyChanged;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            });
        }
    }
}
