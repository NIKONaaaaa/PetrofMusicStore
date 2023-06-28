namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
    }
}