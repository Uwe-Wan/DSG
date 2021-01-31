using System;

namespace DSG.Presentation.Messaging
{
    public interface IMessenger
    {
        void NotifyEventTriggered(MessageDto messageDto);

        void Register(string name, Action<object> action);
    }
}
