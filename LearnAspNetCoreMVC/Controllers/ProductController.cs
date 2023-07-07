using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using LearnAspNetCoreMVC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LearnAspNetCoreMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Company> _companyRepository;
        public ProductController(IRepository<Product> productRepository, IRepository<Company> companyRepository)
        {
            _productRepository = productRepository;
            _companyRepository = companyRepository;
        }
        public void InitCompaniesCombobox()
        {
            TempData["ListCompanies"] = from Company in _companyRepository.Get()
                                        select Company;
        }
        //GET
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = from product in _productRepository.Get()
                                                  join company in _companyRepository.Get() on product.CompanyID equals company.Id
                                                  let compareDate = product.UpdateDate != null ? product.UpdateDate : product.CreateDate
                                                  orderby compareDate descending, product.Name
                                                  select new Product
                                                  {
                                                      Id = product.Id,
                                                      Name = product.Name,
                                                      DisplayOrder = product.DisplayOrder,
                                                      Company = company
                                                  };

            //var objProductList = _productRepository.GetAll()
            //.Include(product => product.Company)
            //.ToList();

            ProductViewModel viewModel = new ProductViewModel()
            {
                Products = objProductList,
                Id = null,
                DisplayOrder = null,
                CompanyID = null
            };
            InitCompaniesCombobox();
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ProductViewModel obj)
        {
            IEnumerable<Product> res = from product in _productRepository.Get()
                                       join company in _companyRepository.Get() on product.CompanyID equals company.Id
                                       where (obj.Id == null || product.Id == obj.Id)
                                       && (obj.Name == null || product.Name.Contains(obj.Name))
                                       && (obj.DisplayOrder == null || product.DisplayOrder == obj.DisplayOrder)
                                       && (obj.CompanyID == null || obj.CompanyID == -1 || product.CompanyID == obj.CompanyID)
                                       let compareDate = product.UpdateDate != null ? product.UpdateDate : product.CreateDate
                                       orderby compareDate descending, product.Name
                                       select new Product
                                       {
                                           Id = product.Id,
                                           Name = product.Name,
                                           DisplayOrder = product.DisplayOrder,
                                           Company = company
                                       };

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
                if (_productRepository.Add(obj) > 0)
                {
                    TempData["success"] = "Product created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "There is an error in adding data to database!";
                    return View(obj);
                }
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
            var ProductFromDb = _productRepository.GetById((int)id);

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
                if (_productRepository.Update(obj) > 0)
                {
                    TempData["success"] = "Product updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "There is an error in updating data to database!";
                    return View(obj);
                }
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

            var ProductFromDb = (from product in _productRepository.Get()
                                 join company in _companyRepository.Get() on product.CompanyID equals company.Id
                                 where product.Id == id
                                 select new Product
                                 {
                                     Id = product.Id,
                                     Name = product.Name,
                                     DisplayOrder = product.DisplayOrder,
                                     Company = company
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
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _productRepository.GetById((int)id);
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_productRepository.Delete(obj) > 0)
                {
                    TempData["success"] = "Product deleted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "There is an error in deleting data to database!";
                    return View(obj);
                }
            }
            return View(obj);
        }
    }
}