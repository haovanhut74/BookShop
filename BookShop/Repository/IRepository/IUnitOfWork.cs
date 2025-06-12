namespace BookShop.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    ICategoryReponsitory Category { get; }
    ICoverTypeResponsitory CoverType { get; }
    ISP_Call SP_Call { get; }
    void Save();
}