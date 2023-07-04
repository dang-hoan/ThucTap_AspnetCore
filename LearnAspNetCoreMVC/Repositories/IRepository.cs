namespace LearnAspNetCoreMVC.Repositories
{
    public interface IRepository<T>
    {
        T? GetById(int id);
        IQueryable<T> GetAll();
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
