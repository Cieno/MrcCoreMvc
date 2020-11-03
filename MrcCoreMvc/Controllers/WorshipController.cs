using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    public class WorshipController : Controller
    {
        private readonly IWorshipData _worshipData;
        private readonly ICodeMasterData _codeMasterData;

        public WorshipController(IWorshipData worshipData, ICodeMasterData codeMasterData)
        {
            _worshipData = worshipData;
            _codeMasterData = codeMasterData;
        }

        public IActionResult Index()
        {
            return View();
        }
        // GET: WorshipController
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var worships = await _worshipData.GetWorships();
            var worshipType = await _codeMasterData.GetCodeList("WORSHIP_TYPE");
            worships.ForEach(x =>
            {
                x.WorshipName = worshipType.Where(n => x.WorshipType == n.CODE_ID).FirstOrDefault()?.CODE_DESCR;
                x.SimpleDate = x.WorshipDate.ToString("yyyy-MM-dd");
                x.SimpleTime = x.WorshipDate.ToString("HH:mm");
            });
            //return View(worships);
            return Json(new { data = worships.ToList() });
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


        //public async Task<IActionResult> Delete(string worshipId)
        //{
        //    var worship = await _worshipData.GetWorshipById(worshipId);
        //    return View(worship);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(WorshipModel worship)
        //{
        //    await _worshipData.DeleteWorship(worship.WorshipId);
        //    return RedirectToAction("Index");
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(string worshipId)
        {
            await _worshipData.DeleteWorship(worshipId);
            //return RedirectToAction("Index");
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
