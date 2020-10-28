var dataTable;
var wId = document.currentScript.getAttribute('wId');
$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#AttendanceList').DataTable({
        ajax: {
            url: "/Attendance/GetAttendanceList",
            data: {
                worshipId: wId
            },
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: 'memberName', width: "25%" },
            { data: 'attendanceType', width: "25%" },
            {
                data: 'estimatedArrival', width: "25%",
                render: function (data) {
                    return `${("0" + data.hours).slice(-2)}:${("0" + data.minutes).slice(-2)}`
                }
            },
            {
                data: { "worshipId": "worshipId", "memberId": "memberId" },
                render: function (data) {
                    return `<div class="text-center"> 
                              <a class='btn btn-danger text-white' style='cursor:pointer; width:100px;'
                                onclick="Delete('/Attendance/Delete?worshipId='+${data.worshipId}+'&memberId='+'${(data.memberId)}');"> 
                                <i class="far fa-trash-alt"></i> Delete
                              </a>
                            </div>`
                }, width: "25%"
            }
        ],
        language: {
            emptyTable: "no data found."
        },
        width: "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
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