using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    public class WorshipController : Controller
    {
        private readonly IWorshipData _worshipData;

        public WorshipController(IWorshipData worshipData)
        {
            _worshipData = worshipData;
        }
        // GET: WorshipController
        public async Task<IActionResult> Index()
        {
            var worships = await _worshipData.GetWorships();
            return View(worships);
        }

        // GET: WorshipController/Details/5
        public async Task<IActionResult> Details(string worshipId)
        {
            var worship = new WorshipModel();
            worship = await _worshipData.GetWorshipById(worshipId);
            return View(worship);
        }

        // GET: WorshipController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorshipController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorshipModel worship)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }
            string id = await _worshipData.CreateWorship(worship);
            return RedirectToAction("Details", new { worshipId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(WorshipModel worship)
        {
            string worshipId = await _worshipData.UpdateWorship(worship);
            return RedirectToAction("Details", new { worshipId });
        }

        // GET: WorshipController/Delete/5
        public async Task<IActionResult> Delete(string worshipId)
        {
            var worship = await _worshipData.GetWorshipById(worshipId);
            return View(worship);
        }

        // POST: WorshipController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(WorshipModel worship)
        {
            await _worshipData.DeleteWorship(worship.WORSHIP_ID);
            return RedirectToAction("Index");
        }
    }
}
