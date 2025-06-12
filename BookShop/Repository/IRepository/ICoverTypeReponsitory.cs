using BookShop.Models;

namespace BookShop.Repository.IRepository;

public interface ICoverTypeResponsitory : IReponsitory<CoverType>
{
    void Update(CoverType coverType);
}