var dataTable;
$(document).ready(function () {
    loadData();
});

function loadData() {
    dataTable = $("#myTable").DataTable({
        "ajax": {
            "url": "/Admin/OrderManagment/GetData"
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "25%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'totalPrice', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {

                    return `
                    <a href="/admin/orderManagment/details?orderid=${data}" class="btn btn-success">Details</a>
                   
                    
                    `
                }

            }

        ]
    });
}
