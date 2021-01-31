using System;
using System.Collections.Generic;

namespace DSG.Presentation.Messaging
{
    public class Messenger : IMessenger
    {
        public Dictionary<string, Action<object>> ActionsByMessageName { get; set; }

        public event EventHandler MessagingEvent;

        public Messenger()
        {
            ActionsByMessageName = new Dictionary<string, Action<object>>();
        }

        public void NotifyEventTriggered(MessageDto messageDto)
        {
            MessagingEvent(messageDto, new EventArgs());
        }

        public void Register(string name, Action<object> action)
        {
            ActionsByMessageName.Add(name, action);
            this.MessagingEvent += HandleEvent;
        }

        internal void HandleEvent(object sender, EventArgs e)
        {
            MessageDto messageDto = (MessageDto)sender;

            if (ActionsByMessageName.ContainsKey(messageDto.Name))
            {
                ActionsByMessageName[messageDto.Name](messageDto.Data);
            }
        }
    }
}
