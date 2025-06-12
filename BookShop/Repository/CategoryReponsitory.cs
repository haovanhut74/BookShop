using BookShop.Data;
using BookShop.Models;
using BookShop.Repository.IRepository;

namespace BookShop.Repository;

public class CategoryReponsitory : Repository<Category>, ICategoryReponsitory
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryReponsitory(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Category category)
    {
        //_dbContext.Categories.Update(category);
        var objFromData = _dbContext.Categories
            //CategoryId bằng với category.CategoryId
            .FirstOrDefault(x => x.CategoryId == category.CategoryId);
        if (objFromData != null)
        {
            objFromData.CategoryName = category.CategoryName;
        }
        // _dbContext.SaveChanges();
    }
}