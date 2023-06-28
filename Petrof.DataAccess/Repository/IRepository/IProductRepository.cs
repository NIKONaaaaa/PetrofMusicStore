namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}