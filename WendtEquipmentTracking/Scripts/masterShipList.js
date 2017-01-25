$(function () {

    var MasterShipList = function () {
        $.ajaxSetup({ cache: false });

        this.editsMade = false;
        this.editQueue = [];
        this.createURL = ROOT_URL + "Equipment/Create";
        this.editURL = ROOT_URL + "Equipment/Edit";
        this.chunkURL = ROOT_URL + "Equipment/Chunk";

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

            this.loadChunk(50);
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
                    
                    var rowIndex = cell[0].index.row;
                    var $row = $(table.DataTable().row(rowIndex).node());

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
                            $row.find(".createSubmit").button("reset");
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

        this.updateRow = function ($row) {
            var $this = this;

            var url = $this.editURL + "/" + $row.find("input[name='item.EquipmentId']").val();
            var $form = $("<form/>");

            $row.find("input[name], select[name]").each(function (i, e) {

                //cloning selects does not clone selected value, known jquery bug
                var $element = $(e).clone().val(e.value);
                $element.attr("name", $element.attr("name").replace("item.", ""));

                $form.append($element);
            });

            $.ajax({
                type: "POST",
                url: url,
                data: $form.serialize(),
                success: function (data) {

                    table.DataTable().row($row).remove().draw();
                    var rowNode = table.DataTable().row.add($(data)[0]).draw().node();
                    var $newRow = $(rowNode);

                    if (!$newRow.hasClass("danger")) {
                        $newRow.animate({
                            backgroundColor: "#dff0d8"
                        }, 1000);

                        setTimeout(function () {
                            $newRow.animate({
                                backgroundColor: "#ffffff"
                            }, 1000);
                        }, 2000);
                    }

                    form.initStyles();

                    if ($this.editQueue.length) {
                        $this.updateRow($this.editQueue.pop());
                    }
                }
            });
        }

        this.loadChunk = function (skip) {
            var $this = this;
            var url = this.chunkURL;
            var take = 4000;

            $.ajax({
                type: "GET",
                url: url,
                data: { skip: skip, take: take },
                dataType: "html",
                success: function (data) {
                    var rows = $(data);
                    if (rows.length) {

                        table.DataTable().rows.add(rows).draw();
                        
                        form.initStyles();
                        
                        skip += take;

                        $this.loadChunk.call($this, skip);

                    } else {
                        waitingDialog.hide();
                    }
                }
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});