using LearnAspNetCoreMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearnAspNetCoreMVC.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
        }

        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, int? pageSize = null, int? pageNumber = null)
        {
            IQueryable<T> data = _context.Set<T>().AsNoTracking();

            try
            {
                if (filter != null)
                    data = data.Where(filter);

                if (pageSize != null && pageNumber != null)
                {
                    return data.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize);
                }

                return data;

            }catch (Exception)
            {
                return Enumerable.Empty<T>().AsQueryable().AsNoTracking();
            }
        }

        public virtual int Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();
        }

        public virtual int Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return _context.SaveChanges();
        }

        public virtual int Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChanges();
        }
    }
}
