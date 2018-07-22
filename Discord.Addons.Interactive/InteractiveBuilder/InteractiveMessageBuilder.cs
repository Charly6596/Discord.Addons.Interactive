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
        private string _message = String.Empty;

        private string _cancelationMessage = String.Empty;

        private string _timeoutMessage = String.Empty;

        private string _cancelationWord = String.Empty;

        private TimeSpan _timeout = System.TimeSpan.FromSeconds(15);

        private bool _repeat = false;

        private InteractiveTextResponseType _responseType = InteractiveTextResponseType.Any;

        private Criteria<SocketMessage> _messageCriteria = new Criteria<SocketMessage>();

        private String[] _options;

        private IMessageChannel _channel = null;

        private IUser _user = null;

        public InteractiveTextResponseType ResponseType
        {
            get => _responseType;
            internal set
            {
                if(_responseType != InteractiveTextResponseType.Any) throw new InvalidOperationException("Cannot set the response type because has been already setted.");
                _responseType = value;
            }
        }

        public string Message
        {
            get => _message;

            internal set
            {
                if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cannot set an interactive message builder's field to null or empty",nameof(Message));
                _message = value;
            }
        }

        public TimeSpan Timeout
        {
            get => _timeout;
            internal set
            {
                _timeout = value;
            }
        }

        public bool Repeat { get => _repeat; internal set => _repeat = value; }

        public IMessageChannel Channel
        {
            get => _channel;
            internal set
            {
                _channel = value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null", nameof(Channel));
                if (MessageCriteria.ContainsCriteriaType(typeof(EnsureSourceChannelCriterion))) throw new InvalidOperationException($"Cannot add a {nameof(EnsureFromChannelCriterion)} because {nameof(EnsureSourceChannelCriterion)} has been already selected");
                MessageCriteria.AddCriterion(new EnsureFromChannelCriterion(_channel));
            }
        }

        public Criteria<SocketMessage> MessageCriteria { get => _messageCriteria; internal set => _messageCriteria = value; }

        public String[] Options
        {
            get => _options;
            internal set
            {
                _options = value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null", nameof(value));
                ResponseType = InteractiveTextResponseType.Options;
            }
        }

        public IUser User
        {
            get => _user;
            internal set
            {
                _user = value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null", nameof(value));
                if(_messageCriteria.ContainsCriteriaType(typeof(EnsureSourceUserCriterion))) throw new InvalidOperationException($"Cannot add a {nameof(EnsureFromUserCriterion)} because {nameof(EnsureSourceUserCriterion)} has been already selected");
                _messageCriteria.AddCriterion(new EnsureFromUserCriterion(_user.Id));
            } 
        }

        public string CancelationWord
        {
            get => _cancelationWord;
            internal set => _cancelationWord = value;
        }

        public string CancelationMessage
        {
            get => _cancelationMessage;
            internal set => _cancelationMessage = value;
        }

        public string TimeoutMessage
        {
            get => _timeoutMessage;
            internal set => _timeoutMessage = value;
        }

        public InteractiveMessageBuilder SetCancelationMessage(string cancelationMessage)
        {
            CancelationMessage = cancelationMessage;
            return this;
        }

        public InteractiveMessageBuilder SetTimeoutMessage(string timeoutMessage)
        {
            TimeoutMessage = timeoutMessage;
            return this;
        }

        /// <summary>
        /// Let the user stop the <see cref="InteractiveMessage"/> with a certain word
        /// </summary>
        /// <param name="word">
        /// The word to stop the Interactive Message.
        /// </param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithCancellationWord([NotNull]string word)
        {
            CancelationWord = word;
            return this;
        }

        /// <summary>
        /// Interactive message Builder
        /// </summary>
        /// <param name="message">The message to send.</param>
        public InteractiveMessageBuilder([NotNull]string message)
        {
            Message = message;
        }


        /// <summary>
        /// Specify the channel to read messages.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder SetChannel([NotNull]IMessageChannel channel)
        {
            Channel = channel;
            return this;
        }

        /// <summary>
        /// Adds a <see cref="Criteria{T}"/>
        /// </summary>
        /// <param name="criteriaType">The <see cref="Criteria{T}"/> to add</param>
        /// <exception cref="InvalidOperationException"> </exception>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder AddCriteria(CriteriaType criteriaType)
        {
            switch (criteriaType)
            {
                case CriteriaType.SourceUser:
                    if (_user != null || _messageCriteria.ContainsCriteriaType(typeof(EnsureFromUserCriterion))) throw new InvalidOperationException("Cannot add an SourceUser criteria because an user has been already selected");
                    MessageCriteria.AddCriterion(new EnsureSourceUserCriterion());
                    break;
                case CriteriaType.SourceChannel:
                    if(_channel != null || _messageCriteria.ContainsCriteriaType(typeof(EnsureFromChannelCriterion))) throw new InvalidOperationException("Cannot add an SourceChannel criteria because a channel has been already selected");
                    MessageCriteria.AddCriterion(new EnsureSourceChannelCriterion());
                    break;
                case CriteriaType.Empty:
                    MessageCriteria.AddCriterion(new EmptyCriterion<SocketMessage>());
                    break;
                default:
                    break;
            }
            return this;
        }

        /// <summary>
        /// Set a filter to the response.
        /// </summary>
        /// <param name="responseType">The response Type.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithResponseType(InteractiveTextResponseType responseType)
        {
            ResponseType = responseType;
            return this;
        }

        /// <summary>
        /// If loop is activated, won't stop until a valid response is given.
        /// </summary>
        /// <param name="withLoop"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder EnableLoop([NotNull]bool withLoop = true)
        {
            Repeat = withLoop;
            return this;
        }

        /// <summary>
        /// Specify a few options to be valid as response.
        /// </summary>
        /// <param name="options">The options</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder SetOptions([ItemNotNull] params String[] options)
        {
            Options = options;
            return this;
        }

        /// <summary>
        /// Set the user who can trigger the <see cref="InteractiveMessage"/>.
        /// </summary>
        /// <param name="user">The user to listen.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder SetUser([NotNull] IUser user)
        {
            User = user;

            return this;
        }

        /// <summary>
        /// Time the bot will wait for a reply.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder WithTimeSpan(TimeSpan timeSpan)
        {
            Timeout = timeSpan;
            return this;
        }

        /// <summary>
        /// Message to send when the Interactive Message starts.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>InteractiveMessageBuilder</returns>
        public InteractiveMessageBuilder SetMessage([NotNull]string message)
        {
            Message = message;
            return this;
        }

        /// <summary>
        /// Build an <see cref="InteractiveMessage"/>
        /// </summary>
        /// <returns><see cref="InteractiveMessage"/></returns>
        public InteractiveMessage Build() => new InteractiveMessage(Message, Timeout, ResponseType, Repeat,
            MessageCriteria, CancelationMessage, TimeoutMessage, CancelationWord, Channel, Options);
    }
}