$(function () {

    var Table = function () {

        this.dataTable;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $searchHeader = $(".table.my-datatable thead tr").clone();

            $searchHeader.find("th").each(function () {
                var title = $.trim($(this).text());

                if (title) {
                    $(this).html('<input class="form-control" type="text" placeholder="Search ' + title + '" />');
                }
            });

            $(".table.my-datatable thead").prepend($searchHeader);


            this.dataTable = $(".table.my-datatable").DataTable({
                bPaginate: false,
                bInfo: false
            });
        }

        this.initEvents = function () {
            var $this = this;

            $('.table.my-datatable').on('click', ".expand", function () {
                var id = $(this).attr("data-id");

                var $row = $(this).closest('tr');
                var datatableRow = $this.dataTable.row($row);

                if (datatableRow.child.isShown()) {
                    // This row is already open - close it
                    datatableRow.child.hide();
                }
                else {
                    // Open this row
                    datatableRow.child($("#" + id).html()).show();
                }


                var $icon = $(this).find(".glyphicon");

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });


            // Apply the search
            this.dataTable.columns().every(function () {
                var column = this;

                var $input = $("thead tr").eq(0).find("th").eq(this.index()).find("input");

                $input.on('keyup change', function () {
                    if (column.search() !== this.value) {
                        column.search(this.value).draw();
                    }
                });
            });
        }


        this.initStyles();
        this.initEvents();
    }

    table = new Table();

});

function DeleteSuccess() {
    $(this).closest("tr").remove();
};