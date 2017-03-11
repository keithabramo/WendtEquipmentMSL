$(function () {

    var BillOfLading = function () {

        this.initStyles = function () {
            
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    table.DataTable().$("input[type='checkbox']").prop("checked", true);
                }
            });

            $("table").on('change', '.quantity', function () {
                var check = $(this).val() && parseInt($(this).val(), 10) > 0;

                if (check) {
                    $(this).closest("tr").find("input[type='checkbox']").prop("checked", true);
                } else {
                    $(this).closest("tr").find("input[type='checkbox']").removeAttr("checked").prop("checked", false);
                }
            });

            $("table").on('change', 'input, textarea, select', function () {
                table.DataTable().cell($(this).closest("td")[0]).draw();
            });

            $("form").submit(function () {
                if (!form.invalid) {
                    var $inputs = table.DataTable().$('input, select');
                    var $hidden = $("<div>").addClass("hidden");
                    $(this).append($hidden);
                    $hidden.append($inputs);
                }
                form.invalid = false;
            });

            //autofill selection after finished event
            table.DataTable().on('autoFill', function (e, datatable, cells) {

                $.each(cells, function (i, cell) {

                    var index = cell[0].index;
                    var newValue = $(cell[0].set).val();

                    datatableCell = table.DataTable().cell(index.row, index.column);

                    var $cell = $(datatableCell.node());
                    $cell.find("input, textarea").val(newValue);

                    datatableCell.data($cell.html()).draw();

                });
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new BillOfLading();

});