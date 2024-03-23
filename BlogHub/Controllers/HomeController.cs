using BlogHub.Data;
using BlogHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace BlogHub.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index(int PageNumber = 1)
        {
            const int pageSize = 6;

            IQueryable<Article> articlesQuery = _db.Articles.Include(b => b.Category);

            if (articlesQuery == null)
            {
                return NotFound();
            }

            int count = await articlesQuery.CountAsync();

            var paginatedList = await PaginatedList<Article>.CreateAsync(articlesQuery, PageNumber, pageSize);

            return View(paginatedList);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleWithCategories = _db.Articles
                .Include(article => article.Category)
                .FirstOrDefault(article => article.Id == id);

            if (articleWithCategories == null)
            {
                return NotFound();
            }

            return View(articleWithCategories);
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture) || string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
