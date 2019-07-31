// -----------------------------------------------------------------------
// <copyright file="ToolBarViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class ToolBarViewModel : ViewModelBase
    {
        public ToolBarViewModel(IEnumerable<string> elements)
        {
            this.Elements = new ObservableCollection<string>(elements);
        }

        public ObservableCollection<string> Elements { get; }
    }
}
