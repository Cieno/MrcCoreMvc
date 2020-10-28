﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MrcCoreMvc.Areas.Identity.Data;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserData _userData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public UserController(IUserData userData,
                              UserManager<ApplicationUser> userManager,
                              ILogger<DeletePersonalDataModel> logger)
        {
            _userData = userData;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userData.GetUsers();
            return Json(new { data = users.ToList() });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userData.GetUser(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(UserModel user)
        {
            string id = await _userData.UpdateUser(user);
            return RedirectToAction("Details", new { id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            //var user = await _userData.GetUser(id);
            var result = await _userData.DeleteUser(id);
            if (result <= 0)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{id}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", id);

            //return Redirect("~/");
            return Json(new { success = true, message = "Delete successful" });

        }
    }
}
