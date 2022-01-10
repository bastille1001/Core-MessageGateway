using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMessageStateRepository
    {
        Task<int> GetState(Guid id);
    }
}