using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyBook.Models.ViewModel;
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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> ProductsList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
           
            return View(ProductsList);
        }

       /* public IActionResult Upsert()
        {
            *//*IEnumerable<SelectListItem> Categories = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                        ViewBag.Categories= Categories;
            ViewData["Categories"]= Categories*//*

            ProductVM productVM = new ProductVM()
            {
                Categeories = _unitOfWork.Category.GetAll().Select(
				u => new SelectListItem()
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
                Product = new Product()
            };
            return View(productVM);
        }*/

        /*[HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (productVM.Product.Title == productVM.Product.Description)
            {
                ModelState.AddModelError("description", "Product Title And Description Cant Be The Same");
                productVM.Categeories = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
				return View(productVM);
			}
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Added Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }*/
        public IActionResult Upsert(int? id)
        {
            if (id == null || id <= 0)
            {
				ProductVM productVM = new ProductVM()
				{
					Categeories = _unitOfWork.Category.GetAll().Select(
				u => new SelectListItem()
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
					Product = new Product()
				};
				return View(productVM);
            }
            else
            {
				ProductVM productVM = new ProductVM()
				{
					Categeories = _unitOfWork.Category.GetAll().Select(
				u => new SelectListItem()
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
					Product = _unitOfWork.Product.Get(u => u.Id == id)
			};
				return View(productVM);
			}
           
        }

        [HttpPost]
		public IActionResult Upsert(ProductVM  productVM,IFormFile file)
		{
           
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
            {
                    string filename=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productPath= Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldimage=Path.Combine(wwwRootPath, productVM.Product.ImageUrl.Trim('\\'));
                        if(System.IO.File.Exists(oldimage))
                        {
                            System.IO.File.Delete(oldimage);
                        }
                    }

                    using (var filestream= new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.ImageUrl= @"\images\product\"+filename;

            }
                
                if(productVM.Product.Id==0)
                {
                _unitOfWork.Product.Add(productVM.Product);
                TempData["success"] = "Product Added Successfully";
                }
                else
                {
					_unitOfWork.Product.Update(productVM.Product);
					TempData["success"] = "Product Updated Successfully";
				}
				_unitOfWork.Save();
                return RedirectToAction("Index");
            }else
			{
                productVM.Categeories = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
				return View(productVM);
			}

		}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");


        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductsList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new {data=ProductsList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj==null)
            {
            return Json(new { succss=false,message="Error While Deleting" });
            }
            var imagepath= Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { succss = true, message = "Deleted Succesfully" });
        }

        #endregion
    }
}
