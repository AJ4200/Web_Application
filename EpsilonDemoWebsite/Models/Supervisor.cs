using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class Supervisor
    {
        [Key]
        public string? StaffId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Telephone { get; set; }
        public bool Active { get; internal set; }

        public Supervisor(string staffid, string name, string surname, string email, string telephone, bool active)
        {
            StaffId = staffid;
            Name = name;
            Surname = surname;
            Email = email;
            Telephone = telephone;
            Active = active;
        }

        public Supervisor()
        {
        }
    }
}
