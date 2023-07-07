using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using LearnAspNetCoreMVC.Repositories;

namespace LearnAspNetCoreMVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IRepository<Company> _companyRepository;
        public CompanyController(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        //private bool IsSwaggerRequest()
        //{
        //    //var userAgent = Request.Headers[HeaderNames.UserAgent].ToString();
        //    //return userAgent.Contains("Swagger");

        //    var headers = Request.Headers;
        //    var isSwaggerRequest = headers.ContainsKey("X-Requested-With") && headers["X-Requested-With"] == "XMLHttpRequest";
        //    isSwaggerRequest = isSwaggerRequest || (headers.ContainsKey("Referer") && headers["Referer"].ToString().Contains("/swagger"));
        //    return isSwaggerRequest;
        //}

        //GET
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = from Company in _companyRepository.Get()
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
            IEnumerable<Company> res = from Company in _companyRepository.Get()
                                       where (obj.Id == null || Company.Id == obj.Id)
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
                if (_companyRepository.Add(obj) > 0)
                {
                    TempData["success"] = "Company created successfully!";
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
            var CompanyFromDb = _companyRepository.GetById((int)id);

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
                if (_companyRepository.Update(obj) > 0)
                {
                    TempData["success"] = "Company updated successfully!";
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
            var CompanyFromDb = _companyRepository.GetById((int)id);

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
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _companyRepository.GetById((int)id);
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_companyRepository.Delete(obj) > 0)
                {
                    TempData["success"] = "Company deleted successfully!";
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