var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/Order/GetAllOrders" },
        "columns": [
            { data: 'OrderHeaderId', "width": "5%" },
            { data: 'Name', "width": "20%" },
            { data: 'Email', "width": "25%" },
            { data: 'PhoneNumber', "width": "10%" },
            { data: 'Status', "width": "10%" },
            { data: 'OrderTotal', "width": "10%" },
        ]
    })
}