namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        void Update(ProductImage obj);
    }
}