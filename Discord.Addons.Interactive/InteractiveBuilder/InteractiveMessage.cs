using System;
using System.Collections.Generic;
using Discord.WebSocket;

// ReSharper disable once CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessage
    {
        public string Message { get; private set; }
        public TimeSpan TimeSpan { get; private set; }
        public InteractiveTextResponseType ResponseType { get; private set; }
        public bool Repeat { get; private set; }
        public Criteria<SocketMessage> MessageCriteria { get; private set; }
        public IMessageChannel Channel { get; set; }
        public String[] Options { get; private set; }
        public string CancelationMessage { get; private set; }
        public string TimeoutMessage { get; private set; }
        public string CancelationWord { get; private set; }

        public InteractiveMessage(string message, TimeSpan timeSpan, InteractiveTextResponseType responseType, bool repeat, Criteria<SocketMessage> messageCriteria, string cancelationMessage, string timeoutMessage, string cancelationWord, IMessageChannel channel = null, String[] options = null)
        {
            Message = message;
            TimeSpan = timeSpan;
            ResponseType = responseType;
            Repeat = repeat;
            MessageCriteria = messageCriteria;
            CancelationMessage = cancelationMessage;
            TimeoutMessage = timeoutMessage;
            CancelationWord = cancelationWord;
            Channel = channel;
            Options = options;
        }
    }
}