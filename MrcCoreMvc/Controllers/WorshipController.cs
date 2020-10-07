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
    public class WorshipController : Controller
    {
        private readonly IWorshipData _worshipData;
        private readonly ICodeMasterData _codeMasterData;

        public WorshipController(IWorshipData worshipData, ICodeMasterData codeMasterData)
        {
            _worshipData = worshipData;
            _codeMasterData = codeMasterData;
        }
        // GET: WorshipController
        public async Task<IActionResult> Index()
        {
            var worships = await _worshipData.GetWorships();
            var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");
            worships.ForEach(x =>
            {
                x.WORSHIP_NAME = worshipType.Where(n => x.WORSHIP_TYPE == n.CODE_ID).FirstOrDefault()?.CODE_DESCR;
            });
            return View(worships);
        }

        // GET: WorshipController/Details/5
        public async Task<IActionResult> Details(string worshipId)
        {
            var worship = new WorshipModel();
            worship = await _worshipData.GetWorshipById(worshipId);
            var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");
            worshipType.ForEach(x =>
            {
                worship.WorshipTypeSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });
            return View(worship);
        }

        // GET: WorshipController/Create
        public async Task<IActionResult> Create()
        {
            var worship = new WorshipModel();
            var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");
            worshipType.ForEach(x =>
            {
                worship.WorshipTypeSelectList.Add(new SelectListItem { Value = x.CODE_ID, Text = x.CODE_DESCR });
            });
            return View(worship);
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
