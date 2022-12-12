using EventBus.Messages.Common;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WindParkAPI.Daemon.Services;

namespace WindParkAPI.Daemon.Workers
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IParkService _parkService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IParkService parkService, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _parkService = parkService ?? throw new ArgumentNullException(nameof(parkService));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            var requestTimeGapInMilliseconds = _configuration.GetValue<int>("APISettings:TimeGapBetweenParkRequestsInSeconds");
            var requestsDelayInSeconds = _configuration.GetValue<int>("APISettings:RequestsDelayInSeconds") * 1000;

            while (!cancellationToken.IsCancellationRequested)
            {
                var eventMessage = new WindParkCheckOutEvent();

                try
                {
                    var data = new List<WindPark>();
                    var periodEnd = DateTimeOffset.Now.AddSeconds(requestTimeGapInMilliseconds);
                    while (DateTimeOffset.Now < periodEnd)
                    {
                        var requestId = Guid.NewGuid();
                        var requestTime = DateTimeOffset.Now;

                        try
                        {
                            data.AddRange(await _parkService.GetWindParksAsync(requestId, requestTime));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"CorrelationId = {eventMessage.CorrelationId}. Request ID = {requestId}: Request at {requestTime} ended with exception. Exception message: {ex.Message}");
                        }

                        await Task.Delay(requestsDelayInSeconds);
                    }

                    eventMessage.WindParks = AggregateData(data);

                    var factory = new ConnectionFactory() { HostName = _configuration.GetValue<string>("EventBusSettings:HostAddress") };

                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: EventBusConstants.WindParkCheckoutQueue, type: ExchangeType.Fanout);

                        var message = JsonSerializer.Serialize(eventMessage);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: EventBusConstants.WindParkCheckoutQueue,
                                             routingKey: "",
                                             basicProperties: null,
                                             body: body);

                        _logger.LogInformation($"{nameof(WindParkCheckOutEvent)} published successfully. CorrelationId = {eventMessage.CorrelationId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"CorrelationId = {eventMessage.CorrelationId}: Event at {eventMessage.CreationDate} ended with exception. Exception message: {ex.Message}");
                }
            }
        }
        public List<WindPark> AggregateData(List<WindPark> data)
        {
            return data.GroupBy(x => x.Id)
                        .Select(group => new { groupKey = group.Key, Items = group.ToList() })
                        .Select(x => new WindPark
                        {
                            Id = x.groupKey,
                            Country = x.Items[0].Country,
                            Description = x.Items[0].Description,
                            Name = x.Items[0].Name,
                            Region = x.Items[0].Region,
                            Turbines = x.Items.SelectMany(t => t.Turbines)
                                                .GroupBy(y => y.Id)
                                                .Select(group => new { groupKey = group.Key, Items = group.ToList() })
                                                .Select(y => new Turbine
                                                {
                                                    Id = y.groupKey,
                                                    Name = y.Items[0].Name,
                                                    Manufacturer = y.Items[0].Manufacturer,
                                                    Version = y.Items[0].Version,
                                                    WindDirection = y.Items[0].WindDirection,
                                                    MaxProduction = y.Items[0].MaxProduction,
                                                    CurrentProduction = y.Items.Sum(a => a.CurrentProduction),
                                                    Windspeed = y.Items.Average(a => a.Windspeed),
                                                })
                                                .ToList()
                        })
                        .ToList();
        }
    }
}
