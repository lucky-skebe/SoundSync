// -----------------------------------------------------------------------
// <copyright file="CommandInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.InteractionInfos
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// An interaction that works similar to a WPF <see cref="ICommand"/>.
    /// This can be used for buttons.
    /// </summary>
    public class CommandInteraction : IInteraction, ICommand
    {
        private readonly Action action;
        private bool canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInteraction"/> class.
        /// </summary>
        /// <param name="name">The name of the command. Used for labeing Buttons...</param>
        /// <param name="action">the ction that should happen when the button is pressed.</param>
        /// <param name="canExecute">True is the command can be executed immideatly otherwise False.</param>
        public CommandInteraction(string name, Action action, bool canExecute = true)
        {
            this.Name = name;
            this.action = action;
            this.canExecute = canExecute;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>
        /// Sets whenever the command can currently be executed.
        /// </summary>
        /// <param name="canExecute">If the command can be executed.</param>
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

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            this.action();
        }
    }
}
