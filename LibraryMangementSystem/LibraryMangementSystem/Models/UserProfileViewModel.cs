using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
