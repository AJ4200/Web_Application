using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace EpsilonDemoWebsite.Models
{
    public class BinView
    {
        public string Bin { get; set; }
        public string Waste { get; set; }

        public BinView(string id, string waste)
        {
            Bin = id;
            Waste = waste;
        }

        public BinView()
        {
        }
    }
}


