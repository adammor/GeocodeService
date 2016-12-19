using RestEL.Modules;
using System;

namespace GeocodeService.Modules
{
    [RegisterEntity(EntityName = "StreetAddress")]
    public class StreetAddress
    {
        public string DoctorID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public Guid ServiceAPIEndPointUID { get; set; }
        public string LocationType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int SRID { get; set; }
    }
}
