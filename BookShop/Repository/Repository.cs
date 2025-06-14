using System.Linq.Expressions;
using BookShop.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

// Dùng để truyền biểu thức lọc động như Where(x => x.Name == "A")
// Chứa ApplicationDbContext

// Hỗ trợ các hàm như .Include(), .Set<>, .Find()

namespace BookShop.Repository;

// Repository chung, dùng cho mọi entity class
public class Repository<T> : IReponsitory<T> where T : class
{
    private DbSet<T> _dbSet; // DbSet đại diện cho bảng tương ứng kiểu T trong database

    protected Repository(DbContext dbContext)
    {
        // Gán DbContext được truyền vào
        _dbSet = dbContext.Set<T>(); // Gán DbSet dựa trên kiểu T
    }


    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet; // Bắt đầu từ toàn bộ dữ liệu của T

        if (filter != null)
            query = query.Where(filter); // Áp dụng điều kiện lọc nếu có

        if (includeProperties != null) // Nếu có chuỗi include
        {
            foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }


        }

        if (orderBy != null)
        {
            return orderBy(query).ToList(); // Trả về dữ liệu sau khi sắp xếp
        }

        return query.ToList(); // Trả về toàn bộ dữ liệu nếu không sắp xếp
    }

    // Lấy bản ghi đầu tiên phù hợp điều kiện (nếu có), kèm include nếu cần
    public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter); // Lọc dữ liệu theo biểu thức

        if (includeProperties != null)
        {
            foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }


        }

        // Trả về kết quả đầu tiên hoặc báo lỗi nếu không có
        return query.FirstOrDefault() ?? throw new Exception("Khong tim thay du lieu");
    }

    // Thêm mới một entity
    public void Add(T entity)
    {
        _dbSet.Add(entity); // Dùng DbSet để thêm đối tượng
    }

    // Lấy entity theo id (primary key)
    public T GetById(int id)
    {
        return _dbSet.Find(id) ?? throw new Exception("Khong tim thay du lieu"); // Tìm theo id, không thấy thì báo lỗi
    }

    // Xóa entity dựa trên id
    public void Remove(int id)
    {
        T entity = _dbSet.Find(id) ?? throw new Exception("Khong tim thay du lieu"); // Tìm entity theo id
        _dbSet.Remove(entity); // Xóa entity khỏi DbSet
    }

    // Xóa entity truyền vào
    public void Remove(T entity)
    {
        _dbSet.Remove(entity); // Xóa trực tiếp đối tượng
    }

    // Xóa danh sách entity
    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities); // Xóa hàng loạt
    }
}