using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UserRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserData _userData;

        public UserRoleController(RoleManager<IdentityRole> roleManager, IUserData userData)
        {
            _roleManager = roleManager;
            _userData = userData;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles;
            return Json(new { data = roles.ToList() });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(IdentityRole role)
        {
            var thisRole = await _roleManager.FindByIdAsync(role.Id);
            thisRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(thisRole);

            if (result.Succeeded)
            {
                return RedirectToAction("Details", new { name = role.Name });
            }
            else
            {
                return View();
            }
        }

        public IActionResult Create()
        {
            var newRole = new IdentityRole();
            return View(newRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role.Name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Delete successful" });
            }
            else
            {
                return View();
            }
        }
    }
}
