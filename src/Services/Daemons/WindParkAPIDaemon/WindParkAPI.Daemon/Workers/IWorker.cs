using System.Threading;
using System.Threading.Tasks;

namespace WindParkAPI.Daemon.Workers
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
