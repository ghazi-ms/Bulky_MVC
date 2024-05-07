using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Category> CategoriesList = _dbContext.Categories.ToList();
            return View(CategoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category NewCategory)
        {
            if (NewCategory.Name == NewCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","Category Name And Display Order Cant Be Equal");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(NewCategory);
                _dbContext.SaveChanges();
                TempData["success"] = "Category Added Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id==null || id<=0)
            {
                return NotFound();
            }
            Category? category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category NewCategory)
        {
          
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(NewCategory);
                _dbContext.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Category? category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj= _dbContext.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");


        }
    }
}
