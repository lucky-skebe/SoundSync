using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class ToolBarViewModel : ViewModelBase
    {
        public ToolBarViewModel(IEnumerable<string> elements)
        {
            this.Elements = new ObservableCollection<string>(elements);
        }

        public ObservableCollection<string> Elements { get; }
    }
}
