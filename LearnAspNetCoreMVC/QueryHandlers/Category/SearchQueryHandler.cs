using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using LearnAspNetCoreMVC.Queries.Category;

namespace LearnAspNetCoreMVC.QueryHandlers.Category
{
    public class SearchQueryHandler
    {
        private readonly ApplicationDBContext _db;
        public SearchQueryHandler(ApplicationDBContext db)
        {
            _db = db;
        }
        public IEnumerable<Models.Category> Handle(SearchQuery query)
        {
            return from category in _db.Categories
                   where (query.Name == null || category.Name.Contains(query.Name))
                   && (query.DisplayOrder == null || category.DisplayOrder == query.DisplayOrder)
                   orderby category.Name
                   select category;
        }

    }
}
