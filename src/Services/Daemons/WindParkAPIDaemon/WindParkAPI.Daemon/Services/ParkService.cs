using EventBus.Messages.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WindParkAPI.Daemon.Services
{
    public class ParkService : IParkService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ParkService> _logger;

        public ParkService(IConfiguration configuration, ILogger<ParkService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var baseAddress = _configuration.GetValue<string>("APISettings:WindParkBaseAddress");

            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<List<WindPark>> GetWindParksAsync(Guid requestId, DateTimeOffset requestTime)
        {
            var url = $"{_configuration.GetValue<string>("APISettings:GetWindParksUrl")}";

            var result = new List<WindPark>();

            try
            {
                _logger.LogInformation($"Request ID = {requestId}: Sending request to {_client.BaseAddress}{url} at {requestTime}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    result = JsonSerializer.Deserialize<List<WindPark>>(stringResponse);

                    _logger.LogInformation($"Request ID = {requestId}: Request to {_client.BaseAddress}{url} at {requestTime} successfully ended.");
                }
                else
                {
                    _logger.LogError($"Request ID = {requestId}: Request to {_client.BaseAddress}{url} at {requestTime} ended with {response.StatusCode} statusCode.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Request ID = {requestId}: Request to {_client.BaseAddress}{url} at {requestTime} ended with exception. Exception message: {ex.Message}");
            }

            return result;
        }
    }
}
