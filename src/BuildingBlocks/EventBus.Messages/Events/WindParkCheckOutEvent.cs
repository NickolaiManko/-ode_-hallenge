using EventBus.Messages.Models;
using System.Collections.Generic;

namespace EventBus.Messages.Events
{
    public class WindParkCheckOutEvent : IntegrationBaseEvent
    {
        public List<WindPark> WindParks { get; set; }
    }
}
