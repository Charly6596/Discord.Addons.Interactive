using System;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Addons.Interactive.Extensions
{
    public static class IMessageChannelExt
    {
        public static async Task SendMessagesAsync(this IMessageChannel channel, params string[] messages)
        {
            foreach (var message in messages.Where(message => !String.IsNullOrEmpty(message)))
            {
                //await channel.TriggerTypingAsync(); TODO: Option to enable this
                await channel.SendMessageAsync(message);
            }
        }
    }
}