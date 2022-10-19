using System.ComponentModel.DataAnnotations;

namespace EpsilonDemoWebsite.Models
{
    public class Driver
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
        [Required]
        public string? Rating { get; set; }
        public bool Active { get; internal set; }

        public Driver(string staffid, string name, string surname, string email, string telephone, string rating, bool active)
        {
            StaffId = staffid;
            Name = name;
            Surname = surname;
            Email = email;
            Telephone = telephone;
            Rating = rating;
            Active = active;
        }

        public Driver()
        {
        }
    }
}
