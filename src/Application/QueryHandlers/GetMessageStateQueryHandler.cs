using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Queries;
using MediatR;

namespace Application.QueryHandlers
{
    public class GetMessageStateQueryHandler : IRequestHandler<GetMessageStateQuery, int>
    {
        private readonly IMessageStateRepository _repository;

        public GetMessageStateQueryHandler(IMessageStateRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<int> Handle(GetMessageStateQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetState(request.Id);
        }
    }
}