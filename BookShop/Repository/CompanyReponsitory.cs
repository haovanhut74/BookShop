using BookShop.Data;
using BookShop.Models;
using BookShop.Repository.IRepository;

namespace BookShop.Repository;

public class CompanyReponsitory : Repository<Company>, ICompanyReponsitory
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyReponsitory(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Company company)
    {
        _dbContext.Update(company);
    }
}