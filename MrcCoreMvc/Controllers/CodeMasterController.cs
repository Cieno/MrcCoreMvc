using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CodeMasterController : Controller
    {
        private readonly ICodeMasterData _codeMasterData;
        private readonly ICategoryMasterData _categoryMasterData;

        public CodeMasterController(ICodeMasterData codeMasterData, ICategoryMasterData categoryMasterData)
        {
            _codeMasterData = codeMasterData;
            _categoryMasterData = categoryMasterData;
        }



        public async Task<IActionResult> Index(string categoryId)
        {
            var codeList = await _codeMasterData.GetCodeList(categoryId);
            return View(codeList);
        }


        public async Task<IActionResult> Create()
        {
            var category = new CodeMasterModel();
            var categoryList = await _categoryMasterData.GetCategoryList(null);
            categoryList.ForEach(x =>
            {
                category.CategoryList.Add(item: new SelectListItem { Value = x.CATEGORY_ID, Text = x.CATEGORY_DESCR });
            });
            
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CodeMasterModel codeMaster)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }

            string codeId = await _codeMasterData.CreateCode(codeMaster);
            return RedirectToAction("Details", new { codeId, categoryId = codeMaster.CATEGORY_ID });
        }


        public async Task<IActionResult> Details(string codeId, string categoryId)
        {
            var code = await _codeMasterData.GetCodeInfo(codeId, categoryId);
            return View(code);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(CodeMasterModel codeMaster)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var codeId = await _codeMasterData.UpdateCode(codeMaster);
            return RedirectToAction("Details", new { codeId, categoryId = codeMaster.CATEGORY_ID });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string codeId, string categoryId)
        {
            await _codeMasterData.DeleteCode(codeId, categoryId);
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
