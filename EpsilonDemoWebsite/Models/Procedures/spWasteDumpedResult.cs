using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonDemoWebsite.Models
{
    public partial class spWasteDumpedResult
    {
        public string WasteType { get; set; }
        public int? NumberOfLoads { get; set; }
        public string Month { get; set; }
        public decimal? Tonnage { get; set; }
    }
}
