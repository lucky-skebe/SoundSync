using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Interfaces.Messages
{
    public class PadsUnlinkedMessage : Message
    {
        public PadsUnlinkedMessage(ISrcPad srcPad, ISinkPad sinkPad)
        {
            this.SrcPad = srcPad;
            this.SinkPad = sinkPad;
        }

        public ISrcPad SrcPad { get; }

        public ISinkPad SinkPad { get; }
    }
}
