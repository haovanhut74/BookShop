namespace BookShop.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    ICategoryReponsitory Category { get; }
    ICoverTypeResponsitory CoverType { get; }
    IProductReponsitory Product { get; }
    ICompanyReponsitory Company { get; }
    ISP_Call SP_Call { get; }
    void Save();
}