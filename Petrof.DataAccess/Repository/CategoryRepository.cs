namespace Petrof.DataAccess.Repository
{
    using Petrof.DataAccess.Data;
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}