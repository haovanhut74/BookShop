using BookShop.Data;
using BookShop.Models;
using BookShop.Repository.IRepository;

namespace BookShop.Repository;

public class CoverTypeReponsitory : Repository<CoverType>, ICoverTypeResponsitory
{
    private readonly ApplicationDbContext _dbContext;

    public CoverTypeReponsitory(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(CoverType coverType)
    {
        //_dbContext.Categories.Update(category);
        var objFromData = _dbContext.CoverTypes
            //CategoryId bằng với category.CategoryId
            .FirstOrDefault(x => x.CoverTypeId == coverType.CoverTypeId);
        if (objFromData != null)
        {
            objFromData.CoverTypeName = coverType.CoverTypeName;
        }
        // _dbContext.SaveChanges();
    }

    public IEnumerable<CoverType> GetCoverTypes()
    {
        return _dbContext.CoverTypes.ToList();
    }
}