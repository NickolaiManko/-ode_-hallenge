using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WindPark.API.EventBusConsumer
{
    public class WindParkCheckOutConsumer : IConsumer<WindParkCheckOutEvent>
    {
        private readonly ILogger<WindParkCheckOutConsumer> _logger;

        public WindParkCheckOutConsumer(ILogger<WindParkCheckOutConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<WindParkCheckOutEvent> context)
        {
            _logger.LogInformation($"{nameof(WindParkCheckOutEvent)} consumed successfully. CorrelationId = {context.Message.CorrelationId}");
        }
    }
}
