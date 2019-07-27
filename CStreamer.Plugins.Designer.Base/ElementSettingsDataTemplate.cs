using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Designer.Base
{
    public abstract class ElementSettingsDataTemplate : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public abstract IControl Build(object element);
        
        public virtual bool Match(object data)
        {
            return data is IElement;
        }
    }

    public abstract class ElementSettingsDataTemplate<TElement> : ElementSettingsDataTemplate where TElement: IElement
    {
        public abstract IControl Build(TElement element);

        public override IControl Build(object param)
        {
            return this.Build((TElement)param);
        }

        public override bool Match(object data)
        {
            return data is TElement;
        }
    }
}
