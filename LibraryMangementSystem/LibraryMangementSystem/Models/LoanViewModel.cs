using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class LoanViewModel : Loan
    {
        [Display(Name = "Member Email")]
        public string MemberEmail { get; set; }
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }
    }
}
