using BlogHub.Data;
using BlogHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Controllers
{
    [Authorize]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {
            List<Category> obj = _db.Categories.ToList();
            return View(obj);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (_db.Categories.Any(o => o.Name == obj.Name))
            {
                ModelState.AddModelError($"Name ", $"{obj.Name} exits already in the Category ");
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Category? DbfromCategory = _db.Categories.Find(Id);
            if (DbfromCategory == null)
            {
                return NotFound();

            }
            return View(DbfromCategory);

        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (_db.Categories.Any(o => o.Name == obj.Name))
            {
                ModelState.AddModelError($"Name ", $"{obj.Name} exits already in the Category ");
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Category? DbfromCategory = _db.Categories.Find(Id);
            if (DbfromCategory == null)
            {
                return NotFound();

            }
            return View(DbfromCategory);

        }

        [HttpPost]
        public IActionResult Delete(Category obj)
        {

            if (ModelState.IsValid)
            {
                _db.Categories.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


    }
}
