using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace Discord.Addons.Interactive.Extensions
{
    public static class SocketMessageExt
    {
        public static bool ContainsRole(this SocketMessage message, int numberOfRoles = 0, SocketRole role = null)
            => role != null 
                ? message.MentionedRoles.Contains(role) 
                :(numberOfRoles > 0 ? message.MentionedRoles.Count == numberOfRoles : message.MentionedRoles.Count >= 1);

        public static bool ContainsChannel(this SocketMessage message, int numberOfChannels = 0, SocketChannel channel = null)
            => channel != null 
                ? message.MentionedChannels.Contains(channel)
                : (numberOfChannels > 0 ? message.MentionedChannels.Count == numberOfChannels : message.MentionedChannels.Count >= 1);

        public static bool ContainsUser(this SocketMessage message, int numberOfUsers = 0, IUser user = null)
            => user != null 
                ? message.MentionedUsers.Contains(user)
                : (numberOfUsers > 0 ? message.MentionedUsers.Count == numberOfUsers : message.MentionedUsers.Count >= 1);

        public static bool ContainsWords(this SocketMessage message,  int numberOfCoincidences = 0, params string[] words)
        {
            var coincidences = words.Where(word => message.Content.Contains(word));
            return numberOfCoincidences > 0 ? coincidences.Count() == numberOfCoincidences : coincidences.Any();
        }
    }
    
}