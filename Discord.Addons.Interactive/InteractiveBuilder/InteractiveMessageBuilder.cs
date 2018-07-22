using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

// ReSharper disable CheckNamespace
namespace Discord.Addons.Interactive.InteractiveBuilder
{
    public class InteractiveMessageBuilder
    {
        private string _message = String.Empty;

        private TimeSpan _timeout = System.TimeSpan.FromSeconds(15);

        private bool _repeat = false;

        private InteractiveTextResponseType _responseType = InteractiveTextResponseType.Any;

        private Criteria<SocketMessage> _messageCriteria = new Criteria<SocketMessage>();

        private List<string> _options;

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

        public List<string> Options
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

        public InteractiveMessageBuilder(string message)
        {
            Message = message;
        }

        public InteractiveMessageBuilder SetChannel(IMessageChannel channel)
        {
            Channel = channel;
            return this;
        }
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
        public InteractiveMessageBuilder WithLoop(bool withLoop = true)
        {
            Repeat = withLoop;
            return this;
        }

        public InteractiveMessageBuilder SetOptions(params String[] options)
        {
            Options = options.ToList();
            return this;
        }

        public InteractiveMessageBuilder SetUser(IUser user)
        {
            User = user;

            return this;
        }
        /// <summary>
        /// Time the bot will wait for a reply.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder WithTimeSpan(TimeSpan timeSpan)
        {
            Timeout = timeSpan;
            return this;
        }

        /// <summary>
        /// Message to send.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public InteractiveMessageBuilder SetMessage(string message)
        {
            Message = message;
            return this;
        }
        public InteractiveMessage Build()
        {
            return new InteractiveMessage(_message, _timeout, _responseType, _repeat, _messageCriteria, _channel, _options);
        }
    }
}