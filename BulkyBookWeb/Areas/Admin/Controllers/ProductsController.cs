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
            return View();
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
                
                return View(productVM);
            }
            /*
            * Else Update
            */
            else
            {
                productVM.Product = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
            
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

                   /*If we are updating a product we have to delete the old image and add a new one in the images/products folder
                    * 1. Check if there's an imageURL in the DB
                    * 2. Get the Image Path
                    */
                    if (obj.Product.ImageUrl != null) {
                        
                        var oldImagePath = Path.Combine(wwwRootPath,obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) { 
                            System.IO.File.Delete(oldImagePath);
                        }


                    }
                    
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Products.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Products.Update(obj.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully!!";
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
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var productsList = _unitOfWork.Products.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productsList });

        }
        #endregion
    }
}
