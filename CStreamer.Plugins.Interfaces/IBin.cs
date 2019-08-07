namespace CStreamer.Plugins.Interfaces
{
    using CStreamer.Plugins.Interfaces.Messages;

    public interface IBin
    {
        void ReceiveMessage(Message message);
    }
}
