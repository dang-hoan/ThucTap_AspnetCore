using LearnAspNetCoreMVC.Commands.Category;
using LearnAspNetCoreMVC.Data;

namespace LearnAspNetCoreMVC.CommandHandlers.Category
{
    public class AddCommandHandler
    {
        private readonly ApplicationDBContext _db;

        public AddCommandHandler(ApplicationDBContext db)
        {
            _db = db;
        }

        public int Handle(AddCommand command)
        {
            var category = new Models.Category
            {
                Name = command.Name,
                DisplayOrder = command.DisplayOrder
            };

            _db.Categories.Add(category);
            return _db.SaveChanges();
        }

    }
}
