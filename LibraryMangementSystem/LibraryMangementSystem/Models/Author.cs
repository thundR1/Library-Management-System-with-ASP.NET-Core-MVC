using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        public string FullName { get; set; }

        public ICollection<Book> Books { get; set; }


    }
}
