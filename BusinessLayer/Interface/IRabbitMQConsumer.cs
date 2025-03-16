using System.Threading;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IRabbitMQConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
