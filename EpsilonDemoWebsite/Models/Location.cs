namespace EpsilonDemoWebsite.Models
{
    public class Location
    {
        public string? Dtype { get; set; }
        public string Address { get; set; }
        public string LocationId { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        
        public string? Supervisor { get; set; }
        public bool? Active { get; set; }

        public Location(string dtype, string address, string locationId, string? longitude, string? latitude, string? supervisor, bool? active)
        {
            Dtype = dtype;
            Address = address;
            LocationId = locationId;
            Longitude = longitude;
            Latitude = latitude;
           
            Supervisor = supervisor;
            Active = active;
        }

        public Location()
        {
            //default;
        }
    }
}
