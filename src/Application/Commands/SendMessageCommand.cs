using System;
using Application.Common.Mappings;
using Kibrit.Smpp.Contracts;
using MediatR;

namespace Application.Commands
{
    public class SendMessageCommand : IMapFrom<SendMessage>, IRequest<Guid>
    {
        public string Source { get; init; }
        public string Destination { get; set; }
        public string Text { get; init; }
        public string Tag { get; init; }
    }
}