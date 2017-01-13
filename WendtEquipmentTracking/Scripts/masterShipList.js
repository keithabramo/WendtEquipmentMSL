$(function () {

    var MasterShipList = function () {

        this.editsMade = false;
        this.createURL = "/Equipment/Create";
        this.editURL = "/Equipment/Edit";

        this.initStyles = function () {
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var isHardware = data[1];
                        var releaseDate = data[4];
                        var leftToShip = data[15];

                        if (isHardware === "False" && releaseDate && leftToShip && parseInt(leftToShip, 10) > 0) {
                            return true;
                        }
                    }

                    return false;
                }
            );

            table.DataTable()
                .order([2, 'asc'])
                .draw();

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Equipment Released</label>');
        }

        this.initEvents = function () {
            var $this = this;

            $('.table').on('change', "> tbody > tr input, > tbody > tr select", function () {
                $this.editsMade = true;
            });

            $('#readyToShipFilter').on("change", function () {
                table.DataTable().draw();
            });

            $('.table').on('change', "input[name='IsHardware']", function () {
                var isHardware = $(this).is(":checked");
                var $equipmentNameInput = $(this).closest("tr").find("input[name='EquipmentName']");

                if (isHardware) {
                    $equipmentNameInput.val("HARDWARE").attr("readonly", "readonly");
                } else {
                    $equipmentNameInput.removeAttr("readonly");
                }
            });

            $('.table tbody').on('blur', "tr", function () {

                if ($this.editsMade) {

                    $this.editsMade = false;

                    var $row = $(this);
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
                            var $newRow = $(data);
                            $row.replaceWith($newRow);


                            if (!$newRow.hasClass("danger")) {
                                $newRow.animate({
                                    backgroundColor: "#dff0d8"
                                }, 1000);

                                setTimeout(function () {
                                    $newRow.animate({
                                        backgroundColor: "#ffffff"
                                    }, 1000, function () {
                                        table.DataTable().draw();
                                    });
                                }, 2000);
                            }

                            form.initStyles();
                        }
                    });
                }


            });

            $('.table tfoot').on('click', ".createSubmit", function () {


                var $row = $(this).closest("tfoot tr");
                var url = $this.createURL;
                var $form = $("<form/>");

                $row.find("input[name], select[name]").each(function (i, e) {
                    var $element = $(e).clone();

                    $form.append($element);
                });

                $.ajax({
                    type: "POST",
                    url: url,
                    data: $form.serialize(),
                    success: function (data) {
                         
                        if (!$(data).hasClass("danger")) {
                            var rowNode = table.DataTable().row.add($(data)[0]).draw().node();
                            var $newRow = $(rowNode);
                            
                            $newRow.animate({
                                backgroundColor: "#dff0d8"
                            }, 1000);

                            setTimeout(function () {
                                $newRow.animate({
                                    backgroundColor: "#ffffff"
                                }, 1000);
                            }, 2000);

                            //reset create row
                            $row.removeClass("danger").addClass("warning");

                            $row.find("input").not("[type='checkbox'], [type='button']").val("");
                            $row.find("input[type='checkbox']").removeAttr("data-val-required");
                            if ($row.find("input[type='checkbox']").is(":checked")) {
                                $row.find("input[type='checkbox']").click();
                            }
                            $row.find("input[name='IsHardware']").val("false")
                            $row.find("select").prop('selectedIndex', 0);

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

                        } else {
                            $row.replaceWith($(data));
                        }

                        form.initStyles();
                    }
                });
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});