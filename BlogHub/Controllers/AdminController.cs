using BlogHub.Models;
using BlogHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {
            List<AppUser> admins = _userManager.GetUsersInRoleAsync("Admin").Result.ToList();

            return View(admins);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Admin adminModel)
        {
            if (ModelState.IsValid)
            {
                // Vérifier et créer le rôle "Admin" si nécessaire
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var user = new AppUser
                {
                    Email = adminModel.Email,
                    UserName = adminModel.Email,
                    Name = adminModel.Name,

                };

                var result = await _userManager.CreateAsync(user, adminModel.Password!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    // Rediriger vers la liste des administrateurs après l'ajout
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Revenir à la vue AddAdmin si le modèle n'est pas valide
            return View(adminModel);
        }
    }
}
