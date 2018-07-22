using System;
using System.Collections.Generic;
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

        private IMessageChannel _channel;

        public InteractiveTextResponseType ResponseType { get => _responseType; internal set => _responseType = value; }

        

        public string Message
        {
            get => _message;

            internal set
            {
                if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cannot set an interactive message builder's field to null or empty",nameof(value));
                _message = value;
            }
        }

        public TimeSpan Timeout { get => _timeout; internal set => _timeout = value; }

        public bool Repeat { get => _repeat; internal set => _repeat = value; }

        public IMessageChannel Channel
        {
            get => _channel;
            internal set
            {
                _channel = value ?? throw new ArgumentNullException("Cannot set an interactive message builder's field to null", nameof(value));
            }
        }

        public Criteria<SocketMessage> MessageCriteria { get => _messageCriteria; internal set => _messageCriteria = value; }

        public List<string> Options { get => _options; internal set
        {
            _responseType = InteractiveTextResponseType.Options;
            _options = value;
        }  }

        public InteractiveMessageBuilder(string message)
        {
            Message = message;
        }

        public InteractiveMessageBuilder SetChannel(IMessageChannel channel)
        {
            Channel = channel;
            _messageCriteria.AddCriterion(new EnsureFromChannelCriterion(Channel));
            return this;
        }
        public InteractiveMessageBuilder AddCriteria(CriteriaType criteriaType)
        {
            switch (criteriaType)
            {
                case CriteriaType.SourceUser:
                    _messageCriteria.AddCriterion(new EnsureSourceUserCriterion());
                    break;
                case CriteriaType.SourceChannel:
                    _messageCriteria.AddCriterion(new EnsureSourceChannelCriterion());
                    break;
                case CriteriaType.Empty:
                    _messageCriteria.AddCriterion(new EmptyCriterion<SocketMessage>());
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