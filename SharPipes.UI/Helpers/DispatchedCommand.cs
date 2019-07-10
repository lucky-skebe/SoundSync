using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace SharPipes.UI.Helpers
{
    internal class DispatchedCommand : ICommand
    {
        private ICommand command;
        private Dispatcher dispatcher;

        public DispatchedCommand(ICommand command, Dispatcher dispatcher)
        {
            this.command = command;
            this.dispatcher = dispatcher;

            command.CanExecuteChanged += Command_CanExecuteChanged;
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if(this.CanExecuteChanged != null)
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

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.command.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.command.Execute(parameter);
        }
    }
}