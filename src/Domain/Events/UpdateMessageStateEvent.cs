using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class UpdateMessageStateEvent : DomainEvent
    {
        public Message Message { get; }

        public UpdateMessageStateEvent(Message message)
        {
            Message = message;
        }
    }
}