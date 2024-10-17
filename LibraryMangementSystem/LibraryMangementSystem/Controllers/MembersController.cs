using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class MembersController : Controller
    {
        private readonly LibraryContext database;
        public MembersController(LibraryContext db)
        {
            database = db;
        }
        public IActionResult Index(int? page)
        {
            var members = database.Members.ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            int totalItems = members.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            var item = members.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View(item);
        }

        public IActionResult MemberLoans(int id)
        {
            var loans = database.Loans.Where(s=>s.MemberID == id).Include(s => s.Book).Include(s => s.Member).ToList();
            //ViewBag.MemberID = id;

            var fullName = database.Members.Find(id).FirstName + " " + database.Members.Find(id).LastName;
            if (fullName != " ") ViewBag.MemberName = fullName;
            else { ViewBag.MemberName = database.Members.Find(id).Email; }
            return View(loans);
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = database.Members.Find(id);
            if (member != null)
            {
                return View(member);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Member newMember)
        {
            if (newMember != null)
            {
                var currentMember = database.Members.Find(newMember.MemberID);
                if (currentMember != null)
                {

                    currentMember.FirstName = newMember.FirstName;
                    currentMember.LastName = newMember.LastName;
                    currentMember.Address = newMember.Address;
                    currentMember.Phone = newMember.Phone;
                    await database.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return NotFound();
        }

        public IActionResult Delete(int id)
        {
            var member = database.Members.Find(id);
            if (member != null)
            {
                return View(member);
            }
            return NotFound();
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = database.Members.Find(id);
            if (member != null)
            {
                database.Members.Remove(member);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
        public async Task<IActionResult> SearchByEmail(string email, int? page)
        {
            var members = database.Members.ToList();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            if (email != null)
            {
                members = await database.Members.Where(b => b.Email.Contains(email)).ToListAsync();
            }
            int totalItems = members.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.Search = email;
            ViewBag.CurrentAction = "SearchByEmail";
            var item = members.Skip((page - 1 ?? 0) * PageSize).Take(PageSize);
            return View("index", item);
        }
    }


}
