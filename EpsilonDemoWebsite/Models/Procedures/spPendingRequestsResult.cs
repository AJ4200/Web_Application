using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonDemoWebsite.Models
{
    public partial class spPendingRequestsResult
    {
        public int RequestNumber { get; set; }
        public string Bin { get; set; }
        public string Waste { get; set; }
        public string GardenSite { get; set; }
        public string Supervisor { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
