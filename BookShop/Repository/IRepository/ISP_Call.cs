using Dapper;

namespace BookShop.Repository.IRepository;

// Giao diện định nghĩa các phương thức để gọi thủ tục SQL (Stored Procedures) dùng Dapper
public interface ISP_Call : IDisposable
{
    // Trả về một giá trị đơn kiểu T (thường dùng để lấy scalar: int, string,...)
    T Single<T>(string produreName, DynamicParameters? param = null);

    // Gọi stored procedure mà không cần lấy kết quả trả về (kiểu void)
    void Excute(string produreName, DynamicParameters? param = null);

    // Lấy duy nhất 1 bản ghi, gán vào đối tượng kiểu T (trả về 1 object)
    T? OneRecord<T>(string produreName, DynamicParameters? param = null);

    //Lấy danh sách các bản ghi từ stored procedure, kiểu IEnumerable<T>
    IEnumerable<T> List<T>(string produreName, DynamicParameters? param = null);

    //Trả về 2 danh sách kiểu khác nhau từ 1 stored procedure (Tuple)
    Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string produreName, DynamicParameters? param = null);
}