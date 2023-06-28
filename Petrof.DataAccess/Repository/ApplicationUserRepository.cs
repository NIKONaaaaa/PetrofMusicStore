namespace Petrof.DataAccess.Repository
{
    using Petrof.DataAccess.Data;
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
        }
    }
}