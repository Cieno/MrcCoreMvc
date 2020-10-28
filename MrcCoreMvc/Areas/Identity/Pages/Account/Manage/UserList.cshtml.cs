using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MrcCoreMvc.Areas.Identity.Data;

namespace MrcCoreMvc.Areas.Identity.Pages.Account.Manage
{
    public class UserListModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserListModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userManager.GetUserAsync(User);
            return Page();
        }
    }
}
