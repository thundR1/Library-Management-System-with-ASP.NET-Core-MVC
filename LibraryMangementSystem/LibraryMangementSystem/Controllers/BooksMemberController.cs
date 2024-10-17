using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles = "Member")]
    public class BooksMemberController : Controller
    {
        private readonly LibraryContext database;
        private readonly UserManager<IdentityUser> _userManager;
        public BooksMemberController(LibraryContext db, UserManager<IdentityUser> userManager)
        {
            database = db;
            _userManager = userManager;
        }
        public IActionResult Index(int? page)
        {
            var books = database.Books
                .Include(s => s.Publisher)
                .Include(s => s.Category)
                .Include(s => s.Author)
                .ToList();
            int PageSize = 12;
            int PageNumber = (page ?? 1);
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View(item);
        }



        // create actions
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

        [HttpGet]
        public IActionResult Loan(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = database.Members.FirstOrDefault(m => m.IdentityUserId == userId);
            if (member != null)
            {
                ViewBag.MemberID = member.MemberID;

                var book = database.Books.Find(id);
                if (book != null)
                {
                    ViewBag.BookID = book.BookID;
                    return View();
                }

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Loan(Loan loan)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var member = await database.Members.FirstOrDefaultAsync(m => m.IdentityUserId == user.Id);
            if (member == null)
            {
                return RedirectToAction("Register", "Account");
            }
            if (string.IsNullOrEmpty(member.FirstName) || string.IsNullOrEmpty(member.LastName) ||
                string.IsNullOrEmpty(member.Phone) || string.IsNullOrEmpty(member.Address))
            {
                return RedirectToAction("CompleteProfile");
            }
            if (loan != null)
            {
                loan.DueDate = loan.LoanDate.AddDays(loan.LoanPeriod);
                database.Loans.Add(loan);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(loan);
        }

        [HttpGet]
        public IActionResult CompleteProfile() => View();

        [HttpPost]
        public async Task<IActionResult> CompleteProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var member = await database.Members.FirstOrDefaultAsync(m => m.IdentityUserId == user!.Id);

                if (member != null)
                {
                    member.FirstName = model.FirstName;
                    member.LastName = model.LastName;
                    member.Phone = model.Phone;
                    member.Address = model.Address;
                    database.Update(member);
                    await database.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Search
        public async Task<IActionResult> SearchByTitle(string bookTitle, int? page)
        {
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
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
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
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
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
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
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
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
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
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
        public async Task<IActionResult> SearchByStock(string bookStock, int? page)
        {
            var books = database.Books.Include(s => s.Publisher).Include(s => s.Category).Include(s => s.Author).ToList();
            int PageSize = 12;
            int PageNumber = (page ?? 1);
            if (bookStock != null)
            {
                books = await database.Books.Where(b => b.Quantity >= Convert.ToInt32(bookStock)).ToListAsync();
            }
            int totalItems = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookStock;
            ViewBag.CurrentAction = "SearchByStock";
            var item = books.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }

        public IActionResult GetCover(int id)
        {
            var book = database.Books.Find(id);
            if (book?.Cover != null)
            {
                byte[] img = (byte[])book.Cover.ToArray();
                return File(img, "image/jpg");
            }
            return NotFound();
        }
    }
}
