using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class LoansController : Controller
    {
        private readonly LibraryContext database;
        public LoansController(LibraryContext db)
        {
            database = db;
        }
        public IActionResult Index(int? page)
        {
            var loans = database.Loans.Include(p => p.Book).Include(p => p.Member).ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            int totalItems = loans.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            var item = loans.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LoanViewModel loanData)
        {
            if (loanData != null)
            {
                var book = await database.Books.FirstOrDefaultAsync(s => s.Title == loanData.BookTitle);
                var member = await database.Members.FirstOrDefaultAsync(s => s.Email == loanData.MemberEmail);
                if (book == null || member == null) return View(loanData);
                if (book.Quantity < 1) return BadRequest("This book is out of stock");
                else
                {
                    var loan = new Loan()
                    {
                        LoanDate = loanData.LoanDate,
                        DueDate = loanData.DueDate,
                        BookID = book.BookID,
                        MemberID = member.MemberID,
                        Approved = loanData.Approved,
                        LoanPeriod = (loanData.DueDate - loanData.LoanDate).Days,
                    };
                    database.Loans.Add(loan);
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                
            }
            return NotFound();
        }
        public IActionResult ApproveLoan(int id)
        {
            var loan = database.Loans.Find(id);
            ViewBag.Book = database.Books.Find(loan.BookID).Title;
            ViewBag.Member = database.Members.Find(loan.MemberID).Email;
            return View(loan);
        }

        public async Task<IActionResult> LoanApproved(int id)
        {
            var loan = database.Loans.Find(id);
            if (loan != null)
            {
                loan.Approved = true;
                if (loan.LoanDate < DateTime.Now)
                {
                    loan.DueDate = DateTime.Now.AddDays(loan.LoanPeriod);
                }
                var borrowedBook = database.Books.Find(loan.BookID);
                if (borrowedBook != null && loan.ReturnDate == null) borrowedBook.Quantity--;
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var loan = database.Loans.Find(id);
            if (loan != null)
            {
                return View(loan);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Loan newLoan)
        {
            if (newLoan != null)
            {
                var oldLoan = database.Loans.Find(newLoan.LoanID);
                if (oldLoan != null)
                {
                    oldLoan.ReturnDate = newLoan.ReturnDate;
                    oldLoan.DueDate = newLoan.DueDate;
                    oldLoan.LoanDate = newLoan.LoanDate;
                    oldLoan.LoanPeriod = (newLoan.DueDate - newLoan.LoanDate).Days;

                    // New PenaltyCost
                    if (oldLoan.ReturnDate.HasValue)
                    {
                        oldLoan.PenaltyCost = GetPenaltyCostValue(oldLoan.ReturnDate.Value, oldLoan.DueDate);
                    }
                    else
                    {
                        oldLoan.PenaltyCost = 0;
                    }

                    await database.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Return(int id)
        {

            var loan = database.Loans.Find(id);
            ViewBag.DueDate = loan.DueDate;
            if (loan.ReturnDate != null) return NoContent();
            if (loan != null)
            {
                return View(loan);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Return(Loan loanData)
        {
            var loan = database.Loans.Find(loanData.LoanID);

            if (loan != null)
            {
                loan.ReturnDate = loanData.ReturnDate;
                if (loan.ReturnDate != null) loan.PenaltyCost = GetPenaltyCostValue(loan.ReturnDate.Value, loan.DueDate);
                loanData.PenaltyCost = loan.PenaltyCost;

                var returnedBook = database.Books.Find(loan.BookID);
                if (returnedBook != null) returnedBook.Quantity++;
                database.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();

        }

        public IActionResult Delete(int id)
        {
            var loan = database.Loans.Find(id);
            if (loan != null)
            {
                ViewBag.BookTitle = database.Books.Find(loan.BookID).Title;
                ViewBag.MemberName = database.Members.Find(loan.MemberID).Email;
                return View(loan);
            }
            return NotFound();
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = database.Loans.Find(id);
            if (loan != null)
            {
                database.Loans.Remove(loan);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        private int GetPenaltyCostValue(DateTime returnDate, DateTime dueDate)
        {
            int daysLate = (returnDate.Date - dueDate.Date).Days;
            if (daysLate <= 0)
            {
                return 0;
            }
            int penalty = (int)Math.Ceiling(daysLate / 5.0);
            return penalty;
        }

        [HttpPost]
        public IActionResult PenaltyCost(DateTime returnDate, DateTime dueDate)
        {
            int daysLate = (returnDate - dueDate).Days;
            int penalty = (int)Math.Ceiling(daysLate / 5.0);
            return Json(new { penalty });
        }

        //Search Actions
        public async Task<IActionResult> SearchByTitle(string bookTitle, int? page)
        {
            var loans = database.Loans.Include(p => p.Book).Include(p => p.Member).ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (bookTitle != null)
            {
                var Loans = await database.Loans.Where(b => b.Book.Title.Contains(bookTitle)).ToListAsync();
            }
            int totalItems = loans.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = bookTitle;
            ViewBag.CurrentAction = "SearchByTitle";
            var item = loans.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", loans);
        }
        public async Task<IActionResult> SearchByMember(string member, int? page)
        {
            var loans = database.Loans.Include(p => p.Book).Include(p => p.Member).ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (member != null)
            {
                var Loans = await database.Loans.Where(b => b.Member.Email.Equals(member)).ToListAsync();
            }
            int totalItems = loans.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = member;
            ViewBag.CurrentAction = "SearchByMember";
            var item = loans.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", loans);
        }
        public async Task<IActionResult> SearchByLoanDate(DateTime startdate, DateTime enddate, int? page)
        {
            var loans = database.Loans.Include(p => p.Book).Include(p => p.Member).ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (startdate != null || enddate != null)
            {
                var Loans = await database.Loans.Where(d => d.LoanDate.Day >= startdate.Day && d.LoanDate.Day <= enddate.Day).ToListAsync();
            }
            int totalItems = loans.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = startdate;
            ViewBag.Search2 = enddate;
            ViewBag.CurrentAction = "SearchByLoanDate";
            var item = loans.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", loans);
        }
        public async Task<IActionResult> SearchByDueDate(DateTime startdate, DateTime enddate, int? page)
        {
            var loans = database.Loans.Include(p => p.Book).Include(p => p.Member).ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (startdate != null || enddate != null)
            {
                var Loans = await database.Loans.Where(d => d.DueDate.Day >= startdate.Day && d.DueDate.Day <= enddate.Day).ToListAsync();
            }
            int totalItems = loans.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = startdate;
            ViewBag.Search2 = enddate;
            ViewBag.CurrentAction = "SearchByDueDate";
            var item = loans.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", loans);
        }
    }
}
