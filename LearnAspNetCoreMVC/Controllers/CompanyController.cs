using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAspNetCoreMVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CompanyController(ApplicationDBContext db)
        {
            _db = db;
        }
        //GET
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = from Company in _db.Companies
                                                  where Company.IsDelete == false
                                                  let compareDate = Company.UpdateDate != null ? Company.UpdateDate : Company.CreateDate
                                                  orderby compareDate descending, Company.Name
                                                  select Company;

            CompanyViewModel viewModel = new CompanyViewModel()
            {
                Companys = objCompanyList,
                Id = null
            };
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CompanyViewModel obj)
        {
            IEnumerable<Company> res = from Company in _db.Companies
                                       where Company.IsDelete == false 
                                       && (obj.Id == null || Company.Id == obj.Id) 
                                       && (obj.Name == null || Company.Name.Contains(obj.Name)) 
                                       && (obj.Address == null || Company.Address.Contains(obj.Address))
                                       && (obj.PhoneNumber == null || Company.PhoneNumber.Contains(obj.PhoneNumber))
                                       let compareDate = Company.UpdateDate != null ? Company.UpdateDate : Company.CreateDate
                                       orderby compareDate descending, Company.Name
                                       select Company;

            obj.Companys = res;
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
        public IActionResult Create(Company obj)
        {
            if (ModelState.IsValid)
            {
                obj.CreateDate = DateTime.Now;
                _db.Companies.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Company created successfully!";
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
            var CompanyFromDb = _db.Companies.Find(id);
            //var CompanyFromDb = _db.Companies.FirstOrDefault(u => u.Id == id);
            //var CompanyFromDb = _db.Companies.SingleOrDefault(u => u.Id == id);

            if (CompanyFromDb == null)
            {
                return NotFound();
            }

            return View(CompanyFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                obj.UpdateDate = DateTime.Now;
                _db.Companies.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Company updated successfully!";
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
            var CompanyFromDb = _db.Companies.Find(id);
            //var CompanyFromDb = _db.Companies.FirstOrDefault(u => u.Id == id);
            //var CompanyFromDb = _db.Companies.SingleOrDefault(u => u.Id == id);

            if (CompanyFromDb == null)
            {
                return NotFound();
            }

            return View(CompanyFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Companies.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            obj.DeleteDate = DateTime.Now;
            obj.IsDelete = true;
            _db.Companies.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Company deleted successfully!";
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