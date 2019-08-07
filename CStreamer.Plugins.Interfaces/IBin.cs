namespace CStreamer.Plugins.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBin
    {
        void ReceiveMessage(Message message);
    }
}
