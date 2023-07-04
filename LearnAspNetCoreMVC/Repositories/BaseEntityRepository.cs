using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;

namespace LearnAspNetCoreMVC.Repositories
{
    public class BaseEntityRepository<T> : Repository<T> where T : BaseEntity
    {
        private readonly ApplicationDBContext _context;

        public BaseEntityRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override int Add(T entity)
        {
            entity.CreateDate = DateTime.Now;
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();
        }

        public override int Update(T entity)
        {
            entity.UpdateDate = DateTime.Now;
            _context.Set<T>().Update(entity);
            return _context.SaveChanges();
        }

        public override int Delete(T entity)
        {
            entity.DeleteDate = DateTime.Now;
            entity.IsDelete = true;
            _context.Set<T>().Update(entity);
            return _context.SaveChanges();
        }
    }
}
