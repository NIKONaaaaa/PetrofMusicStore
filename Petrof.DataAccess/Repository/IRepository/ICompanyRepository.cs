namespace Petrof.DataAccess.Repository.IRepository
{
    using Petrof.Models;
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}