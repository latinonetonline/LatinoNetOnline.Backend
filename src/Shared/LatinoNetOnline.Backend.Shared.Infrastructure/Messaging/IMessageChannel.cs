using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using System.Threading.Channels;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Messaging
{
    public interface IMessageChannel
    {
        ChannelReader<IMessage> Reader { get; }
        ChannelWriter<IMessage> Writer { get; }
    }
}