using LearnAspNetCoreMVC.CommandHandlers;
using LearnAspNetCoreMVC.CommandHandlers.Category;
using LearnAspNetCoreMVC.Commands.Category;
using LearnAspNetCoreMVC.Models;
using LearnAspNetCoreMVC.Queries.Category;
using LearnAspNetCoreMVC.QueryHandlers.Category;
using Microsoft.AspNetCore.Mvc;

namespace LearnAspNetCoreMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AddCommandHandler _addCommandHandler;
        private readonly DeleteCommandHandler _deleteCommandHandler;
        private readonly UpdateCommandHandler _updateCommandHandler;

        private readonly GetAllQueryHandler _getAllQueryHandler;
        private readonly GetByIdQueryHandler _getByIdQueryHandler;
        private readonly SearchQueryHandler _searchQueryHandler;

        public CategoryController(AddCommandHandler addCommandHandler, DeleteCommandHandler deleteCommandHandler,
                                UpdateCommandHandler updateCommandHandler, GetByIdQueryHandler getByIdQueryHandler,
                                SearchQueryHandler searchQueryHandler, GetAllQueryHandler getAllQueryHandler)
        {
            _addCommandHandler = addCommandHandler;
            _deleteCommandHandler = deleteCommandHandler;
            _updateCommandHandler = updateCommandHandler;

            _getByIdQueryHandler = getByIdQueryHandler;
            _searchQueryHandler = searchQueryHandler;
            _getAllQueryHandler = getAllQueryHandler;
        }
        //GET
        public IActionResult Index() {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Categories = _getAllQueryHandler.Handle(),
                DisplayOrder = null
            };
            return View(viewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SearchQuery obj)
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Categories = _searchQueryHandler.Handle(obj),
                DisplayOrder = null
            };
            return View(viewModel);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddCommand obj)
        {
            if (ModelState.IsValid)
            {
                if (_addCommandHandler.Handle(obj) > 0)
                {
                    TempData["success"] = "Category created successfully!";
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
            var categoryFromDb = _getByIdQueryHandler.Handle(new GetByIdQuery { Id = (int)id });

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateCommand obj)
        {
            if (ModelState.IsValid)
            {
                if (_updateCommandHandler.Handle(obj) > 0)
                {
                    TempData["success"] = "Category updated successfully!";
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
            var categoryFromDb = _getByIdQueryHandler.Handle(new GetByIdQuery { Id = (int)id });

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            if (ModelState.IsValid)
            {
                if (_deleteCommandHandler.Handle(id) > 0)
                {
                    TempData["success"] = "Category deleted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "There is an error in deleting data to database!";
                    return View(id);
                }
            }
            return View(id);
        }
    }
}