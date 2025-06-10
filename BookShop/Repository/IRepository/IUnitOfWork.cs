namespace BookShop.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    ICategoryReponsitory Category { get; }
    SP_Call SP_Call { get; }
}