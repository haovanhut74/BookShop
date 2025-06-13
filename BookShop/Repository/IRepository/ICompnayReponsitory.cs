using BookShop.Models;

namespace BookShop.Repository.IRepository;

public interface ICompanyReponsitory : IReponsitory<Company>
{
    void Update(Company company);
}