using System.Threading;
using System.Threading.Tasks;
using UserManagerService.Integration.Interfaces;

namespace UserManagerService.Integration
{
    public class RecursiveExecutionService : IRecursiveService
    {
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}