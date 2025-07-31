$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {

    $("#dataTable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll",
            "type": "GET",
            "dataSrc": "data" 
        },
        "columns": [
            { "data": "title" },
            { "data": "description" },
            { "data": "isbn" },
            { "data": "author" },
            { "data": "listPrice" },
            { "data": "price" },
            { "data": "price50" },
            { "data": "price100" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-50 btn-group" role="group">
                            <a href="product/upsert?id=${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="product/upsert?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>`
                }
            }

        ]

    })

}
