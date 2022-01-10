using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class MessageCreatedEvent : DomainEvent
    {
        public Message Message { get; }

        public MessageCreatedEvent(Message message)
        {
            Message = message;
        }
    }
}