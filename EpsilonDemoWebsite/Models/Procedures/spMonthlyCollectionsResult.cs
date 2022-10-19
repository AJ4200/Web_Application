using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonDemoWebsite.Models
{
    public partial class spMonthlyCollectionsResult
    {
        public string Month { get; set; }
        public int Amount { get; set; }
    }
}
