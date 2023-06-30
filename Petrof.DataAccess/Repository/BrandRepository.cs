namespace Petrof.DataAccess.Repository
{
    using Petrof.DataAccess.Data;
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand obj)
        {
            _db.Brands.Update(obj);
        }
    }
}