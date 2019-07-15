// -----------------------------------------------------------------------
// <copyright file="DipatchedCommandConverter.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;

    /// <summary>
    /// Converts normal <see cref="ICommand">ICommands</see> into <see cref="DispatchedCommand">DispatchedCommands</see> to dispatch their ebents on a given Dispatcher.
    /// </summary>
    public class DipatchedCommandConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICommand command)
            {
                return new DispatchedCommand(command, Dispatcher.CurrentDispatcher);
            }

            return null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
