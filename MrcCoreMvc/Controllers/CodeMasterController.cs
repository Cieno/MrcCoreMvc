using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize]
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

            string createdId = await _codeMasterData.CreateCode(codeMaster);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CodeMasterModel codeMaster)
        {
            await _codeMasterData.DeleteCode(codeMaster);
            return RedirectToAction("Index");
        }
    }
}
