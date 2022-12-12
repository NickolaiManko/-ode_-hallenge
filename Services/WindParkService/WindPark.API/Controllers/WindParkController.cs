using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WindPark.API.Repositories;

namespace WindPark.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WindParkController : ControllerBase
    {
        private readonly ILogger<WindParkController> _logger;
        private readonly IWindParkRepository _repository;

        public WindParkController(ILogger<WindParkController> logger, IWindParkRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        //[HttpGet]
        //public async Task<IEnumerable<EventBus.Messages.Models.WindPark>> Get()
        //{
        //    //return await _repository.GetWindParks();
        //}
    }
}
