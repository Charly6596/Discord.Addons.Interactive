using System;
using Discord.WebSocket;

namespace Discord.Addons.Interactive.InteractiveBuilder
{
    internal static class InteractiveMessageDefaultOptions
    {
        internal static string Message { get; } = String.Empty;
        internal static TimeSpan TimeSpan { get; } = TimeSpan.Zero;
        internal static InteractiveTextResponseType ResponseType { get; } = InteractiveTextResponseType.Any;
        internal static LoopEnabled Repeat { get; } = LoopEnabled.Null;
        internal static Criteria<SocketMessage> MessageCriteria { get; } = new Criteria<SocketMessage>();
        internal static IMessageChannel Channel { get; } = null;
        internal static String[] Options { get; } = null;
        internal static string CancelationMessage { get; } = null;
        internal static string TimeoutMessage { get; } = null;
        internal static string CancelationWord { get; } = null;
        internal static IUser Users { get; } = null;
    }
}