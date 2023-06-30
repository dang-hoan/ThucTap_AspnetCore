using LearnAspNetCoreMVC.Commands.Category;
using LearnAspNetCoreMVC.Data;

namespace LearnAspNetCoreMVC.CommandHandlers
{
    public class UpdateCommandHandler
    {
        private readonly ApplicationDBContext _db;

        public UpdateCommandHandler(ApplicationDBContext db)
        {
            _db = db;
        }

        public int Handle(UpdateCommand command)
        {
            var category = new Models.Category
            {
                Id = command.Id,
                Name = command.Name,
                DisplayOrder = command.DisplayOrder
            };

            _db.Categories.Update(category);
            return _db.SaveChanges();
        }
    }
}
