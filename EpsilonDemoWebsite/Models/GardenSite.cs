using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class GardenSite
    {
        public string LocationId { get; set; }
        public string Address { get; set; }
        public string Supervisor { get; set; }
        public bool Active { get; set; }
    }
}
