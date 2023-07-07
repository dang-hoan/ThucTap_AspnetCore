using System.Linq.Expressions;

namespace LearnAspNetCoreMVC.Repositories
{
    public interface IRepository<T>
    {
        T? GetById(int id);
        IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, int? pageSize = null, int? pageNumber = null);
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
