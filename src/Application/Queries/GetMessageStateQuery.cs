using System;
using MediatR;

namespace Application.Queries
{
    public class GetMessageStateQuery : IRequest<int>
    {
        public Guid Id { get; init; }
    }
}