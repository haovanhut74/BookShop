let dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll/",
        },
        "columns": [
            { "data": "categoryName", "width": "60%" },
            {
                "data": "categoryId",
                "render": function (data) {
                    return `
                <div class="text-center">
                    <a href="/Admin/Category/Upsert/${data}" class="btn btn-icon btn-outline-primary btn-sm mx-1" title="Sửa">
                        <i class="fas fa-edit"></i>

                    </a>
                    <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-icon btn-outline-danger btn-sm mx-1" title="Xóa">
                        <i class="fas fa-trash"></i>
                    </a>
                </div>`;
                }, "width": "40%"
            }
        ]
    });
}
function Delete(url) {
    swal({
        title: "Bạn có chắc chắn muốn xóa?",
        text: "Bạn sẽ không thể khôi phục lại dữ liệu này!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                }
            })
        } else {
            toastr.error(data.message);
        }
    })
}