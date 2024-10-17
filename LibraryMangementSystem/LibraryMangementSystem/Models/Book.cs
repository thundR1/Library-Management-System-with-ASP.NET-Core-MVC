namespace LibraryMangementSystem.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int? CategoryID { get; set; }
        public int? PublisherID { get; set; }
        public int? AuthorID { get; set; }
        public int? Quantity { get; set; }
        public double Price { get; set; }
        public byte[]? Cover { get; set; }

        // nav properties
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public Author Author { get; set; }
    }
}
