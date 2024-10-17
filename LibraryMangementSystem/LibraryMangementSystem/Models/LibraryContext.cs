using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryMangementSystem.Models
{
    public class LibraryContext : IdentityDbContext<IdentityUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // DbSet properties for each entity
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Loan> Loans { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.IdentityUser)
                .WithMany()
                .HasForeignKey(m => m.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Librarian>()
                .HasOne(l => l.IdentityUser)
                .WithMany()
                .HasForeignKey(l => l.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
