using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class Landfill
    {
        public string LocationId { get; set; }
        public string Address { get; set; }
        public decimal? Capacity { get; set; }
        public bool Active { get; set; }
    }
}
