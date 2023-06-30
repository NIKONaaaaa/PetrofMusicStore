namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface IBrandRepository : IRepository<Brand>
    {
        void Update(Brand obj);
    }
}