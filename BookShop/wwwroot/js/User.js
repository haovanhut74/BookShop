let dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll/",
        },
        "columns": [
            { "data": "email", "width": "30%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.companyName", "width": "20%" },
            { "data": "role", "width": "15%" },
            {
                "data": null,
                "render": function (data, type, row) {
                    var today = new Date().getTime();
                    var lockout = new Date(row.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <button onclick="LockUnlock('${row.id}')" class="btn btn-outline-primary btn-sm mx-1" title="Unlock">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </button>
                            </div>`;
                    } else {
                        return `
                            <div class="text-center">
                                <button onclick="LockUnlock('${row.id}')" class="btn btn-success btn-sm mx-1" title="Lock">
                                    <i class="fas fa-lock"></i> Lock
                                </button>
                            </div>`;
                    }
                },
                "width": "20%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        url: "/Admin/User/LockUnlock",
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message, "Success");
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message, "Operation Failed");
            }
        }
    });
}