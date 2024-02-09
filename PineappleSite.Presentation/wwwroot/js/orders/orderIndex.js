var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }

    else {

        if (url.includes("readyforpickup")) {
            loadDataTable("readyforpickup");
        }

        else {

            if (url.includes("cancelled")) {
                loadDataTable("cancelled");
            }

            else {
                loadDataTable("all");
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        order: [[0, 'desc']],
        "ajax": { url: "/Order/GetAllOrders?status=" + status },
        "columns": [
            { data: 'OrderHeaderId', "width": "5%" },
            { data: 'Email', "width": "25%" },
            { data: 'Name', "width": "20%" },
            { data: 'PhoneNumber', "width": "10%" },
            { data: 'Status', "width": "10%" },
            { data: 'OrderTotal', "width": "10%" },
            {
                data: 'OrderHeaderId',
                "render": function (data) {
                    return `<div class="w-75 btn-group text-center" role="group">
                    <a href="/Order/GetOrderDetails?orderId=${data}" class="btn btn-primary btn-sm mx-1"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            }
        ],
    })
}