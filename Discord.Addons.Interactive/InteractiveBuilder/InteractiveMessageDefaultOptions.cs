using System;
using Discord.WebSocket;

namespace Discord.Addons.Interactive.InteractiveBuilder
{
    internal static class InteractiveMessageDefaultOptions
    {
        internal static string[] Messages { get; } = null;
        internal static TimeSpan TimeSpan { get; } = TimeSpan.FromSeconds(15);
        internal static InteractiveTextResponseType ResponseType { get; } = InteractiveTextResponseType.Any;
        internal static LoopEnabled Repeat { get; } = LoopEnabled.Null;
        internal static Criteria<SocketMessage> MessageCriteria { get; } = new Criteria<SocketMessage>();
        internal static IMessageChannel Channel { get; } = null;
        internal static string[] Options { get; } = null;
        internal static string[] CancelationMessages { get; } = null;
        internal static string[] TimeoutMessage { get; } = null;
        internal static string[] CancelationWords { get; } = null;
        internal static string[] WrongResponseMessages { get; } = null;
        internal static IUser Users { get; } = null;
        internal static bool CaseSensitive { get; } = false;
    }
}