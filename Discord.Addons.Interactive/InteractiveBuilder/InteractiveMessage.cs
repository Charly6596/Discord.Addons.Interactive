using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Addons.Interactive.Extensions;
using Discord.WebSocket;

// ReSharper disable once CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessage : InteractiveMessageOptions
    {
        //TODO: Refactor messages to another class
        public InteractiveMessage(string[] message, TimeSpan timeSpan, InteractiveTextResponseType responseType,
            LoopEnabled repeat, Criteria<SocketMessage> messageCriteria, string[] cancelationMessage,
            string[] timeoutMessage, string[] cancelationWord, string[] wrongResponseMessages, IMessageChannel channel = null, String[] options = null, bool caseSensitive = false)
        {
            Messages = message;
            TimeSpan = timeSpan;
            ResponseType = responseType;
            Repeat = repeat;
            MessageCriteria = messageCriteria;
            CancelationMessages = cancelationMessage;
            TimeoutMessages = timeoutMessage;
            CancelationWords = cancelationWord;
            Channel = channel;
            Options = options;
            CaseSensitive = caseSensitive;
            WrongResponseMessages = wrongResponseMessages;
        }

        internal async Task SendCancellationMessages()
        {
            await Channel.SendMessagesAsync(CancelationMessages);
        }

        internal async Task SendFirstMessages()
        {
            await Channel.SendMessagesAsync(Messages);
        }

        internal async Task SendTimeoutMessages()
        {
            await Channel.SendMessagesAsync(TimeoutMessages);
        }

        internal async Task SendWrongResponseMessages()
        {
            var random = new Random();
            var index = random.Next(0, WrongResponseMessages.Length);
            await Channel.SendMessagesAsync(WrongResponseMessages.ElementAt(index));
        }
    }
}