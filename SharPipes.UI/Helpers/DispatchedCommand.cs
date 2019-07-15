// -----------------------------------------------------------------------
// <copyright file="DispatchedCommand.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System;
    using System.Windows.Input;
    using System.Windows.Threading;

    internal class DispatchedCommand : ICommand
    {
        private readonly ICommand command;
        private readonly Dispatcher dispatcher;

        public DispatchedCommand(ICommand command, Dispatcher dispatcher)
        {
            this.command = command;
            this.dispatcher = dispatcher;

            command.CanExecuteChanged += this.Command_CanExecuteChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.command.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.command.Execute(parameter);
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.dispatcher.Invoke(() =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(sender, e);
                    }
                });
            }
        }
    }
}