using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Events;
using Kibrit.Smpp.Contracts;
using MassTransit;
using MediatR;

namespace Application.EventHandlers
{
    public class MessageCreatedEventHandler : INotificationHandler<DomainEventNotification<MessageCreatedEvent>>
    {
        private static readonly Uri Queue;

        private readonly IBus _bus;

        static MessageCreatedEventHandler()
        {
            Queue = new("queue:mnp_blacklist");
        }
        
        public MessageCreatedEventHandler(IBus bus)
        {
            _bus = bus;
        }
        
        public async Task Handle(
            DomainEventNotification<MessageCreatedEvent> notification, 
            CancellationToken cancellationToken
            )
        {
            var message = notification.DomainEvent.Message ??
                          throw new ArgumentNullException(nameof(notification.DomainEvent.Message));

            var checkMnp = new CheckMnp()
            {
                Id = message.Id,
                Destination = message.Destination,
                Source = message.Source,
                Tag = message.Tag,
                Text = message.Text
            };
            
            var sp = await _bus.GetSendEndpoint(Queue);
            await sp.Send(checkMnp, cancellationToken);
        }
    }
}