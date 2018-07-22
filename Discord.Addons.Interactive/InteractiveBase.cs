namespace Discord.Addons.Interactive
{
    using System;
    using System.Threading.Tasks;
    using InteractiveBuilder;
    using Commands;
    using WebSocket;

    /// <summary>
    /// The interactive base.
    /// </summary>
    public class InteractiveBase : InteractiveBase<SocketCommandContext>
    {
    }

    /// <summary>
    /// The interactive base.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class InteractiveBase<T> : ModuleBase<T>
        where T : SocketCommandContext
    {
        /// <summary>
        /// Gets or sets the interactive service.
        /// </summary>
        public InteractiveService Interactive { get; set; }

        public Task<SocketMessage> NextMessageAsync(ICriterion<SocketMessage> criterion, TimeSpan? timeout = null)
            => Interactive.NextMessageAsync(Context, criterion, timeout);
        public Task<SocketMessage> NextMessageAsync(bool fromSourceUser = true, bool inSourceChannel = true, TimeSpan? timeout = null) 
            => Interactive.NextMessageAsync(Context, fromSourceUser, inSourceChannel, timeout);

        public Task<IUserMessage> ReplyAndDeleteAsync(string content, bool isTTS = false, Embed embed = null, TimeSpan? timeout = null, RequestOptions options = null)
            => Interactive.ReplyAndDeleteAsync(Context, content, isTTS, embed, timeout, options);

        public Task<IUserMessage> InlineReactionReplyAsync(ReactionCallbackData data, bool fromSourceUser = true)
            => Interactive.SendMessageWithReactionCallbacksAsync(Context, data, fromSourceUser);

        public Task<IUserMessage> PagedReplyAsync(PaginatedMessage pager, ReactionList Reactions, bool fromSourceUser = true)
        {
            var criterion = new Criteria<SocketReaction>();
            if (fromSourceUser)
                criterion.AddCriterion(new EnsureReactionFromSourceUserCriterion());
            return PagedReplyAsync(pager, criterion, Reactions);
        }
        public Task<IUserMessage> PagedReplyAsync(PaginatedMessage pager, ICriterion<SocketReaction> criterion, ReactionList Reactions)
            => Interactive.SendPaginatedMessageAsync(Context, pager, Reactions, criterion);

        public RuntimeResult Ok(string reason = null) => new OkResult(reason);

        public async Task<SocketMessage> StartInteractiveMessage(InteractiveMessage interactiveMessage)
        {

            CriteriaResult result;
            InteractiveResponse response;
            interactiveMessage.Channel = interactiveMessage.Channel ?? Context.Channel;
            if (interactiveMessage.Repeat)
            {
                do
                {
                    if (!String.IsNullOrEmpty(interactiveMessage.Message))
                        await interactiveMessage.Channel.SendMessageAsync(interactiveMessage.Message);

                    response = await Interactive.NextMessageAsync(Context, interactiveMessage);
                    result = response.CriteriaResult;
                } while (result == CriteriaResult.WrongResponse);
            }
            else
            {
                if (!String.IsNullOrEmpty(interactiveMessage.Message))
                    await interactiveMessage.Channel.SendMessageAsync(interactiveMessage.Message);

                response = await Interactive.NextMessageAsync(Context, interactiveMessage);
            }

            string message;
            if(response.CriteriaResult != CriteriaResult.Success)
            {
                switch (response.CriteriaResult)
                {
                    case CriteriaResult.Timeout:
                        message = String.IsNullOrEmpty(interactiveMessage.TimeoutMessage) ? "Timeout." : interactiveMessage.TimeoutMessage;
                        break;
                    case CriteriaResult.Canceled:
                        message = String.IsNullOrEmpty(interactiveMessage.CancelationMessage) ? "Alright then, nevermind." : interactiveMessage.CancelationMessage;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                await Context.Channel.SendMessageAsync(message);
            }
            return response.Message;
        }
    }
}
