using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberData _memberData;

        public MemberController(IMemberData memberData)
        {
            _memberData = memberData;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberData.GetMembers();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberModel member)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }

            string id = await _memberData.CreateMember(member);
            return RedirectToAction("Detail", new { memberId = id });
        }

        public async Task<IActionResult> Detail(string memberId)
        {
            var member = new MemberModel();
            member = await _memberData.GetMemberById(memberId);

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Update(MemberModel member)
        {
            string memberId = await _memberData.UpdateMember(member);
            return RedirectToAction("Detail", new { memberId });
        }

        public async Task<IActionResult> Delete(string memberId)
        {
            var member = await _memberData.GetMemberById(memberId);
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MemberModel member)
        {
            await _memberData.DeleteMember(member.MEMBER_ID);
            return RedirectToAction("Index");
        }

    }
}
