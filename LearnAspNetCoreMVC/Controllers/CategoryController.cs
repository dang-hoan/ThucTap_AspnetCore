using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAspNetCoreMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }
        //GET
        public IActionResult Index() {
            IEnumerable<Category> objCategoryList = from category in _db.Categories
                                                    orderby category.Name
                                                    select category;

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Categories = objCategoryList,
                DisplayOrder = null
            };
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CategoryViewModel obj)
        {
            IEnumerable<Category> res = from category in _db.Categories
                                        where (obj.Name == null || category.Name.Contains(obj.Name)) && (obj.DisplayOrder == null || category.DisplayOrder == obj.DisplayOrder)
                                        orderby category.Name
                                        select category;                                        

            obj.Categories = res;
            return View(obj);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDb = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDb = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }

        //POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Search()
        //{
        //    return Index();
        //}
    }
}