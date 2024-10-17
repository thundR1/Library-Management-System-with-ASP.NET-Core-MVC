using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles ="Admin, Librarian")]
    public class AuthorsController : Controller
    {
        private readonly LibraryContext database;
        public AuthorsController(LibraryContext db)
        {
            database = db;
        }
        
        public ActionResult Index()
        {
            var authors = database.Authors.ToList();
            return View(authors);
        }

        
        public ActionResult Details(int id)
        {
            var author = database.Authors.Find(id);
            if (author != null) return View(author);
            else return NotFound();
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> Create(Author author)
        {
            if (author != null)
            {
                author.FullName = author.FirstName + " " + author.LastName;
                database.Authors.Add(author);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var author = database.Authors.Find(id);
            if (author != null) return View(author);
            else return NotFound();
        }

        
        [HttpPost]
        
        public async Task<ActionResult> Edit(Author newAuthor)
        {
            if (newAuthor != null)
            {
                var currentAuthor = database.Authors.Find(newAuthor.AuthorID);
                if (currentAuthor != null)
                {
                   
                    currentAuthor.FirstName = newAuthor.FirstName;
                    currentAuthor.LastName = newAuthor.LastName;
                    currentAuthor.FullName = currentAuthor.FirstName + " " + currentAuthor.LastName;
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");   
                }
            }
            return NotFound();
        }

        
        public ActionResult Delete(int id)
        {
            var author = database.Authors.Find(id);
            return View(author);
        }

        
        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = database.Authors.Find(id);
            if (author != null)
            {
                database.Authors.Remove(author);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
