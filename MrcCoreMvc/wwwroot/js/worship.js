var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#ScheduleList').DataTable({
        ajax: {
            url: "/worship/getall/",
            type: "GET",
            datatype: "json"
        },
        columns: [
            {
                data: { worshipId: "worshipId", simpleDate: "simpleDate" }, width: "15%",
                render: function (data) { return `<a href="/Attendance/AttendanceList?worshipId=${data.worshipId}">${data.simpleDate}</a>` }
            },
            { data: 'simpleTime', width: "10%" },
            { data: 'worshipName', width: "20%" },
            //{ data: 'location', width: "30%" },
            {
                data: 'worshipId',
                render: function (data) {
                    return ` <div class="text-center">
                        <a href="/Worship/Details?worshipId=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            <i class="far fa-edit"></i> Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:100px;'
                            onclick=Delete('/Worship/Delete?worshipId='+${data})>
                            <i class="far fa-trash-alt"></i> Delete
                        </a>
                    </div>`;
                }, width: "25%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        width: "100%",
        "order": [[0, "desc"]]
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

