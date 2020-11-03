var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#RoleList').DataTable({
        ajax: {
            url: "/userRole/GetRoles/",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: 'name', width: "50%" },
            {
                data: 'name',
                render: function (data) {
                    return ` <div class="text-center">
                        <a href="/UserRole/Details?name=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            <i class="far fa-edit"></i> Edit
                        </a>
                    </div>`;
                }, width: "50%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        width: "100%"
    });
}

