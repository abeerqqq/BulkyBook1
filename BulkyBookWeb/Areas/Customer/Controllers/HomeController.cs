using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Products.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }
        /*
         * ShoppingCart This is a View Model 
         *  ViewModels are used to shape multiple entities from one or more models into a single object.
         */
   
        public IActionResult Details(int productId) {

            ShoppingCart cartObj = new()
            {
                ProductId = productId,
                Product = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType"),
                Count = 1,
            };
           return View(cartObj);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartObj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartObj.ApplicationUserId=userId;

            ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCartObj.ProductId);

            if(shoppingCartFromDb != null)
            {
                shoppingCartFromDb.Count += 1;
                _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCartObj);
            }
            TempData["success"] = "Cart Updated Successfully!";
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }

}         
            
    
