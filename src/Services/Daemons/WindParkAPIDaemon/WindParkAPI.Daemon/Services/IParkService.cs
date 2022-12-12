using EventBus.Messages.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindParkAPI.Daemon.Services
{
    public interface IParkService
    {
        Task<List<WindPark>> GetWindParksAsync(Guid requestId, DateTimeOffset requestTime);
    }
}
