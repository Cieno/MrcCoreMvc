var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#UserList').DataTable({
        ajax: {
            url: "/user/getusers/",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: 'fullName', width: "20%"  },
            { data: 'email', width: "20%" },
            { data: 'phoneNumber', width: "20%" },
            { data: 'roleName', width: "20%" },
            {
                data: 'id',
                render: function (data) {
                    return ` <div class="text-center">
                        <a href="/User/Details?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            <i class="far fa-edit"></i> Edit
                        </a>
                    </div>`;
                }, width: "20%"
            }
            //{
            //    data: 'id',
            //    render: function (data) {
            //        return ` <div class="text-center">
            //            <a href="/User/Details?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
            //                <i class="far fa-edit"></i> Edit
            //            </a>
            //            &nbsp;
            //            <a class='btn btn-danger text-white' style='cursor:pointer; width:100px;'
            //                onclick="Delete('/User/Delete?id='+'${data}');">
            //                <i class="far fa-trash-alt"></i> Delete
            //            </a>
            //        </div>`;
            //    }, width: "20%"
            //}
        ],
        "language": {
            "emptyTable": "no data found."
        },
        width: "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

