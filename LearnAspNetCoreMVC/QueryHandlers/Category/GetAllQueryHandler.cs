using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using LearnAspNetCoreMVC.Queries.Category;

namespace LearnAspNetCoreMVC.QueryHandlers.Category
{
    public class GetAllQueryHandler
    {
        private readonly ApplicationDBContext _db;
        public GetAllQueryHandler(ApplicationDBContext db)
        {
            _db = db;
        }
        public IQueryable<Models.Category> Handle()
        {
            return from category in _db.Categories
                   orderby category.Name
                   select category;
        }

    }
}
