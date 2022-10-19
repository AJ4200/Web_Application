using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonDemoWebsite.Models
{
    public partial class spGardenSiteTrafficResult
    {
        public string Area { get; set; }
        public int Requests { get; set; }
    }
}
