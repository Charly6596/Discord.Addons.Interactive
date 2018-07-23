using System;
using Discord.Addons.Interactive.InteractiveBuilder;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public abstract class InteractiveMessageOptions
    {
        public string Message { get; internal set; }
        public TimeSpan TimeSpan { get; internal set; }
        public InteractiveTextResponseType ResponseType { get; internal set; }
        public LoopEnabled Repeat { get; internal set; }
        public Criteria<SocketMessage> MessageCriteria { get; internal set; }
        public IMessageChannel Channel { get; internal set; }
        public String[] Options { get; internal set; }
        public string CancelationMessage { get; internal set; }
        public string TimeoutMessage { get; internal set; }
        public string CancelationWord { get; internal set; }
    }

    public enum LoopEnabled : byte
    {
        Null = 0,
        True = 1,
        False = 2
    }
}