using BookShop.Models;

namespace BookShop.Repository.IRepository;

public interface IProductReponsitory : IReponsitory<Product>
{
    void Update(Product product);
}