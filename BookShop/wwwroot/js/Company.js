let dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll/",
        },
        "columns": [
            {"data": "companyName", "width": "15%"},
            {"data": "Street", "width": "15%"},
            {"data": "City", "width": "10%"},
            {"data": "Country", "width": "10%"},
            {"data": "PhoneNumber", "width": "15%"},
            {
                "data": "IsAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked></i>`
                    } else {
                        return `<input type="checkbox" disabled></i>`
                    }
                },
                "width": "15%"
            },
            {
                "data": "categoryId",
                "render": function (data) {

                    return `
                <div class="text-center">
                    <a href="/Admin/Company/Upsert/${data}" class="btn btn-icon btn-outline-primary btn-sm mx-1" title="Sửa">
                        <i class="fas fa-edit"></i>

                    </a>
                    <a onclick=Delete("/Admin/Company/Delete/${data}") class="btn btn-icon btn-outline-danger btn-sm mx-1" title="Xóa">
                        <i class="fas fa-trash"></i>
                    </a>
                </div>`;
                }, "width": "25%"
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