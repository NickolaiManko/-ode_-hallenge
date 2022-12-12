using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindPark.API.Entities
{
    public class TurbineEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Version { get; set; }
        public int MaxProduction { get; set; }
        public double CurrentProduction { get; set; }
        public double Windspeed { get; set; }
        public string WindDirection { get; set; }

        public int? WindParkEntityId { get; set; }
        [ForeignKey("WindParkEntityId")]
        public WindParkEntity? WindPark { get; set; }
    }
}
