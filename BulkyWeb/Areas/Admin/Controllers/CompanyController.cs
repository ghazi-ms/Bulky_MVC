using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Uitlity;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Company> CompanysList =_unitOfWork.Company.GetAll().ToList();
           
            return View(CompanysList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id <= 0)
            {
			
				return View(new Company());
            }
            else
            {
                Company obj = _unitOfWork.Company.Get(u => u.Id == id);
				return View(obj);
			}
           
        }

        [HttpPost]
		public IActionResult Upsert(Company company)
		{
           
            if (ModelState.IsValid)
            {
            
                if(company.Id==0)
                {
                _unitOfWork.Company.Add(company);
                TempData["success"] = "Company Added Successfully";
                }
                else
                {
					_unitOfWork.Company.Update(company);
					TempData["success"] = "Company Updated Successfully";
				}
				_unitOfWork.Save();
                return RedirectToAction("Index");
            }else
			{
				return View(company);
			}

		}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully";
            return RedirectToAction("Index");


        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanysList = _unitOfWork.Company.GetAll().ToList();

            return Json(new {data=CompanysList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { succss = false, message = "Error While Deleting" });
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { succss = true, message = "Deleted Succesfully" });
        }

        #endregion
    }
}
