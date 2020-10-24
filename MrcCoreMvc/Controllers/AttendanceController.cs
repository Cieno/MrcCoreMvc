using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrcCoreMvc.Areas.Identity.Data;
using MrcCoreMvc.Models;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize]    
    public class AttendanceController : Controller
    {
        private readonly IAttendanceData _attendanceData;
        private readonly IMemberData _memberData;
        private readonly IWorshipData _worshipData;
        private readonly ICodeMasterData _codeMasterData;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(IAttendanceData attendanceData,
                                    IMemberData memberData,
                                    IWorshipData worshipData,
                                    ICodeMasterData codeMasterData,
                                    UserManager<ApplicationUser> userManager)
        {
            _attendanceData = attendanceData;
            _memberData = memberData;
            _worshipData = worshipData;
            _codeMasterData = codeMasterData;
            _userManager = userManager;
        }
        // GET: AttendanceController
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AttendanceList(string worshipId)//, string? message )
        {
            var worship = await _worshipData.GetWorshipById(worshipId);
            var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");
            worship.WorshipName = worshipType.Where(n => worship.WorshipType == n.CODE_ID).FirstOrDefault()?.CODE_DESCR;
            //worship.Message = message != null ? message : null;

            return View(worship);
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAttendanceList(string worshipId)
        {
            var attendance = await _attendanceData.GetAttendanceByWorship(worshipId);
            return Json(new { data = attendance.ToList() }) ;
        }

        #endregion


        //public async Task<IActionResult> AttendanceList(string worshipId)
        //{
        //    var attendanceList = new AttendanceListModel();
        //    var attendance = await _attendanceData.GetAttendanceByWorship(worshipId);
        //    var worship = await _worshipData.GetWorshipById(worshipId);
        //    var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");

        //    worship.WorshipName = worshipType.Where(n => worship.WorshipType == n.CODE_ID).FirstOrDefault()?.CODE_DESCR;

        //    attendanceList.AttendanceList = attendance;
        //    attendanceList.Worship = worship;

        //    return View(attendanceList);
        //}

        // GET: AttendanceController/Create
        //public async Task<IActionResult> Create(string worshipId)
        //{
        //    var members = await _memberData.GetMembersToAdd(worshipId);
        //    var attendance = new AttendanceModel();
        //    attendance.WorshipId = worshipId;
        //    members.ForEach(x =>
        //    {
        //        attendance.MemberList
        //        .Add(item: new SelectListItem { Value = x.MemberId, Text = x.LastName + " " + x.FirstName});
        //    });
        //    return View(attendance);
        //}

        public async Task<IActionResult> Create(string worshipId)
        {
            var userInfo = await _userManager.GetUserAsync(User);
            var attendanceList = await _attendanceData.GetAttendanceByWorship(worshipId);

            bool isRegistered = false;
            foreach (var member in attendanceList)
            {
                if (member.MemberId.Equals(userInfo.Id))
                {
                    isRegistered = true;
                    break;
                }
            }

            if (isRegistered)
            {
                //ModelState.AddModelError(string.Empty, "You have participated this service already.");
                return RedirectToAction("AttendanceList", new { worshipId });//, message = "You have participated in this service already." });
            }
            else
            {
                var attendance = new AttendanceModel();
                attendance.MemberName = userInfo.FullName;
                attendance.MemberId = userInfo.Id;
                attendance.WorshipId = worshipId;

                return View(attendance);
            }
            
        }

        // POST: AttendanceController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[]
        //[Validation]
        //[CustomValidation(typeof(MemoDocument), “ValidateCategoryAndPriority”)]
        public async Task<IActionResult> Create(AttendanceModel attendance)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }
            string id = await _attendanceData.CreateAttendance(attendance);
            return RedirectToAction("AttendanceList", new { worshipId = id });
        }


        //// GET: AttendanceController/Delete/5
        //public async Task<IActionResult> Delete(string worshipId, string memberId)
        //{
        //    var attendance = await _attendanceData.GetAttendanceByWorshipMember(worshipId, memberId);
        //    return View(attendance);
        //}

        //// POST: AttendanceController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(AttendanceModel attendance)
        //{
        //    await _attendanceData.DeleteAttendance(attendance.WorshipId, attendance.MemberId);
        //    return RedirectToAction("AttendanceList", new { worshipId = attendance.WorshipId });
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(string worshipId, string memberId)
        {
            await _attendanceData.DeleteAttendance(worshipId, memberId);
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
