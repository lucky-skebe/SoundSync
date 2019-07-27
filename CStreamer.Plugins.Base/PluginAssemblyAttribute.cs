using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Base
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public class PluginAssemblyAttribute: Attribute
    {
    }
}
