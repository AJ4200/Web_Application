using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class ControlStation
    {
        public string LocationId { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
    }
}
