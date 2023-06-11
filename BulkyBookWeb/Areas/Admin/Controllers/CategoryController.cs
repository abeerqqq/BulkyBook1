using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
   
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        /*
         * DI
         * Here we created an object from the previously create class ApplicationDbContext
         */
        private readonly IUnitOfWork _unitOfWork;

        /*
         * Create a constroctor to assign an object to the above
         * we don't need to call the constructor as new ApplicationDbContext() and pass our values to connect
         * Because we already created our connection in Program.cs
         * And we just have to call the tables directly in two steps"
         *  1. Create an object of the class 
         *  2. Assign that object in the constructor
         */
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //var objCategory = _db.categories.ToList(); -> Return the list
            /*IEnumerable
             * supports a simple iteration over a non-generic collection.
             */
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            //Check for equal values ! (this is server side )
            if (obj.Name.Trim() == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom", "Name And Display Order Cannot be Matching");
            }
            // Add validation 
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Created Successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            /** Find all categories can be done in many ways as: 
             * _db.categories.single<u=>u.Id=id> Or 
             * _db.categories.SignleOrDefault(id)**/
            var categoryList = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryList == null)
            {
                return NotFound();
            }
            return View(categoryList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            //Check for equal values ! (this is server side )
            if (obj.Name.Trim() == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom", "Name And Display Order Cannot be Matching");
            }
            // Add validation 
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Updated Successfully!";
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
             * _db.categories.SignleOrDefault(id)**/
            var categoryList = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryList == null)
            {
                return NotFound();
            }
            return View(categoryList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
