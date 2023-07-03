using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LearnAspNetCoreMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly Data.ApplicationDBContext _db;
        public ProductController(Data.ApplicationDBContext db)
        {
            _db = db;
        }
        public void InitCompaniesCombobox()
        {
            TempData["ListCompanies"] = from Company in _db.Companies
                                        where Company.IsDelete == false
                                        select Company;
        }
        //GET
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = from Product in _db.Products
                                                  where Product.IsDelete == false
                                                  let compareDate = Product.UpdateDate != null ? Product.UpdateDate : Product.CreateDate
                                                  orderby compareDate descending, Product.Name
                                                  select Product;

            ProductViewModel viewModel = new ProductViewModel()
            {
                Products = objProductList,
                Id = null,
                DisplayOrder = null,
                CompanyId = null
            };
            InitCompaniesCombobox();
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ProductViewModel obj)
        {
            IEnumerable<Product> res = from Product in _db.Products
                                       where Product.IsDelete == false
                                       && (obj.Id == null || Product.Id == obj.Id)
                                       && (obj.Name == null || Product.Name.Contains(obj.Name))
                                       && (obj.DisplayOrder == null || Product.DisplayOrder == obj.DisplayOrder)
                                       && (obj.CompanyId == null || obj.CompanyId == -1 || Product.CompanyId == obj.CompanyId)
                                       let compareDate = Product.UpdateDate != null ? Product.UpdateDate : Product.CreateDate
                                       orderby compareDate descending, Product.Name
                                       select Product;

            obj.Products = res;
            InitCompaniesCombobox();
            return View(obj);
        }

        //GET
        public IActionResult Create()
        {
            InitCompaniesCombobox();
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            ModelState.Remove("Company");
            if (ModelState.IsValid)
            {
                obj.CreateDate = DateTime.Now;
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

            InitCompaniesCombobox();
            return View(ProductFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            ModelState.Remove("Company");
            if (ModelState.IsValid)
            {
                obj.UpdateDate = DateTime.Now;
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

            var ProductFromDb = (from product in _db.Products
                                 join company in _db.Companies on product.CompanyId equals company.Id
                                 where product.Id == id
                                 select new Product
                                 {
                                     Id = product.Id,
                                     Name = product.Name,
                                     DisplayOrder = product.DisplayOrder,
                                     CompanyId = product.CompanyId,
                                     Company = company,
                                     CreateDate = product.CreateDate,
                                     UpdateDate = product.UpdateDate,
                                     DeleteDate = product.DeleteDate,
                                     IsDelete = product.IsDelete
                                 }).FirstOrDefault();


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

            obj.DeleteDate = DateTime.Now;
            obj.IsDelete = true;
            _db.Products.Update(obj);
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