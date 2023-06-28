namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}