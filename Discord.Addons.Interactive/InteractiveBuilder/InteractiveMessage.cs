using System;
using System.Collections.Generic;
using Discord.WebSocket;

// ReSharper disable once CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessage
    {
        public string Message { get; internal set; }
        public TimeSpan TimeSpan { get; internal set; }
        public InteractiveTextResponseType ResponseType { get; internal set; }
        public bool Repeat { get; internal set; }
        public Criteria<SocketMessage> MessageCriteria { get; internal set; }
        public IMessageChannel Channel { get; set; }
        public List<string> Options { get; set; }

        public InteractiveMessage(string message, TimeSpan timeSpan, InteractiveTextResponseType responseType, bool repeat, Criteria<SocketMessage> messageCriteria, IMessageChannel channel = null, List<string> options = null)
        {
            Message = message;
            TimeSpan = timeSpan;
            ResponseType = responseType;
            Repeat = repeat;
            MessageCriteria = messageCriteria;
            Channel = channel;
            Options = options;
        }
    }
}