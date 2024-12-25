using Microsoft.AspNetCore.Mvc;
using MyShop.Models.Repository;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;
		public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IViewComponentResult> InvokeAsync ()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if(claims != null)
			{
				if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
				{
					return View(HttpContext.Session.GetInt32(SD.SessionKey));
				}
				else
				{ 
				 HttpContext.Session.SetInt32(SD.SessionKey,
		      _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == claims.Value).ToList().Count());
					return View(HttpContext.Session.GetInt32(SD.SessionKey));
				}
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}

		}

	}
}
