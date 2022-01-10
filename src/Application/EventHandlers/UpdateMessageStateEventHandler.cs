using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Dtos;
using AutoMapper;
using Domain.Events;
using Kibrit.Common.Kafka.Interfaces;
using Kibrit.Smpp.Common;
using MediatR;

namespace Application.EventHandlers
{
    public class UpdateMessageStateEventHandler : INotificationHandler<DomainEventNotification<UpdateMessageStateEvent>>
    {
        private readonly IKafkaProducer<string, string> _kafkaProducer;
        private readonly IMapper _mapper;
        public UpdateMessageStateEventHandler(IKafkaProducer<string, string> kafkaProducer, IMapper mapper)
        {
            _kafkaProducer = kafkaProducer;
            _mapper = mapper;
        }
        
        public async Task Handle(
            DomainEventNotification<UpdateMessageStateEvent> notification, 
            CancellationToken cancellationToken
            )
        {
            var message = notification.DomainEvent.Message ??
                          throw new ArgumentNullException(nameof(notification.DomainEvent.Message));
            
            var messageDto = _mapper.Map<MessageStateDto>(message);
            messageDto.State = MessageState.Created;
            messageDto.Timestamp = notification.DomainEvent.DateOccurred.ToString("yyyy-MM-dd HH:mm:ss");

            await _kafkaProducer.ProduceAsync(
                "message_state", 
                messageDto.Id, 
                JsonSerializer.Serialize(messageDto)
                );
        }
    }
}