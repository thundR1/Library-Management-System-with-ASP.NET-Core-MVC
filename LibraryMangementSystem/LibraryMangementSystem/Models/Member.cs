using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string IdentityUserId { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
