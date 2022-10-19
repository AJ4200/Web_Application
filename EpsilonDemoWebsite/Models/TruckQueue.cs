using System;
using System.Collections.Generic;

namespace EpsilonDemoWebsite.Models
{
    public partial class TruckQueue
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessingStartedAt { get; set; }
        public string Truck { get; set; }
    }
}