using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
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
            shoppingCartVM = new ShoppingCartVM
            {
                shoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                orderHeader = new ()

            };
            // We cannot get the order total directly becasue of the Specal prices we have
            /*orderTotal -> Sum(Price*count) of All items in shoppingCart
             * Example:
             * Book1 
             *          price for one book to 50 Books -> 30$
             *          price for 51 to 100 books -> 25$
             *          price for more than 100 books -> 20$
             * Book2
             *          price for one book to 50 Books -> 50$
             *          price for 51 to 100 books -> 45$
             *          price for more than 100 books -> 40$
             *          
             * if the user added 3 books of Book1 -> Count = 3
             * price is 3*30$ = 90$
             * If the user added 55 books of Book2 -> Count 55
             * price is 55*45$ = 2475$
             * 
             * OrderTotal is 90$ + 2475$ = 2565$
             */

            foreach(var item in shoppingCartVM.shoppingCartList)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                shoppingCartVM.orderHeader.OrderTotal += (item.Price * item.Count);
            }


            return View(shoppingCartVM);
        }

        public IActionResult Plus (int cartItemId)
        {
            // 1. get the item from DB (ShoppingCart table)
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id == cartItemId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minuse(int cartItemId)
        {
            // 1. get the item from DB (ShoppingCart table)
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartItemId);
            if(cartFromDb.Count <= 1)
            {
                //Remove
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                cartFromDb.Count -= 1;
            }
            
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int cartItemId)
        {
            // 1. get the item from DB (ShoppingCart table)
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartItemId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            return View();
        }

        /* in our application 
         * when user order one to 50 books -> price
         * when user order 51 - 100 books -> price 50
         * when user order more than 100 books -> price100
         */

        /*
         * if(count<=50) -> return price
         * elseif(count<=100)-> return price50
         * elseif(count>100)-> return proce100
         */
        public double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else 
            {
                if (shoppingCart.Count <= 100) {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
                    
            }
        }
    }
}
