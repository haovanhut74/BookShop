// Import các thư viện cần thiết

using BookShop.Data; // DbContext để làm việc với CSDL
using BookShop.Repository.IRepository; // Interface của Repository và UnitOfWork

namespace BookShop.Repository;

// Class UnitOfWork triển khai từ IUnitOfWork
public class UnitOfWork : IUnitOfWork
{
    // Đối tượng DbContext dùng chung cho toàn bộ repository
    private readonly ApplicationDbContext _dbContext;

    // Repository dành riêng cho bảng Category
    public ICategoryReponsitory Category { get; }
    public ICoverTypeResponsitory CoverType { get; }

    // Cài đặt thuộc tính công khai để sử dụng SP_Call
    public ISP_Call SP_Call { get; private set; }

    // Constructor: nhận DbContext từ DI container, khởi tạo các repository và SP_Call
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        // Khởi tạo repository Category với context dùng chung
        Category = new CategoryReponsitory(_dbContext);

        CoverType = new CoverTypeReponsitory(_dbContext);
        
        // Khởi tạo SP_Call để gọi stored procedure bằng Dapper
        SP_Call = new SP_Call(_dbContext);
    }

    // Hàm Dispose: giải phóng DbContext khi xong việc
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    // Hàm Save: lưu tất cả thay đổi từ các repository vào database
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}