using BookShop.Models;

namespace BookShop.Repository.IRepository;

public interface ICategoryReponsitory : IReponsitory<Category>
{
    void Update(Category category);
}