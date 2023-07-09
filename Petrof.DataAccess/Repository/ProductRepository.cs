namespace Petrof.DataAccess.Repository
{
    using Petrof.DataAccess.Data;
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    using Petrof.Utility;
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.BrandId = obj.BrandId;
                objFromDb.Weight = obj.Weight;
                objFromDb.Price = obj.Price;
                objFromDb.ProductImages = obj.ProductImages;
                objFromDb.VideoUrl = LinkTransformer.Transform(obj.VideoUrl);
                objFromDb.InStock = obj.InStock;
            }
        }
    }
}