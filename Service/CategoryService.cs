using DAO.Models;
using Repository;
using Repository.Interface;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Category>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Category?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public async Task AddAsync(Category category)
    {
        await _repo.AddAsync(category);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _repo.Update(category);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category != null)
        {
            _repo.Delete(category);
            await _repo.SaveAsync();
        }
    }
    public async Task SaveAsync()
    {
        await _repo.SaveAsync();
    }

    public IEnumerable<Category> GetAll()
    {
        return _repo.GetAll();
    }
}
