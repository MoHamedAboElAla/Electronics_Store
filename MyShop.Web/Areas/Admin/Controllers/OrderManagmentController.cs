using Microsoft.AspNetCore.Mvc;
using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Models.ViewModel;
using MyShop.Utilities;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
		public OrderVm OrderVm { get; set; }
		public OrderManagmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData() 
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(include: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }
      
        public IActionResult Details(int orderid)
        {

            OrderVm orderVm = new OrderVm()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(i => i.Id == orderid, include: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(i => i.OrderHeaderId == orderid, include: "Product")
            };

            return View(orderVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

		public IActionResult UpdateOrderDetails()
		{
            var orderfromdb = _unitOfWork.OrderHeader.Get(i=>i.Id == OrderVm.OrderHeader.Id);
			orderfromdb.Name = OrderVm.OrderHeader.Name;
			orderfromdb.PhoneNumber = OrderVm.OrderHeader.PhoneNumber;
			orderfromdb.Address = OrderVm.OrderHeader.Address;
			orderfromdb.City = OrderVm.OrderHeader.City;

            if (OrderVm.OrderHeader.Carrier != null) 
            { 
             orderfromdb.Carrier = OrderVm.OrderHeader.Carrier;
			}
            if(OrderVm.OrderHeader.TrackingNumber != null) 
            { 
             orderfromdb.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;
			}
            _unitOfWork.OrderHeader.Update(orderfromdb);
            _unitOfWork.Complete();
            TempData["message"] = "Order Details Updated Successfully";
			return RedirectToAction("Details", "OrderManagment", new { orderid = orderfromdb.Id });

		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProccess()
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(OrderVm.OrderHeader.Id, SD.Processing,null);
            _unitOfWork.Complete();
            TempData["message"] = "Order Details Processing Successfully";
            return RedirectToAction("Details", "OrderManagment", new { orderid = OrderVm.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderfromdb = _unitOfWork.OrderHeader.Get(i => i.Id == OrderVm.OrderHeader.Id);
			orderfromdb.Carrier = OrderVm.OrderHeader.Carrier;
            orderfromdb.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;
            orderfromdb.OrderStatus = SD.Shipped;
			orderfromdb.ShippingDate = DateTime.Now;
            _unitOfWork.OrderHeader.Update(orderfromdb);
			_unitOfWork.Complete();
			TempData["message"] = "Order Details Shipped Successfully";
			return RedirectToAction("Details", "OrderManagment", new { orderid = OrderVm.OrderHeader.Id });
		}

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
        {
            var orderfromdb = _unitOfWork.OrderHeader.Get(i => i.Id == OrderVm.OrderHeader.Id);
            if(orderfromdb.PaymentStatus == SD.Approved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfromdb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateOrderStatus(orderfromdb.Id,SD.Cancelled,SD.Refund);
			}
            else
            {
				_unitOfWork.OrderHeader.UpdateOrderStatus(orderfromdb.Id, SD.Cancelled, SD.Cancelled);

			}
             _unitOfWork.Complete();
		     TempData["message"] = "Order Cancelled Successfully";
			 return RedirectToAction(nameof(Index));
        }
	}
}
