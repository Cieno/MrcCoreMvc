using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRCDataLibrary._02_Models;
using MRCDataLibrary._03_Data;

namespace MrcCoreMvc.Controllers
{
    [Authorize]
    public class CategoryMasterController : Controller
    {
        private readonly ICategoryMasterData _categoryMasterData;

        public CategoryMasterController(ICategoryMasterData categoryMasterData)
        {
            _categoryMasterData = categoryMasterData;
        }

        public async Task<IActionResult> Index(string? categoryId)
        {
            var categoryList = await _categoryMasterData.GetCategoryList(categoryId);
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryMasterModel category)
        {
            var itemCount = await _categoryMasterData.GetDuplicationItemCount(category.CATEGORY_ID);
            if (itemCount > 0)
            {
                ModelState.AddModelError(string.Empty, "This category code already exists.");
                return View(category);
            }
            await _categoryMasterData.CreateCategory(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string categoryId)
        {
            var category = await _categoryMasterData.GetCategoryList(categoryId);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryMasterModel category)
        {
            await _categoryMasterData.DeleteCategory(category.CATEGORY_ID);
            return RedirectToAction("Index");
        }
    }
}
