using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public class EnsureFromChannelCriterion : Criteria<SocketMessage>
    {
        ulong ChannelId { get; }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        ///The target channel
        /// </param>
        public EnsureFromChannelCriterion(IMessageChannel channel) => ChannelId = channel.Id;

        /// <summary>
        /// Returns true if the channel is the specified channel
        /// </summary>
        /// <param name="context">
        ///
        /// </param>
        /// <param name="param"></param>
        /// <returns>
        /// True if the message is sent in the specified channel
        /// </returns>
        public Task<bool> JudgeAsync(SocketCommandContext context, IMessage param)
            => Task.FromResult(ChannelId == param.Channel.Id);
    }
}