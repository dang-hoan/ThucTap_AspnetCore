using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Queries.Category;
using LearnAspNetCoreMVC.QueryHandlers.Category;

namespace LearnAspNetCoreMVC.CommandHandlers
{
    public class DeleteCommandHandler
    {
        private readonly ApplicationDBContext _db;

        public DeleteCommandHandler(ApplicationDBContext db)
        {
            _db = db;
        }

        public int Handle(int id)
        {
            _db.Categories.Remove(new GetByIdQueryHandler(_db).Handle(new GetByIdQuery { Id = id }));
            return _db.SaveChanges();
        }
    }
}
