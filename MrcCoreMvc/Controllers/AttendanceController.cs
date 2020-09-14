﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrcCoreMvc.Models;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceData _attendanceData;
        private readonly IMemberData _memberData;
        private readonly IWorshipData _worshipData;

        public AttendanceController(IAttendanceData attendanceData, IMemberData memberData, IWorshipData worshipData)
        {
            _attendanceData = attendanceData;
            _memberData = memberData;
            _worshipData = worshipData;
        }
        // GET: AttendanceController
        public async Task <IActionResult> Index()
        {
            return View();
        }

        // GET: AttendanceController/Details/5
        public async Task<IActionResult> AttendanceList(string worshipId)
        {
            var attendanceList = new AttendanceListModel();
            var attendance = await _attendanceData.GetAttendanceByWorship(worshipId);
            var worship = await _worshipData.GetWorshipById(worshipId);
            attendanceList.AttendanceList = attendance;
            attendanceList.Worship = worship;

            return View(attendanceList);
        }

        // GET: AttendanceController/Create
        public async Task<IActionResult> Create(string worshipId)
        {
            var members = await _memberData.GetMembers();
            var attendance = new AttendanceModel();
            attendance.WORSHIP_ID = worshipId;
            members.ForEach(x =>
            {
                attendance.MEMBER_LIST
                .Add(item: new SelectListItem { Value = x.MEMBER_ID, Text = x.FIRST_NAME + " " + x.LAST_NAME });
            });
            return View(attendance);
        }

        // POST: AttendanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceModel attendance)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }
            string id = await _attendanceData.CreateAttendance(attendance);
            return RedirectToAction("AttendanceList", new { worshipId = id });
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
