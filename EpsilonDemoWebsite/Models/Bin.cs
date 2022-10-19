using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace EpsilonDemoWebsite.Models
{
    public class Bin
    {

        public string BinId { get; set; }
        public byte[] Qrcode { get; set; }
        public int Waste { get; set; }

        public bool Active { get; set; }



        public Bin(string id, int waste, byte[] qrcode, bool active)
        {
            BinId = id;
            Qrcode = qrcode;
            Waste = waste;
            Active = active;
        }

        public Bin()
        {
        }
    }
}


