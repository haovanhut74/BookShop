using BookShop.Data;
using BookShop.Models;
using BookShop.Repository.IRepository;

namespace BookShop.Repository;

public class ProductReponsitory : Repository<Product>, IProductReponsitory
{
    private readonly ApplicationDbContext _dbContext;

    public ProductReponsitory(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Product product)
    {
        var objFromData = _dbContext.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
        if (objFromData != null)
        {
            objFromData.ImageUrl = product.ImageUrl;

            objFromData.ISBN = product.ISBN;
            objFromData.Price = product.Price;
            objFromData.ProductName = product.ProductName;
            objFromData.Description = product.Description;
            objFromData.CategoryId = product.CategoryId;
            objFromData.Author = product.Author;
            objFromData.CoverTypeId = product.CoverTypeId;
        }
        // _dbContext.SaveChanges();
    }
}