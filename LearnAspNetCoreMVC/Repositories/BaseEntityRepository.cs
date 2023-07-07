using LearnAspNetCoreMVC.Data;
using LearnAspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearnAspNetCoreMVC.Repositories
{
    public class BaseEntityRepository<T> : Repository<T> where T : BaseEntity
    {
        private readonly ApplicationDBContext _context;

        public BaseEntityRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public override IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, int? pageSize = null, int? pageNumber = null)
        {
            IQueryable<T> data = _context.Set<T>().Where(entity => entity.IsDelete == false).AsNoTracking();

            try
            {
                if (filter != null)
                    data = data.Where(filter);

                if (pageSize != null && pageNumber != null)
                {
                    return data.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize);
                }

                return data;
            }catch(Exception)
            {
                return Enumerable.Empty<T>().AsQueryable().AsNoTracking();
            }

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
