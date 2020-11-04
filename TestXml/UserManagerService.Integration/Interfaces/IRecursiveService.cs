using System.Threading;
using System.Threading.Tasks;

namespace UserManagerService.Integration.Interfaces
{
    /// <summary>
    /// Methods which calls in recursive way
    /// </summary>
    public interface IRecursiveService
    {
        /// <summary>
        /// Execute method asynchronously
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}