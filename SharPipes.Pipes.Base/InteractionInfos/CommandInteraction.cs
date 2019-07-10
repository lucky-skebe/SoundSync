using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class CommandInteraction : IInteraction, ICommand
    {
        private readonly Action action;
        private bool canExecute;

        public CommandInteraction(string Name, Action action, bool canExecute = true)
        {
            this.Name = Name;
            this.action = action;
            this.canExecute = canExecute;
        }

        public void SetCanExecute(bool canExecute)
        {
            if (this.canExecute != canExecute)
            {
                this.canExecute = canExecute;
                
                if (this.CanExecuteChanged != null)
                {
                    this.CanExecuteChanged.Invoke(this, new EventArgs());
                }
            }
        }

        public string Name { get; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
