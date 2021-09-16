
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Modules
{
    public interface IModuleClient
    {
        Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class;
        Task PublishAsync(object message);
    }
}
