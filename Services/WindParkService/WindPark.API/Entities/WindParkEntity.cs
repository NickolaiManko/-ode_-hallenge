using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WindPark.API.Entities
{
    public class WindParkEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public List<TurbineEntity> Turbines { get; set; } = new();
    }
}
