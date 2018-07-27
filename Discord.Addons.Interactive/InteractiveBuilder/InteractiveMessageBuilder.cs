using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using JetBrains.Annotations;

// ReSharper disable CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessageBuilder
    {
        private InteractiveTextResponseType _responseType = InteractiveMessageDefaultOptions.ResponseType;
        private string[] _options = InteractiveMessageDefaultOptions.Options;
        private List<IMessageChannel> _channels;
        private List<IUser> _users;

        public InteractiveTextResponseType ResponseType //TODO: Regex response type
        {
            get => _responseType;
            internal set
            {
                if (_responseType != InteractiveTextResponseType.Any)
                    throw new InvalidOperationException(
                        "Cannot set the response type because has been already setted.");
                _responseType = value;
            }
        }

        public string[] Messages { get; internal set; } = InteractiveMessageDefaultOptions.Messages;
        public TimeSpan Timeout { get; internal set; } = InteractiveMessageDefaultOptions.TimeSpan;
        public LoopEnabled Repeat { get; internal set; } = InteractiveMessageDefaultOptions.Repeat;
        public bool CaseSensitive { get; internal set; } = InteractiveMessageDefaultOptions.CaseSensitive;

        public List<IMessageChannel> Channels
        {
            get => _channels;
            internal set => _channels =
                value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null",
                    nameof(Channels));
        }

        public Criteria<SocketMessage> MessageCriteria { get; internal set; } =
            InteractiveMessageDefaultOptions.MessageCriteria;

        public string[] Options
        {
            get => _options;
            internal set
            {
                _options = value ?? throw new ArgumentNullException(
                               "Cannot set an interactive message builder's field to null", nameof(value));
                ResponseType = InteractiveTextResponseType.Options;
            }
        }

        public List<IUser> Users
        {
            get => _users;
            internal set => _users =
                value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null",
                    nameof(value));
        }

        public string[] CancelationWords { get; internal set; } = InteractiveMessageDefaultOptions.CancelationWords;
        public string[] CancelationMessages { get; internal set; } = InteractiveMessageDefaultOptions.CancelationMessages;
        public string[] TimeoutMessages { get; internal set; } = InteractiveMessageDefaultOptions.TimeoutMessage;
        public string[] WrongResponseMessages { get; internal set; } = InteractiveMessageDefaultOptions.WrongResponseMessages;
        
        //TODO: Some kind of message pool object, to pick one randomly to send it
        
        /// <summary>
        /// This message(s) will be sent if the <see cref="CancelationWords"/> is sent by any listened source.
        /// If it's not setted, won't be sent. 
        /// </summary>
        /// <param name="cancelationMessage"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder SetCancelationMessage(params string[] cancelationMessage)
        {
            CancelationMessages = cancelationMessage;
            return this;
        }

        /// <summary>
        /// This message(s) will be sent if one of the listened users reply with a wrong answer
        /// <example>
        ///<code>
        ///interactiveBuilder.SetWrongResponseMessage("You have to reply with Yes or No", "Please, reply with Yes or No");
        /// </code>
        /// </example>
        /// If it's not setted, won't be sent. 
        /// </summary>
        /// <param name="wrongResponse"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder SetWrongResponseMessage(params string[] wrongResponse)
        {
            WrongResponseMessages = wrongResponse;
            return this;
        }
        
        /// <summary>
        /// This message(s) will be sent if the <see cref="Timeout"/> is triggered.
        /// If it's not setted, won't be sent.
        /// </summary>
        /// <param name="timeoutMessage"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder SetTimeoutMessage(params string[] timeoutMessage)
        {
            TimeoutMessages = timeoutMessage;
            return this;
        }

        /// <summary>
        ///     Let the user stop the <see cref="InteractiveMessage" /> with a certain word
        ///     Can add as many as you want. By default, it's not case sensitive. <seealso cref="EnableCaseSensitive"/>
        /// </summary>
        /// <param name="word">
        ///     The word to stop the Interactive Message.
        /// </param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithCancellationWord([NotNull] params string[] word)
        {
            CancelationWords = word;
            return this;
        }

        /// <summary>
        ///     Interactive message Builder
        /// </summary>
        /// <param name="message">Message or mesasges to send before listening.</param>
        public InteractiveMessageBuilder([NotNull] params string[] message)
        {
            Messages = message;
        }

        public InteractiveMessageBuilder()
        {
        }

        /// <summary>
        ///     Adds a pre-defined <see cref="Criteria{T}" />
        /// </summary>
        /// <param name="criteriaType">The <see cref="Criteria{T}" /> to add</param>
        /// <exception cref="InvalidOperationException"> </exception>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder AddCriteria(CriteriaType criteriaType)
        {
            switch (criteriaType)
            {
                case CriteriaType.SourceUser:
                    if (Users != null || MessageCriteria.ContainsCriteriaType(typeof(EnsureFromUserCriterion)))
                        throw new InvalidOperationException(
                            "Cannot add an SourceUser criteria because an user has been already selected");
                    MessageCriteria.AddCriterion(new EnsureSourceUserCriterion());
                    break;
                case CriteriaType.SourceChannel:
                    if (Channels != null || MessageCriteria.ContainsCriteriaType(typeof(EnsureFromChannelCriterion)))
                        throw new InvalidOperationException(
                            "Cannot add an SourceChannel criteria because a channel has been already selected");
                    MessageCriteria.AddCriterion(new EnsureSourceChannelCriterion());
                    break;
                case CriteriaType.Empty:
                    MessageCriteria.AddCriterion(new EmptyCriterion<SocketMessage>());
                    break;
            }

            return this;
        }

        /// <summary>
        ///     Adds a <see cref="Criteria{T}" />
        /// </summary>
        /// <param name="criterion">The <see cref="Criteria{T}" /> to add</param>
        /// <exception cref="InvalidOperationException"> </exception>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder AddCriteria(ICriterion<SocketMessage> criterion)
        {
            MessageCriteria.AddCriterion(criterion);
            return this;
        }

        /// <summary>
        ///     Set a filter to the response.
        /// </summary>
        /// <param name="responseType">The response Type.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithResponseType(InteractiveTextResponseType responseType)
        {
            ResponseType = responseType;
            return this;
        }

        /// <summary>
        ///     If loop is activated, won't stop until a valid response is given.
        /// <seealso cref="WithResponseType"/>
        /// </summary>
        /// <param name="withLoop"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder EnableLoop(LoopEnabled withLoop = LoopEnabled.True)
        {
            Repeat = withLoop;
            return this;
        }

        /// <summary>
        ///     Decide if <see cref="Options"/> and <see cref="CancelationWords"/> checks are case sensitive
        /// <seealso cref="WithResponseType"/>
        /// </summary>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder EnableCaseSensitive(bool caseSensitive = true)
        {
            CaseSensitive = caseSensitive;
            return this;
        }
      
        /// <summary>
        ///     Specify a few options to be valid as response.
        ///     By default, it's not case sensitive. <seealso cref="EnableCaseSensitive"/>
        /// </summary>
        /// <param name="options">The options</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder SetOptions([ItemNotNull] params string[] options)
        {
            Options = options;
            return this;
        }

        /// <summary>
        /// The bot will listen the provided user. Multiple users can be listened.
        /// </summary>
        /// <param name="user">User to listen</param>
        /// <returns></returns>
        [Obsolete]
        public InteractiveMessageBuilder ListenUser([NotNull] IUser user)
        {
            if (Users == null) Users = new List<IUser>();
            Users.Add(user);
            return this;
        }
        
        /// <summary>
        /// The bot will listen the provided users.
        /// </summary>
        /// <param name="users">Users to listen</param>
        /// <returns></returns>
        public InteractiveMessageBuilder ListenUsers([NotNull] params IUser[] users)
        {
            Users = Users == null ? new List<IUser>(users) : Users.Concat(users).ToList();
            return this;
        }

        /// <summary>
        /// The bot will listen the provided channel. Multiple channels can be added.
        /// </summary>
        /// <param name="channel">Channel to listen</param>
        /// <returns></returns>
        [Obsolete]
        public InteractiveMessageBuilder ListenChannel([NotNull] IMessageChannel channel)
        {
            if (Channels == null) Channels = new List<IMessageChannel>();
            Channels.Add(channel);
            return this;
        }

        /// <summary>
        /// The bot will listen the provided channels.
        /// </summary>
        /// <param name="channels">Channels to listen</param>
        /// <returns></returns>
        public InteractiveMessageBuilder ListenChannels([NotNull] params IMessageChannel[] channels)
        {
            Channels = Channels == null ? new List<IMessageChannel>(channels) : Channels.Concat(channels).ToList();
            return this;
        }

        /// <summary>
        ///     Time the bot will wait for a reply (timeout).
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithTimeSpan(TimeSpan timeSpan)
        {
            Timeout = timeSpan;
            return this;
        }

        /// <summary>
        ///     Message to send when the Interactive Message starts.
        ///     Messages are sent to the first channel in the provided list (<see cref="Channels"/>,
        /// or the source channel <seealso cref="ListenChannel(Discord.IMessageChannel)"/>
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        [Obsolete]
        public InteractiveMessageBuilder SetMessage([NotNull] params string[] message)
        {
            Messages = message;
            return this;
        }

        /// <summary>
        ///     Build an <see cref="InteractiveMessage" />
        /// </summary>
        /// <returns>
        ///     <see cref="InteractiveMessage" />
        /// </returns>
        public InteractiveMessage Build()
        {
            if (Users != null)
            {
                if (!MessageCriteria.ContainsCriteriaType(typeof(EnsureSourceUserCriterion)))
                    MessageCriteria.AddCriterion(new EnsureFromUsersCriterion(Users));
            }

            if (Channels != null)
            {
                if (!MessageCriteria.ContainsCriteriaType(typeof(EnsureSourceChannelCriterion)))
                    MessageCriteria.AddCriterion(new EnsureFromChannelsCriterion(Channels));
            }

            return new InteractiveMessage(Messages, Timeout, ResponseType, Repeat, MessageCriteria, CancelationMessages,
                TimeoutMessages, CancelationWords, WrongResponseMessages, Channels?.FirstOrDefault(), Options);
        }
    }
}