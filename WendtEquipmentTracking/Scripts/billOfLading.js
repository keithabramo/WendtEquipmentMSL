$(function () {

    var BillOfLading = function () {

        this.initStyles = function () {
            
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    table.DataTable().$("input[type='checkbox']").prop("checked", true).change();
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

            $("table").on('change', 'tbody input[type="checkbox"]', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    var readyToShipVal = $(this).closest("tr").find(".originalReadyToShip").val();
                    var $quantity = $(this).closest("tr").find(".quantity");

                    if (!$quantity.val() || $quantity.val() === "0") {
                        $quantity.val(readyToShipVal);
                    }
                    
                }
            });

            //$("table").on('change', 'input, textarea, select', function () {
            //    var datatableCell = table.DataTable().cell($(this).closest("td")[0]);
            //    $(this).attr("value", $(this).val());
            //    datatableCell.data($(this).closest("td").html()).draw();
            //});

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

                var value;
                $.each(cells, function (i, cell) {

                    var index = cell[0].index;
                    datatableCell = table.DataTable().cell(index.row, index.column);
                    var $cell = $(datatableCell.node());

                    if (i === 0) {
                        value = $cell.find("input, textarea").val();
                    } else {
                        $cell.find("input, textarea").val(value).attr("value", value).change();

                        datatableCell.data($cell.html()).draw();
                    }
                });
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new BillOfLading();

});