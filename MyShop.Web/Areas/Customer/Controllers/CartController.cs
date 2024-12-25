using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Models.ViewModel;
using MyShop.Utilities;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM = new ShoppingCartVM()
            {

                CartsList = _unitOfWork.ShoppingCart.GetAll(i=>i.ApplicationUserId==userId, include:"Product"),
                OrderHeader=new()
            };
            foreach (var item in shoppingCartVM.CartsList)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
			}

            return View(shoppingCartVM);
        }
        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == userId, include: "Product"),
                OrderHeader = new OrderHeader()
            };
            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(i=> i.Id == userId);

            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.Address = shoppingCartVM.OrderHeader.ApplicationUser.Address;

            foreach (var item in shoppingCartVM.CartsList)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary(ShoppingCartVM ShoppingCartVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.CartsList = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == userId, include: "Product");
           
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;

            foreach (var item in ShoppingCartVM.CartsList) 
            {
				ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
			}

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Complete();

            foreach(var item in ShoppingCartVM.CartsList)
            {
                OrderDetails orderDetail = new OrderDetails()
                {
                    ProductId=item.ProductId,
                    OrderHeaderId= ShoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.Complete();
            }
            var domain = "https://localhost:7100/";
            var options = new SessionCreateOptions
            {
              
                LineItems = new List<SessionLineItemOptions>(),
                Mode= "payment",                      
                SuccessUrl = domain +$"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };
            foreach (var item in ShoppingCartVM.CartsList)
            {
				var sessionlineoption = new SessionLineItemOptions
				{
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                       
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionlineoption);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;
          
            _unitOfWork.Complete();

            //_unitOfWork.Complete();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        
		}
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(i => i.Id == id);
            var service= new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateOrderStatus(id, SD.Approved,SD.Approved);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            HttpContext.Session.Clear();
            List<ShoppingCart> carts = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(carts);
            _unitOfWork.Complete();

            return View(id);
        }

        public IActionResult Plus(int cartId) 
        { 
            var cart = _unitOfWork.ShoppingCart.Get(i => i.Id == cartId, include: "Product");
			cart.Count += 1;
			_unitOfWork.Complete();
			return RedirectToAction(nameof(Index));

		}
	      public IActionResult Minus(int cartId)
	      {
            var cart = _unitOfWork.ShoppingCart.Get(i => i.Id == cartId, include: "Product");
		     
            if (cart.Count <=1) 
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                var count = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == cart.ApplicationUserId).ToList().Count()-1;
                HttpContext.Session.SetInt32(SD.SessionKey,count);
            }
            else
            {
				cart.Count -= 1;
			}

	    	_unitOfWork.Complete();
	     	return RedirectToAction(nameof(Index));
  
	      }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(i => i.Id == cartId, include: "Product");
            _unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.Complete();
            var count = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction(nameof(Index));
		}

		}
}
