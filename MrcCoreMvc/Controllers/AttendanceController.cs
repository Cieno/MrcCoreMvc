using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceData _attendanceData;

        public AttendanceController(IAttendanceData attendanceData)
        {
            _attendanceData = attendanceData;
        }
        // GET: AttendanceController
        public async Task <IActionResult> Index()
        {
            return View();
        }

        // GET: AttendanceController/Details/5
        public async Task<IActionResult> AttendanceList(string worshipId)
        {
            var attendance = await _attendanceData.GetAttendanceByWorship(worshipId);
            return View(attendance);
        }

        // GET: AttendanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
