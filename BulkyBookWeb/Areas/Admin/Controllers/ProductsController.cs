using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BulkyBook.DataAccess;
using BulkyBook.Models.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models.ViewModels;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnv)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = webHostEnv;
        }

        //Get All
        public IActionResult Index()
        {

            IEnumerable<Product> products = _unitOfWork.Products.GetAll();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypesList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            /*
             * Insert new
             */
            if (id == null || id == 0)
            {
                //To populate the retrived selected ListsITems
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypesList;
                return View(productVM);
            }
            /*
            * Else Update
            */
            else
            {

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                _unitOfWork.Products.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            /** Find all categories can be done in many ways as: 
             * _db.categories.single<u=>u.Id=id> Or 
             * _db.categories.SignleOrDefault(id)
             * **/
            var productsList = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == id);
            if (productsList == null)
            {
                return NotFound();
            }
            return View(productsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product obj)
        {
            _unitOfWork.Products.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
