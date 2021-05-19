using LocatePlate.Infrastructure.Domain;

namespace LocatePlate.Model.Location
{
    public class CanadaCity : BaseEntity
    {
        public string City { get; set; }
        public string CityAscii { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public float Population { get; set; }
        public float Density { get; set; }
        public string TimeZone { get; set; }
        public int Ranking { get; set; }
        public int Postal { get; set; }
    }
}
