using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;

namespace SharPipes.UI.Helpers
{
    public abstract class DispatchedObservableCollectionConverter<T> : IValueConverter
    {
        protected abstract DispatchedObservableCollection<T> GetWrapper(ObservableCollection<T> collection, Dispatcher dispatcher);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<T> collection)
            {
                return GetWrapper(collection, Dispatcher.CurrentDispatcher);
            }

            throw new NotImplementedException();
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
