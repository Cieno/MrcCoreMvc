var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/worship/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "worship_date", "width": "20%" },
            { "data": "WORSHIP_NAME", "width": "20%" },
            { "data": "WORSHIP_LOCATION", "width": "30%" },
            {
                "data": "WORSHIP_ID",
                "render": function (data) {
                    return ` <div class="text-center">
                        <a href="/Worship/Details?worshipId=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            <i class="far fa-edit"></i> Edit
                        </a>
                    </div>`;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

