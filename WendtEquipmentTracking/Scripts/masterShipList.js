$(function () {

    var MasterShipList = function () {
        $.ajaxSetup({ cache: false });

        this.editsMade = false;
        this.editQueue = [];
        this.createURL = ROOT_URL + "api/EquipmentApi/Create";
        this.editURL = ROOT_URL + "api/EquipmentApi/Edit";

        this.initStyles = function () {
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var equipment = data[0];
                        var leftToShip = data[13];

                        if (equipment.toLowerCase() === "hardware" && leftToShip && parseInt(leftToShip, 10) > 0) {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Work In Progress</label>');
            
            
        }

        this.initEvents = function () {
            var $this = this;


            //edits made
            $('.table').on('change', "> tbody > tr input, > tbody > tr select", function () {
                $this.editsMade = true;
            });

            //checbox ready to ship filter
            $('#readyToShipFilter').on("change", function () {
                table.DataTable().draw();
            });

            //autofill selection after finished event
            table.DataTable().on('autoFill', function (e, datatable, cells) {

                $.each(cells, function (i, cell) {

                    var index = cell[0].index;

                    var $cell = $(table.DataTable().cell(index.row, index.column).node());
                    $cell.find("input").val(cell[0].set);

                    var $row = $(table.DataTable().row(index.row).node());

                    $this.editQueue.push($row);

                });

                $this.updateRow.call($this, $this.editQueue.pop());
            });


            //checking ishardware to see if we need to disable equipment name
            $('.table').on('change', "input[name='IsHardware']", function () {
                var isHardware = $(this).is(":checked");
                var $equipmentNameInput = $(this).closest("tr").find("input[name='EquipmentName']");

                if (isHardware) {
                    $equipmentNameInput.val("HARDWARE").attr("readonly", "readonly");
                } else {
                    $equipmentNameInput.removeAttr("readonly");
                }
            });

            //saving an edited row if changes were made
            $('.table tbody').on('blur', "tr", function () {

                if ($this.editsMade) {

                    $this.editsMade = false;

                    var $row = $(this);

                    $this.updateRow.call($this, $row);

                }
            });

            //create row
            $('.table tfoot').on('click', ".createSubmit", function () {
                $(this).button("loading");

                var $row = $(this).closest("tfoot tr");
                var url = $this.createURL;
                var $form = $("<form/>");

                $row.find("input[name], select[name]").each(function (i, e) {

                    //cloning selects does not clone selected value, known jquery bug
                    var $element = $(e).clone().val(e.value);

                    $form.append($element);
                });

                $.ajax({
                    type: "POST",
                    url: url,
                    data: $form.serialize(),
                    success: function (data) {

                        $this.resetValidation($row);

                        if (data.Status === 1) {
                            var $newRow = $this.addRow(data);
                            table.DataTable().draw(false);

                            $newRow.animate({
                                backgroundColor: "#dff0d8"
                            }, 1000);

                            setTimeout(function () {
                                $newRow.animate({
                                    backgroundColor: "#ffffff"
                                }, 1000);
                            }, 2000);

                            //reset create row
                            $row.find(".createSubmit").button("reset");

                            $row.find("input").not("[type='checkbox'], [type='button']").val("");
                            if ($row.find("input[type='checkbox']").is(":checked")) {
                                $row.find("input[type='checkbox']").click();
                            }
                            $row.find("input[name='IsHardware']").val("false")
                            $row.find("select").prop('selectedIndex', 0);

                            form.initStyles();

                        } else {
                            $row.find(".createSubmit").button("reset");
                            $row.addClass("danger");

                            $.each(data.Errors, function (i, error) {
                                var name = error.Name.replace("model.", "");

                                var $input = $row.find("input[name='" + name + "']").addClass('input-validation-error');
                                $input.siblings(".field-validation-valid").text(error.Message).addClass('field-validation-error');
                            });
                        }
                    }
                });
            });
        }

        this.updateRow = function ($row) {
            var $this = this;

            var url = $this.editURL + "/" + $row.find("input[name='EquipmentId']").val();
            var $form = $("<form/>");

            $row.find("input[name], select[name]").each(function (i, e) {

                //cloning selects does not clone selected value, known jquery bug
                var $element = $(e).clone().val(e.value);

                $form.append($element);
            });

            $.ajax({
                type: "POST",
                url: url,
                data: $form.serialize(),
                success: function (data) {

                    $this.resetValidation($row);

                    if (data.Status === 1) {
                        table.DataTable().row($row).remove();
                        var $newRow = $row;
                        $this.addRow(data);

                        mslRender.EquipmentNameRender($newRow.find("td").eq(0), data);
                        mslRender.PriorityRender($newRow.find("td").eq(1), data);
                        mslRender.ReleaseDateRender($newRow.find("td").eq(2), data);
                        mslRender.DrawingNumberRender($newRow.find("td").eq(3), data);
                        mslRender.WorkOrderNumberRender($newRow.find("td").eq(4), data);
                        mslRender.QuantityRender($newRow.find("td").eq(5), data);
                        mslRender.ShippingTagNumberRender($newRow.find("td").eq(6), data);
                        mslRender.DescriptionRender($newRow.find("td").eq(7), data);
                        mslRender.UnitWeightRender($newRow.find("td").eq(8), data);
                        mslRender.TotalWeightRender($newRow.find("td").eq(9), data);
                        mslRender.TotalWeightShippedRender($newRow.find("td").eq(10), data);
                        mslRender.ReadyToShipRender($newRow.find("td").eq(11), data);
                        mslRender.ShippedQuantityRender($newRow.find("td").eq(12), data);
                        mslRender.LeftToShipRender($newRow.find("td").eq(13), data);
                        mslRender.FullyShippedRender($newRow.find("td").eq(14), data);
                        mslRender.ShippedFromRender($newRow.find("td").eq(15), data);
                        mslRender.CustomsValueRender($newRow.find("td").eq(16), data);
                        mslRender.SalePriceRender($newRow.find("td").eq(17), data);
                        mslRender.HTSCodeRender($newRow.find("td").eq(18), data);
                        mslRender.CountryOfOriginRender($newRow.find("td").eq(19), data);
                        mslRender.NotesRender($newRow.find("td").eq(20), data);
                        mslRender.SalesOrderNumberRender($newRow.find("td").eq(21), data);
                        mslRender.AutoShipFileRender($newRow.find("td").eq(22), data);
                        mslRender.DeleteRender($newRow.find("td").eq(23), data);
                        mslRender.HasBillOfLadingRender($newRow.find("td").eq(24), data);


                        $newRow.animate({
                            backgroundColor: "#dff0d8"
                        }, 1000);

                        setTimeout(function () {
                            $newRow.animate({
                                backgroundColor: "#ffffff"
                            }, 1000);
                        }, 2000);

                        form.initStyles();
                    } else {

                        $row.addClass("danger");

                        $.each(data.Errors, function (i, error) {
                            var name = error.Name.replace("model.", "");

                            var $input = $row.find("input[name='" + name + "']").addClass('input-validation-error');
                            $input.siblings(".field-validation-valid").text(error.Message).addClass('field-validation-error');
                        });
                        
                    }

                    if ($this.editQueue.length) {
                        $this.updateRow($this.editQueue.pop());
                    }
                }
            });
        }

        this.addRow = function (data) {
            var rowNode = table.DataTable().row.add(data).node();
            return $(rowNode);
        }

        this.resetValidation = function ($row) {

            $row.removeClass("danger");

            $row.find("input[type='checkbox']").removeAttr("data-val-required");
            
            //Removes validation from input-fields
            $row.find('.input-validation-error')
                .addClass('input-validation-valid')
                .removeClass('input-validation-error');
            //Removes validation message after input-fields
            $row.find('.field-validation-error')
                .addClass('field-validation-valid')
                .removeClass('field-validation-error')
                .removeClass('text-danger')
                .text("");

        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});