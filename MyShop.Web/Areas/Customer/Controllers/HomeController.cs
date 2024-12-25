using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Utilities;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int ? page)
        {
            var PageNumber = page ?? 1;
            var PageSize = 4;
            var products = _unitOfWork.Product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {

                Product = _unitOfWork.Product.Get(i => i.Id == id, include: "Category"),
               ProductId=id,
                Count = 1
            };
          
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity= (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
           
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
                u=>u.ApplicationUserId== userId && u.ProductId == shoppingCart.ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
               
                TempData["message"] = "Cart Updated Successfully";
                _unitOfWork.Complete();
            }
            else
            {
                shoppingCart.Id = 0;
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey,
                  _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId).ToList().Count()
                   );
            }
            return RedirectToAction("Index");
        }
    }
}
