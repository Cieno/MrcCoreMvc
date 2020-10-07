using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberData _memberData;
        private readonly ICodeMasterData _codeMasterData;

        public MemberController(IMemberData memberData, ICodeMasterData codeMasterData)
        {
            _memberData = memberData;
            _codeMasterData = codeMasterData;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberData.GetMembers();
            var groupType = await _codeMasterData.GetCodeList("GROUP_CD");
            var teamType = await _codeMasterData.GetCodeList("TEAM_CD");
            members.ForEach(x =>
            {
                x.GROUP_NAME = groupType.Where(g => x.GROUP_CD == g.CODE_ID).FirstOrDefault()?.CODE_DESCR;
                x.TEAM_NAME = teamType.Where(t => x.TEAM_CD == t.CODE_ID).FirstOrDefault()?.CODE_DESCR;
            });
            return View(members);
        }

        public async Task<IActionResult> Create()
        {
            var member = new MemberModel();
            var groupType = await _codeMasterData.GetCodeList("GROUP_CD");
            groupType.ForEach(x =>
            {
                member.GroupSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });
            var teamType = await _codeMasterData.GetCodeList("TEAM_CD");
            teamType.ForEach(x =>
            {
                member.TeamSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });
            return View(member);
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
            var member = await _memberData.GetMemberById(memberId);
            var groupType = await _codeMasterData.GetCodeList("GROUP_CD");
            groupType.ForEach(x =>
            {
                member.GroupSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });
            var teamType = await _codeMasterData.GetCodeList("TEAM_CD");            
            teamType.ForEach(x =>
            {
                member.TeamSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });

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
