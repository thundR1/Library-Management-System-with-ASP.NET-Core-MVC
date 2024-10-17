using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMangementSystem.Controllers
{
    [Authorize(Roles ="Admin,Librarian")]
    public class CategoriesController : Controller
    {
        private readonly LibraryContext database;
        public CategoriesController(LibraryContext db)
        {
            database = db;
        }

        public ActionResult Index()
        {
            var Categories = database.Categories.ToList();
            return View(Categories);
        }


        public ActionResult Details(int id)
        {
            var category = database.Categories.Find(id);
            if (category != null) return View(category);
            else return NotFound();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            if (category != null)
            {
                
                database.Categories.Add(category);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = database.Categories.Find(id);
            if (category != null) return View(category);
            else return NotFound();
        }


        [HttpPost]

        public async Task<ActionResult> Edit(Category newCat)
        {
            if (newCat != null)
            {
                var currentCat = database.Categories.Find(newCat.CategoryID);
                if (currentCat != null)
                {

                    currentCat.Name = newCat.Name;
                    
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }


        public ActionResult Delete(int id)
        {
            var category = database.Categories.Find(id);
            return View(category);
        }


        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = database.Categories.Find(id);
            if (category != null)
            {
                database.Categories.Remove(category);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
