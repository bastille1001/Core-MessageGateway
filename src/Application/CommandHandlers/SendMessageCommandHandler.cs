using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.CommandHandlers
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Guid>
    {
        private readonly IDomainEventService _publisher;
        
        public SendMessageCommandHandler(IDomainEventService publisher)
        {
            _publisher = publisher;
        }
        
        public Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Source = request.Source,
                Destination = request.Destination,
                Tag = request.Tag,
                Text = request.Text
            };
            
            _publisher.Publish(new UpdateMessageStateEvent(message));
            _publisher.Publish(new MessageCreatedEvent(message));
            return Task.FromResult(message.Id);
        }
    }
}