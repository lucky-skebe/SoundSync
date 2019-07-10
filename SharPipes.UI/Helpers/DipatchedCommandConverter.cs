using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SharPipes.UI.Helpers
{
    class DipatchedCommandConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ICommand command)
            {
                return new DispatchedCommand(command, Dispatcher.CurrentDispatcher);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
