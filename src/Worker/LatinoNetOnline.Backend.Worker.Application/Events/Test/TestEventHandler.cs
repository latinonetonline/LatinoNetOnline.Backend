using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Worker.Application.Events.Test
{
    class TestEventHandler : IEventHandler<TestEventInput>
    {
        private readonly ILogger<TestEventHandler> _logger;

        public TestEventHandler(ILogger<TestEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TestEventInput input, CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);

            _logger.LogInformation("Mensaje: " + input.Message);
        }
    }
}
