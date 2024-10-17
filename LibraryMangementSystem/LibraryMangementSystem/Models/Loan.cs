using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int MemberID { get; set; }
        public int BookID { get; set; }
        [Display(Name = "Loan Date")]
        public DateTime LoanDate { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }
        [Display(Name = "Penalty Cost")]
        public int PenaltyCost { get; set; } = 0;
        [Display(Name = "Loan Period")]
        public int LoanPeriod { get; set; }
        public bool Approved { get; set; }
        // nav properties
        public Book Book { get; set; }
        public Member Member { get; set; }
        
    }
}
