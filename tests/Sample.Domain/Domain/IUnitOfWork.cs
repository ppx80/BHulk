using System.Threading;
using System.Threading.Tasks;

namespace Sample.Domain.Domain
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }   
}
