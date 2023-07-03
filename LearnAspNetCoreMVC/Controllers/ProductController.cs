using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAspNetCoreMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;
        public ProductController(ApplicationDBContext db)
        {
            _db = db;
        }
        //GET
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = from Product in _db.Products
                                                  orderby Product.Name
                                                  select Product;

            ProductViewModel viewModel = new ProductViewModel()
            {
                Products = objProductList,
                DisplayOrder = null
            };
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ProductViewModel obj)
        {
            IEnumerable<Product> res = from Product in _db.Products
                                       where (obj.Name == null || Product.Name.Contains(obj.Name)) && (obj.DisplayOrder == null || Product.DisplayOrder == obj.DisplayOrder)
                                       orderby Product.Name
                                       select Product;

            obj.Products = res;
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
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Product created successfully!";
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
            var ProductFromDb = _db.Products.Find(id);
            //var ProductFromDb = _db.Products.FirstOrDefault(u => u.Id == id);
            //var ProductFromDb = _db.Products.SingleOrDefault(u => u.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Product updated successfully!";
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
            var ProductFromDb = _db.Products.Find(id);
            //var ProductFromDb = _db.Products.FirstOrDefault(u => u.Id == id);
            //var ProductFromDb = _db.Products.SingleOrDefault(u => u.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Products.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Product deleted successfully!";
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