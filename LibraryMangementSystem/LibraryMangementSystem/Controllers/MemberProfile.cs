using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberProfile : Controller
    {
        private readonly LibraryContext database;
        public MemberProfile(LibraryContext db)
        {
            database = db;
        }
        public IActionResult Index()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = database.Members.FirstOrDefault(s => s.IdentityUserId == userID);
            return View(member);
        }

        public IActionResult MyLoans(int id)
        {
            var loans = database.Loans.Include(s => s.Member).Include(s => s.Book).ToList();
            ViewBag.MemberID = database.Members.Find(id).MemberID;
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
    }
}
