using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class BooksController : Controller
    {
        private readonly LibraryContext database;
        public BooksController(LibraryContext db)
        {
            database = db;
        }
        public IActionResult Index(int? page)
        {
            var books = database.Books
                .Include(s => s.Publisher)
                .Include(s => s.Category)
                .Include(s => s.Author)
                .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View(item);
        }

        //Search
        public async Task<IActionResult> SearchByTitle(string bookTitle, int? page)
        {
            var books = database.Books
                .Include(s => s.Publisher)
                .Include(s => s.Category)
                .Include(s => s.Author)
                .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookTitle != null)
            {
                books = await database.Books.Where(b => b.Title.Contains(bookTitle)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookTitle;
            ViewBag.CurrentAction = "SearchByTitle";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }

        public async Task<IActionResult> SearchByISBN(string bookISBN, int? page)
        {
            var books = database.Books
              .Include(s => s.Publisher)
              .Include(s => s.Category)
              .Include(s => s.Author)
              .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookISBN != null)
            {
                books = await database.Books.Where(b => b.ISBN.StartsWith(bookISBN)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookISBN;
            ViewBag.CurrentAction = "SearchByISBN";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }
        public async Task<IActionResult> SearchByCat(string bookCat, int? page)
        {
            var books = database.Books
              .Include(s => s.Publisher)
              .Include(s => s.Category)
              .Include(s => s.Author)
              .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookCat != null)
            {
                books = await database.Books.Where(b => b.Category.Name.StartsWith(bookCat)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookCat;
            ViewBag.CurrentAction = "SearchByCat";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }
        public async Task<IActionResult> SearchByPub(string bookPub, int? page)
        {
            var books = database.Books
                .Include(s => s.Publisher)
                .Include(s => s.Category)
                .Include(s => s.Author)
                .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookPub != null)
            {
                books = await database.Books.Where(b => b.Publisher.Name.StartsWith(bookPub)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookPub;
            ViewBag.CurrentAction = "SearchByPub";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }
        public async Task<IActionResult> SearchByAuthor(string bookAuthor, int? page)
        {
            var books = database.Books
              .Include(s => s.Publisher)
              .Include(s => s.Category)
              .Include(s => s.Author)
              .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookAuthor != null)
            {
                books = await database.Books.Where(b => b.Author.FullName.Contains(bookAuthor)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookAuthor;
            ViewBag.CurrentAction = "SearchByAuthor";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }
        public async Task<IActionResult> SearchByStock(string minStock, string maxStock, int? page)
        {
            var books = database.Books
                .Include(s => s.Publisher)
                .Include(s => s.Category)
                .Include(s => s.Author)
                .ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (minStock != null && maxStock != null)
            {
                books = await database.Books.Where(b => b.Quantity >= Convert.ToInt32(minStock) && b.Quantity <= Convert.ToInt32(maxStock)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = minStock;
            ViewBag.Search2 = maxStock;
            ViewBag.CurrentAction = "SearchByStock";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }



        // crud actions
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Pubs = new SelectList(database.Publishers.ToList(), "PublisherID", "Name");
            //ViewBag.Cats = new SelectList(database.Categories.ToList(), "CategoryID", "Name");
            //ViewBag.Authors = new SelectList(database.Authors.ToList(), "AuthorID", "FullName");
            ViewBag.ReturnUrl = Request.GetDisplayUrl();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile bookCover, int selectedAuthor, int selectedPublisher, int selectedCategory)
        {
            if (book != null)
            {
                if (bookCover != null)
                {
                    byte[] img;
                    using (var x = bookCover.OpenReadStream())
                    using (var stream = new MemoryStream())
                    {
                        x.CopyTo(stream);
                        img = stream.ToArray();
                    }
                    book.Cover = img;
                }
                book.AuthorID = selectedAuthor;
                book.PublisherID = selectedPublisher;
                book.CategoryID = selectedCategory;
                database.Books.Add(book);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAuthors(string searchAuthor)
        {
            var authors = database.Authors;

            var items = await authors
           .Where(i => i.FullName.Contains(searchAuthor))
           .Select(i => new { id = i.AuthorID, text = i.FullName })
           .ToListAsync();

            return Json(items);
        }
        [HttpGet]
        public async Task<IActionResult> SearchPublishers(string searchPublisher)
        {
            var publishers = database.Publishers;

            var items = await publishers
       .Where(i => i.Name.Contains(searchPublisher))
       .Select(i => new { id = i.PublisherID, text = i.Name })
       .ToListAsync();

            return Json(items);
        }
        [HttpGet]
        public async Task<IActionResult> SearchCategories(string searchCategory)
        {
            var publishers = database.Categories;

            var items = await publishers
       .Where(i => i.Name.Contains(searchCategory))
       .Select(i => new { id = i.CategoryID, text = i.Name })
       .ToListAsync();

            return Json(items);
        }

        public FileResult GetCover(int id)
        {
            var book = database.Books.Find(id);
            byte[] img = (byte[])book.Cover.ToArray();
            return File(img, "image/jpeg", "image/png");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var book = database.Books.Find(id);
            if (book != null)
            {
                //ViewBag.Publisher = database.Publishers.Find(book.PublisherID).Name;
                //ViewBag.Category = database.Categories.Find(book.CategoryID).Name;
                //ViewBag.Author = database.Authors.Find(book.AuthorID).FullName;
                return View(book);
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book newBook, IFormFile bookCover, int selectedAuthor, int selectedPublisher, int selectedCategory)
        {
            var currentBook = database.Books.Find(newBook.BookID);
            if (bookCover != null)
            {
                byte[] img;
                using (var x = bookCover.OpenReadStream())
                using (var stream = new MemoryStream())
                {
                    x.CopyTo(stream);
                    img = stream.ToArray();
                }
                newBook.Cover = img;
            }
            if (currentBook != null)
            {
                currentBook.Cover = newBook.Cover;
                currentBook.Title = newBook.Title;
                currentBook.ISBN = newBook.ISBN;
                currentBook.Quantity = newBook.Quantity;
                currentBook.Price = newBook.Price;
                if(database.Publishers.Find(selectedPublisher) != null)currentBook.PublisherID = selectedPublisher;
                if (database.Categories.Find(selectedCategory) != null) currentBook.CategoryID = selectedCategory;
                if (database.Authors.Find(selectedAuthor) != null) currentBook.AuthorID = selectedAuthor;
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(newBook);
        }

        public IActionResult Delete(int id)
        {

            var book = database.Books.Find(id);
            if (book != null)
            {
                string pubName = database.Publishers.Find(book.PublisherID)?.Name ?? "Unknown";
                string catName = database.Categories.Find(book.CategoryID)?.Name ?? "No Category";
                string authName = database.Authors.Find(book.AuthorID)?.FullName ?? "Unknown";

                ViewBag.PublisherName = pubName;
                ViewBag.CategoryName = catName;
                ViewBag.AuthorName = authName;

                return View(book);
            }


            return NotFound();
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = database.Books.Find(id);
            if (book != null)
            {
                database.Books.Remove(book);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
        public IActionResult Details(int id)
        {

            var book = database.Books.Find(id);
            if (book != null)
            {
                string pubName = database.Publishers.Find(book.PublisherID)?.Name ?? "Unknown";
                string catName = database.Categories.Find(book.CategoryID)?.Name ?? "No Category";
                string authName = database.Authors.Find(book.AuthorID)?.FullName ?? "Unknown";

                ViewBag.PublisherName = pubName;
                ViewBag.CategoryName = catName;
                ViewBag.AuthorName = authName;
                return View(book);
            }

            return NotFound();

        }


    }
}
