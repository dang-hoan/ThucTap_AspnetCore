using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using LearnAspNetCoreMVC.Queries.Category;

namespace LearnAspNetCoreMVC.QueryHandlers.Category
{
    public class GetByIdQueryHandler
    {
        private readonly ApplicationDBContext _db;
        public GetByIdQueryHandler(ApplicationDBContext db)
        {
            _db = db;
        }
        public Models.Category? Handle(GetByIdQuery query)
        {
            return _db.Categories.FirstOrDefault(obj => obj.Id == query.Id);
        }

    }
}
