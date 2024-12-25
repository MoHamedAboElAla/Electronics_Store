using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Models.ViewModel;
using MyShop.Web.Data;
using MyShop.Web.Models;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
       
        public IActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(include:"Category");
            return Json(new { data = allObj });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVm productVm = new ProductVm()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVm);
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Create(ProductVm productVm,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"Images\Products\" + filename + extension;
                }
                _unitOfWork.Product.Add(productVm.Product);
                _unitOfWork.Complete();
                TempData["message"] = "Product Created Successfully";

                return RedirectToAction("Index");
            }
          
            return View(productVm);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            ProductVm productVm = new ProductVm()
            {
                Product = _unitOfWork.Product.Get(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVm);
           
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Edit(ProductVm productVm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);
                    if(productVm.Product.ImageUrl != null)
                    {
                        var oldImage = Path.Combine(RootPath, productVm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"Images\Products\" + filename + extension;
                }
                _unitOfWork.Product.Update(productVm.Product);
                _unitOfWork.Complete();
                TempData["message"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productVm);
        }
    /*    [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productDb = _unitOfWork.Product.Get(x => x.Id == id);

            return View(productDb);
        }*/
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.Get(x => x.Id == id);
            if (product == null)
            {
               return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Product.Remove(product);
            var oldImage = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            _unitOfWork.Complete();
           // TempData["message"] = "Product Deleted Successfully";
            return Json(new { success = true, message = "Data has been  deleted" });
        }
    }
}
