using System.Collections.Generic;
using System.Linq;

namespace Discord.Addons.Interactive
{
    using System.Threading.Tasks;

    using Commands;
    using WebSocket;

    public class EnsureFromUsersCriterion : ICriterion<SocketMessage>
    {
        private readonly List<IUser> _users;

        public EnsureFromUsersCriterion(List<IUser> users) => _users = users;

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
            bool ok = _users.Any(u => u.Id == parameter.Author.Id);
            return Task.FromResult(ok);
        }
    }
}
