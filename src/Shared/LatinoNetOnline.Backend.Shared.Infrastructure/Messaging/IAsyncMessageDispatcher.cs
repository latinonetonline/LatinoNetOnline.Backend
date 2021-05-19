using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Messaging
{
    public interface IAsyncMessageDispatcher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}