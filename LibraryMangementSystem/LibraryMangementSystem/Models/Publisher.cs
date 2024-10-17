namespace LibraryMangementSystem.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string Name { get; set;}
        public string Address { get; set;}

        // nav properties
        public ICollection<Book> books { get; set; }
        
    }
}
