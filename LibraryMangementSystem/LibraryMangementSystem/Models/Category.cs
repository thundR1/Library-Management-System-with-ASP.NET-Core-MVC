using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace LibraryMangementSystem.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Display(Name ="Category Name")]
        public string Name { get; set; }

        //nav property
        public ICollection<Book> Books { get; set; }
    }
}
