using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class Librarian
    {
        [Key]
        public int LibrarianID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string IdentityUserId { get; set; }

        // nav property
        
        public IdentityUser IdentityUser { get; set; }
    }
}
