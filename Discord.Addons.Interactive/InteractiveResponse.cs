using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public class InteractiveResponse
    {
        public SocketMessage Message { get; set; }

        public CriteriaResult CriteriaResult { get; set; }

        public InteractiveResponse(CriteriaResult criteriaResult, SocketMessage response)
        {
            CriteriaResult = criteriaResult;
            Message = response;
        }
        public InteractiveResponse()
        {

        }
    }
}