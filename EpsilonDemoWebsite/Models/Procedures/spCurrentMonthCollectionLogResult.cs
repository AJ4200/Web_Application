using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonDemoWebsite.Models
{
    public partial class spCurrentMonthCollectionLogResult
    {
        public int CollectionNumber { get; set; }
        public string Driver { get; set; }
        public string SentTo { get; set; }
        public string Waste { get; set; }
        public string Supervisor { get; set; }
        public DateTime? CollectionDate { get; set; }
    }
}
