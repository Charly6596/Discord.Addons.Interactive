using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Discord.Addons.Interactive.InteractiveBuilder;
using Discord.WebSocket;
using JetBrains.Annotations;

namespace Discord.Addons.Interactive
{
    public class InteractiveQueue
    {
        private ConcurrentQueue<InteractiveMessage> _queue = new ConcurrentQueue<InteractiveMessage>();

        public InteractiveMessage defaultQueueOptions { get; set; }

        //public Queue<InteractiveMessage> Queue { get => _queue; internal set => _queue = value; }

        public InteractiveQueue()
        {
            this.defaultQueueOptions = new InteractiveMessageBuilder().Build();
        }

        public InteractiveQueue(InteractiveMessage interactiveMessage)
        {
            defaultQueueOptions = interactiveMessage;
        }

        public InteractiveQueue Add([NotNull] params InteractiveMessage[] interactiveMessages)
        {
            foreach (var interactiveMessage in interactiveMessages)
            {
                var interactiveMessageType = typeof(InteractiveMessage);
                var defaultOptionsType = typeof(InteractiveMessageDefaultOptions);
                foreach (var property in interactiveMessageType.GetProperties())
                {
                    var propertyName = property.Name;
                    if (property.GetValue(interactiveMessage, null) ==
                        defaultOptionsType.GetProperty(propertyName)?.GetValue(null, null))
                    {
                        property.SetValue(interactiveMessage,
                            interactiveMessageType.GetProperty(propertyName)?.GetValue(defaultQueueOptions));
                    }
                }

                _queue.Enqueue(interactiveMessage);
            }

            return this;
        }

        public Task<SocketMessage> Next(InteractiveBase interactiveBase)
        {
            var result = _queue.TryDequeue(out var interactiveMessage);
            return result ? interactiveBase.StartInteractiveMessage(interactiveMessage) : null;
        }
    }
}