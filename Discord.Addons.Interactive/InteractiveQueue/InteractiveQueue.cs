using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Addons.Interactive.InteractiveBuilder;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public class InteractiveQueue
    {
        private Queue<InteractiveMessage> _queue = new Queue<InteractiveMessage>();

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

        public InteractiveQueue Add(InteractiveMessage interactiveMessage)
        {
            var interactiveMessageType = typeof(InteractiveMessage);
            var defaultOptionsType = typeof(InteractiveMessageDefaultOptions);
            foreach (var property in interactiveMessageType.GetProperties())
            {
                var propertyName = property.Name;
                if (property.GetValue(interactiveMessage, null) == defaultOptionsType.GetProperty(propertyName)?.GetValue(null, null))
                {
                    property.SetValue(interactiveMessage, interactiveMessageType.GetProperty(propertyName)?.GetValue(defaultQueueOptions));
                }
            }
            _queue.Enqueue(interactiveMessage);
            return this;
        }   

        public Task<SocketMessage> Next(InteractiveBase interactiveBase)
        {
            var interactiveMessage = _queue.Dequeue();
            return interactiveBase.StartInteractiveMessage(interactiveMessage);
        }

//        public async Task RunAll()
//        {
//            var block = new ActionBlock<QueryAvailabilityMultidayRequest>
//
//        }
    }
}