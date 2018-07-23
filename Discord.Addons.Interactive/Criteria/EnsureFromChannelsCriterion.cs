using System.Collections.Generic;
using System.Linq;

namespace Discord.Addons.Interactive
{
    using System.Threading.Tasks;

    using Commands;
    using WebSocket;

    public class EnsureFromChannelsCriterion : ICriterion<SocketMessage>
    {
        private readonly List<IMessageChannel> _channels;

        public EnsureFromChannelsCriterion(List<IMessageChannel> channels) => _channels = channels;

        /// <summary>
        /// Ensures the user is the author
        /// </summary>
        /// <param name="sourceContext"></param>
        /// <param name="parameter"></param>
        /// <returns>
        /// True if user is author
        /// </returns>
        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketMessage parameter)
        {
            bool ok = _channels.Any(u => u.Id == parameter.Channel.Id);
            return Task.FromResult(ok);
        }
    }
}