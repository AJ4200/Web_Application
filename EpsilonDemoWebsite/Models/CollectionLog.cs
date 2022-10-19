using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class CollectionLog
    {
        public string Bin { get; set; }
        public string Waste { get; set; }
        public string Truck { get; set; }
        public string Driver { get; set; }
        public string Supervisor { get; set; }
        public string GardenSite { get; set; }
        public string Landfill { get; set; }
        public int RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public int CollectionNumber { get; set; }
        public DateTime CollectionDate { get; set; }
    }
}
