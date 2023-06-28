namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}