using AutoMapper;
using EventBus.Messages.Models;
using WindPark.API.Entities;

namespace WindPark.API.Mapping
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<TurbineEntity, Turbine>().ReverseMap();
            CreateMap<WindParkEntity, EventBus.Messages.Models.WindPark>()
                .ForMember(c => c.Turbines, options => options.MapFrom(s => s.Turbines))
                .ReverseMap();
        }
    }
}
