﻿using LatinoNetOnline.Backend.Shared.Abstractions.Modules;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Messaging
{
    public class BackgroundDispatcher : BackgroundService
    {
        private readonly IMessageChannel _messageChannel;
        private readonly IModuleClient _moduleClient;
        private readonly ILogger<BackgroundDispatcher> _logger;

        public BackgroundDispatcher(IMessageChannel messageChannel, IModuleClient moduleClient,
            ILogger<BackgroundDispatcher> logger)
        {
            _messageChannel = messageChannel;
            _moduleClient = moduleClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Running the background dispatcher.");

            await foreach (var message in _messageChannel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await _moduleClient.PublishAsync(message);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            }

            _logger.LogInformation("Finished running the background dispatcher.");

        }
    }
}