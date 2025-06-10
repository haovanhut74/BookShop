using System.Linq.Expressions;

namespace BookShop.Repository.IRepository;

//Kho luu tru dung chung
public interface IReponsitory<T> where T : class
{
    //truy xuat du lieu tu id
    T GetById(int id);

    //truy xuat tat ca du lieu
    IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null
    );

    T GetFirstOrDefault(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null
    );

    //them moi du lieu
    void Add(T entity);

    //Xoa du lieu tu id
    void Remove(int id);

    //Xoa du lieu tu entity
    void Remove(T entity);

    //Xoa nhieu du lieu tu entity
    void RemoveRange(IEnumerable<T> entities);
}