using DAO.Models;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;

        public CompanyService(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateAsync(Company company)
        {
            await _repo.AddAsync(company);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _repo.Update(company);
            await _repo.SaveAsync();
        }
    }
}
