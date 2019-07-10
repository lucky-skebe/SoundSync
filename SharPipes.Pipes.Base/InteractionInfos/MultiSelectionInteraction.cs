using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public abstract class MultiSelectionInteraction<TSelect, TValue> : IInteraction where TSelect: ISelectable<TValue>
    {
        public MultiSelectionInteraction()
        {
        }

        public ObservableCollection<TSelect> Options { get; } = new ObservableCollection<TSelect>();
        
    }
}
