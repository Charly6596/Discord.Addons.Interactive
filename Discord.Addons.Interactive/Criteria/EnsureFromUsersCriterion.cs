using System.Collections.Generic;
using System.Linq;

namespace Discord.Addons.Interactive
{
    using System.Threading.Tasks;
    using Commands;
    using WebSocket;

    public class EnsureFromUsersCriterion : ICriterion<SocketMessage>
    {
        private readonly IEnumerable<IUser> _users;

        public EnsureFromUsersCriterion(IEnumerable<IUser> users) => _users = users;

        /// <summary>
        /// Ensures the user is being listened
        /// </summary>
        /// <param name="sourceContext"></param>
        /// <param name="parameter"></param>
        /// <returns>
        /// True if user is being listened
        /// </returns>
        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketMessage parameter)
        {
            bool ok = _users.Any(u => u.Id == parameter.Author.Id);
            return Task.FromResult(ok);
        }
    }
}