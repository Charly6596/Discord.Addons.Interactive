using Discord.Addons.Interactive.InteractiveBuilder;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public class InteractiveResponse
    {
        private CriteriaResult _criteriaResult;
        private SocketMessage _message;

        public SocketMessage Message { get => _message; set => _message = value; }
        public CriteriaResult CriteriaResult { get => _criteriaResult; set => _criteriaResult = value; }

        public InteractiveResponse(CriteriaResult criteriaResult, SocketMessage response)
        {
            _criteriaResult = criteriaResult;
            _message = response;
        }
        public InteractiveResponse()
        {

        }
    }
}