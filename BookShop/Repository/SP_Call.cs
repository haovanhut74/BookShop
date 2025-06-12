// Import các thư viện cần thiết

using BookShop.Data; // DbContext chứa connection đến database
using BookShop.Repository.IRepository; // Interface ISP_Call
using Dapper; // Thư viện Dapper để chạy stored procedure
using Microsoft.Data.SqlClient; // Dùng để tạo kết nối SQL Server
using Microsoft.EntityFrameworkCore; // Dùng để lấy chuỗi kết nối từ DbContext

// Khai báo namespace chứa lớp repository
namespace BookShop.Repository;

// Khai báo class SP_Call, triển khai giao diện ISP_Call
public class SP_Call : ISP_Call
{
    // Biến dùng để giữ DbContext hiện tại
    private readonly ApplicationDbContext _dbContext;

    // Biến static dùng để lưu chuỗi kết nối
    private static string _Conn = "";

    // Constructor: Nhận DbContext và lấy ConnectionString từ nó
    public SP_Call(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _Conn = dbContext.Database.GetDbConnection().ConnectionString;
    }

    // Hàm Dispose để giải phóng tài nguyên khi không còn dùng nữa
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    // Hàm gọi stored procedure và trả về 1 giá trị đơn lẻ (ví dụ: int, string)
    public T Single<T>(string produreName, DynamicParameters? param = null)
    {
        using (SqlConnection sqlConn = new SqlConnection(_Conn)) // Tạo kết nối SQL
        {
            sqlConn.Open(); // Mở kết nối
            // ExecuteScalar chạy procedure và trả về 1 giá trị
            var value = sqlConn.ExecuteScalar(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
            // Ép kiểu giá trị về kiểu T và trả kết quả
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    // Hàm gọi stored procedure chỉ để thực thi (INSERT, UPDATE, DELETE), không cần trả kết quả
    public void Excute(string produreName, DynamicParameters? param = null)
    {
        using (SqlConnection sqlConn = new SqlConnection(_Conn)) // Tạo kết nối SQL
        {
            sqlConn.Open(); // Mở kết nối
            // Execute chạy procedure mà không cần lấy kết quả trả về
            sqlConn.Execute(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    // Hàm gọi stored procedure và lấy 1 bản ghi duy nhất
    public T OneRecord<T>(string produreName, DynamicParameters? param = null)
    {
        using (SqlConnection sqlConn = new SqlConnection(_Conn)) // Tạo kết nối SQL
        {
            sqlConn.Open(); // Mở kết nối
            // Query trả về danh sách object (có thể rỗng)
            var value = sqlConn.Query<T>(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
            // Lấy dòng đầu tiên, ép kiểu về T
            return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
        }
    }

    // Hàm gọi stored procedure và trả về danh sách kết quả
    public IEnumerable<T> List<T>(string produreName, DynamicParameters? param = null)
    {
        using (SqlConnection sqlConn = new SqlConnection(_Conn)) // Tạo kết nối SQL
        {
            sqlConn.Open(); // Mở kết nối
            // Query trả về IEnumerable<T> từ procedure
            return sqlConn.Query<T>(produreName, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }
    }

    // Hàm gọi stored procedure và trả về 2 danh sách (kết quả 2 bảng)
    public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string produreName, DynamicParameters? param = null)
    {
        using (SqlConnection sqlConn = new SqlConnection(_Conn)) // Tạo kết nối SQL
        {
            sqlConn.Open(); // Mở kết nối
            // QueryMultiple giúp trả nhiều kết quả từ 1 procedure
            var result = SqlMapper.QueryMultiple(sqlConn, produreName, param,
                commandType: System.Data.CommandType.StoredProcedure);

            // Đọc danh sách thứ nhất
            var list1 = result.Read<T1>().ToList();
            // Đọc danh sách thứ hai
            var list2 = result.Read<T2>().ToList();

            // Nếu cả 2 list đều có dữ liệu thì trả tuple
            if (list2 != null && list1 != null)
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(list1, list2);
            }
        }

        // Trường hợp fail hoặc null thì trả về tuple rỗng
        return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
    }
}