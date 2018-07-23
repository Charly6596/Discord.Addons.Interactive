using System;
using System.Collections.Generic;
using Discord.WebSocket;

// ReSharper disable once CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessage : InteractiveMessageOptions
    {
        public InteractiveMessage(string message, TimeSpan timeSpan, InteractiveTextResponseType responseType, LoopEnabled repeat, Criteria<SocketMessage> messageCriteria, string cancelationMessage, string timeoutMessage, string cancelationWord, IMessageChannel channel = null, String[] options = null)
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