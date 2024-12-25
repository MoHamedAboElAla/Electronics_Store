using Microsoft.AspNetCore.Mvc;
using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Web.Data;
using MyShop.Web.Models;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);

        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["message"] = "Category Created Successfully";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryDb = _unitOfWork.Category.Get(x => x.Id == id);

            return View(categoryDb);
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["message"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryDb = _unitOfWork.Category.Get(x => x.Id == id);

            return View(categoryDb);
        }
        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _unitOfWork.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            TempData["message"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
