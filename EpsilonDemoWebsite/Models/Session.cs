using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace EpsilonDemoWebsite.Models
{
    public class Session
    {

        public string Name { get; set; }
        // ------------------------------------------------------------------ //


        public string Email { get; set; }
        // ------------------------------------------------------------------ //

        public string Surname { get; set; }
        // ------------------------------------------------------------------ //

        public Session(string name, string email, string surname)
        {

            Name = name;

            Email = email;
            Surname = surname;

        }

        public Session()
        {
        }
    }
}

