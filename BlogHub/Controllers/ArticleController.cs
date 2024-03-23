using BlogHub.Data;
using BlogHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {

        public readonly AppDbContext _db;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ArticleController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _webHostEnvironment = environment;
        }
        public IActionResult Index()
        {
            List<Article> objProduct = _db.Articles.Include(p => p.Category).ToList();
            return View(objProduct);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            var article = new Article
            {
                Posteddate = DateTime.Now,
                Author = User.Identity.Name

            };
            return View(article);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article resource)
        {

            if (resource.ImageFile != null)
            {
                var folder = "images/";
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + resource.ImageFile.FileName;
                var serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);


                using (var fileStream = new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create))
                {
                    await resource.ImageFile.CopyToAsync(fileStream);
                }

                resource.ImageName = Path.Combine(folder, uniqueFileName);
            }

            _db.Articles.Add(resource);
            _db.SaveChanges();
            TempData["Success"] = "Book Create successfully";
            return RedirectToAction("Index");


            return View(resource);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");



            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Article? bookFromDb = _db.Articles.Find(id);
            bookFromDb.Posteddate = DateTime.Now;



            if (bookFromDb == null)
            {
                return NotFound();
            }

            return View(bookFromDb);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(Article resource)
        {

            if (resource.ImageFile != null)
            {
                var folder = "images/";
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + resource.ImageFile.FileName;
                var serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);


                using (var fileStream = new FileStream(Path.Combine(serverFolder, uniqueFileName), FileMode.Create))
                {
                    await resource.ImageFile.CopyToAsync(fileStream);
                }

                resource.ImageName = Path.Combine(folder, uniqueFileName);
            }

            _db.Articles.Update(resource);
            _db.SaveChanges();
            TempData["Success"] = "Book Update successfully";
            return RedirectToAction("Index");


            return View(resource);

        }

        public IActionResult Delete(int id)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");



            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Article? bookFromDb = _db.Articles.Find(id);




            if (bookFromDb == null)
            {
                return NotFound();
            }

            return View(bookFromDb);


        }
        [HttpPost]
        public IActionResult Delete(Article resource)
        {



            _db.Articles.Remove(resource);
            _db.SaveChanges();
            TempData["Success"] = "Book Remove successfully";
            return RedirectToAction("Index");


            return View(resource);

        }
    }
}
