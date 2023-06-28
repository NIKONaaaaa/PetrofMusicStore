namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}