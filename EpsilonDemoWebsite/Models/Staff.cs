using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace EpsilonDemoWebsite.Models
{
    public class Staff
    {
        public string Dtype { get; set; }
        // ------------------------------------------------------------------ //

        public string StaffId { get; set; }
        // ------------------------------------------------------------------ //
       
        public string IdNumber { get; set; }
        // ------------------------------------------------------------------ //
        public string Name { get; set; }
        // ------------------------------------------------------------------ //
        public string Surname { get; set; }
        // ------------------------------------------------------------------ //
        
        public string Email { get; set; }
        // ------------------------------------------------------------------ //
       
        public string Password { get; set; }
        // ------------------------------------------------------------------ //
        
        public string Telephone { get; set; }
        // ------------------------------------------------------------------ //
        
        public string? LicenceNumber { get; set; }

        public bool Active { get; set; }

        public Staff(string dtype, string staffid, string idnumber, string name,string surname, string password,string email, string telephone, string? licencenumber, bool active)
        {
            Dtype = dtype;
            StaffId = staffid;
            IdNumber = idnumber;
            Name = name;
            Surname = surname;
            Password = password;
            Email = email;
            Telephone = telephone;
            LicenceNumber = licencenumber;
            Active = active;
        }

        public Staff()
        {
        }
    }
}

