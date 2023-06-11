using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        // Return a View with dataTable data
        public IActionResult Index()
        {
            return View();
        }

        //Return Form View (Insert new if Id = 0 | update if the company exist)
        public IActionResult Upsert(int? id)
        {
            Company companyObj = new();

            if (id == null || id == 0)
            {
                return View(companyObj);
            }
            else { 
                companyObj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
                return View(companyObj);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company companyObj)
        {

            if (ModelState.IsValid)
            {

                if (companyObj.Id == 0)
                {

                    _unitOfWork.Company.Add(companyObj);

                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Comapnt created successfully";
                return RedirectToAction("Index");

            }
            return View(companyObj);
        }
        // Get all companies using DataTable
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companiesList = _unitOfWork.Company.GetAll();
            return Json(new { data = companiesList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id ==id);
            if(obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
              
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company Deleted Successfully!" });
            
        }
        #endregion
    }
}
