var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {

    dataTable = $("#dataTable").DataTable({
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
                            <a onClick=Delete("product/delete/${data}") class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>`
                }
            }

        ]

    })

}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}
