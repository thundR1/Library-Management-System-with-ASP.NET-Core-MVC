using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace LibraryMangementSystem.Controllers
{
    public class PublishersController : Controller
    {
        private readonly LibraryContext database;
        public PublishersController(LibraryContext db)
        {
            database = db;
        }
        public ActionResult Index()
        {
            var publishers = database.Publishers.ToList();
            return View(publishers);
        }


        public ActionResult Details(int id)
        {
            var publisher = database.Publishers.Find(id);
            if (publisher != null) return View(publisher);
            else return NotFound();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(LibraryMangementSystem.Models.Publisher publisher)
        {
            if (publisher != null)
            {

                database.Publishers.Add(publisher);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var publisher = database.Publishers.Find(id);
            if (publisher != null) return View(publisher);
            else return NotFound();
        }


        [HttpPost]

        public async Task<ActionResult> Edit(LibraryMangementSystem.Models.Publisher newPub)
        {
            if (newPub != null)
            {
                var currentPub = database.Publishers.Find(newPub.PublisherID);
                if (currentPub != null)
                {

                    currentPub.Name = newPub.Name;
                    currentPub.Address = newPub.Address;

                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }


        public ActionResult Delete(int id)
        {
            var publisher = database.Publishers.Find(id);
            return View(publisher);
        }


        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = database.Publishers.Find(id);
            if (publisher != null)
            {
                database.Publishers.Remove(publisher);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
