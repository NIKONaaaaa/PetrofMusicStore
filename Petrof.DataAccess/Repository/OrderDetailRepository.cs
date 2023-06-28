namespace Petrof.DataAccess.Repository
{
    using Petrof.DataAccess.Data;
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}