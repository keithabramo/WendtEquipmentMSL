$(function () {

    var EditorMSL = function () {

        this.editableColumns = [2, 3, 4, 5, 6, 7, 8, 9, 10, 13, 17, 18, 21, 22];
        this.alwaysEditableColumns = [18, 21, 22];
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            timer.startTimer(60 * 15); //15 Mins

            this.initEditor();
            this.initDatatable();

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterWorkInProgress = $('#workInProgressFilter').is(":checked");

                    if (!filterWorkInProgress) {
                        return true;
                    } else {
                        var leftToShip = data[15];
                        var isAssociatedToHardwareKit = data[27];

                        if (leftToShip && parseInt(leftToShip, 10) > 0 && isAssociatedToHardwareKit === "False") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var fullyShipped = data[16];
                        var readyToShip = data[13];
                        var isAssociatedToHardwareKit = data[27];

                        if (fullyShipped === "NO" && readyToShip && parseInt(readyToShip, 10) > 0 && isAssociatedToHardwareKit === "False") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var hardwareFilter = $('#hardwareFilter').is(":checked");

                    if (!hardwareFilter) {
                        return true;
                    } else {
                        var equipment = data[2];

                        if (equipment.toLowerCase() !== "hardware") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var errorFilter = $('#errorFilter').is(":checked");

                    var isDuplicateText = data[29];
                    var hasErrorsText = data[26];


                    if (!errorFilter) {
                        return true;
                    } else {
                        return hasErrorsText == "True" || isDuplicateText == "True";
                    }
                }
            );

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Ready To Ship</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="workInProgressFilter" /> Work In Progress</label>');
            $("div.custom").append('<br/>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="hardwareFilter" /> Hide Hardware</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="errorFilter" />Rows With Errors</label>');

            var $customActions = $("<div class='custom-actions'></div>");
            $customActions.append('<span>Bulk Actions:</span>');
            $customActions.append('<button id="deleteRecords" class="btn btn-primary btn-xs btn-disabled" disabled="disabled" type="button">Delete Checked Lines</button>');
            $customActions.append('<button id="snipTable" class="btn btn-primary btn-xs btn-disabled" disabled="disabled" type="button">Snip Checked Rows For Email</button>');

            $("div.custom").append($customActions);

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

            $(".workorderprice-autocomplete").autocomplete({
                source: "/api/AutocompleteApi/WorkOrderPrice"
            });
        };

        this.initEvents = function () {
            var $this = this;

            $('#readyToShipFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#workInProgressFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#hardwareFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#errorFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = $this.editorMain.editor.create(false);
                form.set('EquipmentName', $row.find("input[name='EquipmentName']").val());
                form.set('PriorityNumber', $row.find("select[name='PriorityNumber']").val());
                form.set('ReleaseDate', $row.find("input[name='ReleaseDate']").val());
                form.set('DrawingNumber', $row.find("textarea[name='DrawingNumber']").val());
                form.set('WorkOrderNumber', $row.find("input[name='WorkOrderNumber']").val());
                form.set('Quantity', $row.find("input[name='Quantity']").val());
                form.set('ShippingTagNumber', $row.find("textarea[name='ShippingTagNumber']").val());
                form.set('Description', $row.find("textarea[name='Description']").val());
                form.set('UnitWeightText', $row.find("input[name='UnitWeight']").val());
                form.set('ReadyToShip', $row.find("input[name='ReadyToShip']").val());
                form.set('ShippedFrom', $row.find("input[name='ShippedFrom']").val());
                form.set('HTSCode', $row.find("input[name='HTSCode']").val());
                form.set('CountryOfOrigin', $row.find("input[name='CountryOfOrigin']").val());
                form.set('Notes', $row.find("textarea[name='Notes']").val());
                form.submit();

                $this.editorMain.datatable.draw();
            });

            $('.table.my-datatable').on('click', ".copy", function () {

                var $row = $(this).closest('tr');
                var $createRow = $(".table.my-datatable tfoot tr");
                var rowData = $this.editorMain.datatable.row($row).data();

                $createRow.find("input[name='EquipmentName']").val(rowData.EquipmentName);
                $createRow.find("select[name='PriorityNumber']").val(rowData.PriorityNumber);
                $createRow.find("input[name='ReleaseDate']").val(moment().format("M/D/YY"));
                $createRow.find("textarea[name='DrawingNumber']").val(rowData.DrawingNumber);
                $createRow.find("input[name='WorkOrderNumber']").val(rowData.WorkOrderNumber);
                $createRow.find("input[name='Quantity']").val(rowData.Quantity);
                $createRow.find("textarea[name='ShippingTagNumber']").val(rowData.ShippingTagNumber);
                $createRow.find("textarea[name='Description']").val(rowData.Description);
                $createRow.find("input[name='UnitWeight']").val(rowData.UnitWeightText);
                $createRow.find("input[name='ReadyToShip']").val(rowData.ReadyToShip);
                $createRow.find("input[name='ShippedFrom']").val(rowData.ShippedFrom);
                $createRow.find("input[name='HTSCode']").val(rowData.HTSCode);
                $createRow.find("input[name='CountryOfOrigin']").val(rowData.CountryOfOrigin);
                $createRow.find("textarea[name='Notes']").val(rowData.Notes);

                window.scrollTo(0, document.body.scrollHeight);
            });


            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var equipmentName = this.field('EquipmentName');
                    var releaseDate = this.field('ReleaseDate');
                    var drawingNumber = this.field('DrawingNumber');
                    var workOrderNumber = this.field('WorkOrderNumber');
                    var shippingTagNumber = this.field('ShippingTagNumber');
                    var description = this.field('Description');
                    var unitWeight = this.field('UnitWeightText');
                    var quantity = this.field('Quantity');
                    var readyToShip = this.field('ReadyToShip');

                    var error = false;
                    if (action === "edit") {

                        if (!equipmentName.isMultiValue()) {
                            if (!equipmentName.val()) {
                                equipmentName.error('The equipment field is required');
                            }
                        }

                        if (!releaseDate.isMultiValue()) {
                            if (!releaseDate.val()) {
                                releaseDate.error('The release date field is required');
                            } else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                                releaseDate.error('The release date must be in the format m/d/yy');
                            }
                        }

                        if (!drawingNumber.isMultiValue()) {
                            if (!drawingNumber.val()) {
                                drawingNumber.error('The drawing # field is required');
                            }
                        }

                        if (!workOrderNumber.isMultiValue()) {
                            if (!workOrderNumber.val()) {
                                workOrderNumber.error('The work order # field is required');
                            }
                        }

                        if (!shippingTagNumber.isMultiValue()) {
                            if (!shippingTagNumber.val()) {
                                shippingTagNumber.error('The ship tag # field is required');
                            }
                        }

                        if (!description.isMultiValue()) {
                            if (!description.val()) {
                                description.error('The description field is required');
                            }
                        }

                        if (!unitWeight.isMultiValue()) {
                            if (!unitWeight.val()) {
                                unitWeight.error('The unit weight field is required');
                            } else if (isNaN(unitWeight.val())) {
                                unitWeight.error('The unit weight is not a valid number');
                            }
                        }

                        if (!quantity.isMultiValue()) {
                            if (!quantity.val()) {
                                quantity.error('The quantity field is required');
                            } else if (isNaN(quantity.val())) {
                                quantity.error('The quantity is not a valid number');
                            }
                        }

                        if (!readyToShip.isMultiValue()) {
                            if (isNaN(readyToShip.val())) {
                                readyToShip.error('The ready to ship field is not a valid number');
                            }
                        }

                        error = this.inError();
                    } else {

                        if (!equipmentName.val()) {
                            $("#EquipmentName").siblings("span").html('The Equipment field is required.').show();
                            error = true;
                        } else {
                            $("#EquipmentName").siblings("span").html('').hide();
                        }

                        if (!releaseDate.val()) {
                            $("#releaseDate").siblings("span").html('The release date field is required').show();
                            error = true;
                        } else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                            $("#releaseDate").siblings("span").html('The release date must be in the format m/d/yy').show();
                            error = true;
                        } else {
                            $("#releaseDate").siblings("span").html('').hide();
                        }
                        if (!drawingNumber.val()) {
                            $("#DrawingNumber").siblings("span").html('The drawing # field is required').show();
                            error = true;
                        } else {
                            $("#DrawingNumber").siblings("span").html('').hide();
                        }
                        if (!workOrderNumber.val()) {
                            $("#WorkOrderNumber").siblings("span").html('The work order # field is required').show();
                            error = true;
                        } else {
                            $("#WorkOrderNumber").siblings("span").html('').hide();
                        }
                        if (!shippingTagNumber.val()) {
                            $("#ShippingTagNumber").siblings("span").html('The ship tag # field is required').show();
                            error = true;
                        } else {
                            $("#ShippingTagNumber").siblings("span").html('').hide();
                        }
                        if (!description.val()) {
                            $("#Description").siblings("span").html('The description field is required').show();
                            error = true;
                        } else {
                            $("#Description").siblings("span").html('').hide();
                        }
                        if (!unitWeight.val()) {
                            $("#UnitWeight").siblings("span").html('The unit weight date field is required').show();
                            error = true;
                        } else if (isNaN(unitWeight.val())) {
                            $("#UnitWeight").siblings("span").html('The unit weight is not a valid number').show();
                            error = true;
                        } else {
                            $("#UnitWeight").siblings("span").html('').hide();
                        }
                        if (!quantity.val()) {
                            $("#Quantity").siblings("span").html('The quantity field is required').show();
                            error = true;
                        } else if (isNaN(quantity.val())) {
                            $("#Quantity").siblings("span").html('The quantity is not a valid number').show();
                            error = true;
                        } else {
                            $("#Quantity").siblings("span").html('').hide();
                        }
                        if (isNaN(readyToShip.val())) {
                            $("#ReadyToShip").siblings("span").html('The ready to ship field is not a valid number').show();
                            error = true;
                        } else {
                            $("#ReadyToShip").siblings("span").html('').hide();
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (error) {
                        return false;
                    }
                }
            });

            this.editorMain.editor.on('preOpen', function (e, mode, action) {

                var editable = true;

                if (action !== "remove") {

                    var rowData = $this.editorMain.datatable.row($this.editorMain.editor.modifier().row).data();
                    var columnIndex = $this.editorMain.editor.modifier().column;

                    if ($.inArray($this.editorMain.editor.modifier().column, $this.alwaysEditableColumns) < 0) {
                        if (rowData.IsAssociatedToHardwareKit || (rowData.FullyShippedText == "YES" && rowData.Quantity != 0)) {
                            editable = false;
                        }
                        else if ($.inArray($this.editorMain.editor.modifier().column, $this.editableColumns) < 0) {
                            editable = false;
                        } else if (rowData.IsHardwareKit && columnIndex == 2) {
                            editable = false;
                        }
                    }
                }

                return editable;

            });

            this.editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();

                // If any of these cells can't be edited, set their values back to original
                $.each(cells, function (i, cell) {
                    var rowIndex = cell[0].index.row;
                    var columnIndex = cell[0].index.column;

                    var rowData = $this.editorMain.datatable.row(rowIndex).data();
                    

                    if ($.inArray(columnIndex, $this.alwaysEditableColumns) < 0) {
                        if (rowData.IsAssociatedToHardwareKit || (rowData.FullyShippedText == "YES" && rowData.Quantity != 0)) {
                            cell[0].set = cell[0].data;
                        }
                        else if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                            cell[0].set = cell[0].data;
                        }
                        else if (rowData.IsHardwareKit && columnIndex == 2) {
                            cell[0].set = cell[0].data;
                        }
                    }
                });
                
            });

            this.editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
                $createRow.find("select").prop('selectedIndex', 0);
                $createRow.find("#ReadyToShip").val("0");
                $createRow.find("#ShippedFrom").val("WENDT");
                $createRow.find(".datePickerTable").datepicker("setDate", new Date());
            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.EquipmentId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }



                if (data.IsAssociatedToHardwareKit || (data.FullyShippedText == "YES" && data.Quantity != 0)) {
                    $(row.node()).addClass('active');
                } else {
                    $(row.node()).removeClass('active');
                }

                $($this.editorMain.datatable.cell(row.index(), 2).node()).attr("class", data.Indicators.EquipmentNameColor);
                $($this.editorMain.datatable.cell(row.index(), 3).node()).attr("class", data.Indicators.PriorityColor);
                $($this.editorMain.datatable.cell(row.index(), 5).node()).attr("class", data.Indicators.DrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), 6).node()).attr("class", data.Indicators.WorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), 8).node()).attr("class", data.Indicators.ShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), 10).node()).attr("class", "text-right " + data.Indicators.UnitWeightColor);
                $($this.editorMain.datatable.cell(row.index(), 13).node()).attr("class", "text-right " + data.Indicators.ReadyToShipColor);
                $($this.editorMain.datatable.cell(row.index(), 14).node()).attr("class", "text-right active " + data.Indicators.ShippedQtyColor);
                $($this.editorMain.datatable.cell(row.index(), 15).node()).attr("class", "text-right active " + data.Indicators.LeftToShipColor);
                $($this.editorMain.datatable.cell(row.index(), 16).node()).attr("class", "active " + data.Indicators.FullyShippedColor);
                $($this.editorMain.datatable.cell(row.index(), 19).node()).attr("class", "text-right active " + data.Indicators.CustomsValueColor);
                $($this.editorMain.datatable.cell(row.index(), 20).node()).attr("class", "text-right active " + data.Indicators.SalePriceColor);
            });

            $(".table").on("mousedown", "td.focus", function (e) {

                setTimeout(function () {
                    if ($(".dt-autofill-select").length) {
                        $this.editorMain.datatable.cell.blur();
                    }
                }, 100);


            });

            $("#deleteRecords").on("click", function () {
                var selectedRows = $this.editorMain.datatable.rows({ selected: true });
                if (selectedRows.length) {

                    var rawIndexes = selectedRows.indexes();

                    var updatedIndexes = [];
                    $.each(selectedRows.data(), function (i, row) {
                        if (!row.HasBillOfLading && !row.IsAssociatedToHardwareKit && !row.IsHardwareKit) {
                            updatedIndexes.push(rawIndexes[i]);
                        }
                    });

                    $this.editorMain.editor
                        .title('Delete records')
                        .buttons('Confirm delete')
                        .message('Are you sure you want to delete these ' + updatedIndexes.length + ' records?')
                        .remove(updatedIndexes);

                } else {
                    $("#deleteRecords").button("reset");

                    main.error("You must select at least one equipment record");
                }
            });

            $("#snipTable").on("click", function () {
                $this.editorMain.datatable.buttons('.buttons-print' ).trigger();
            });

            $('#equipmentAttachmentModal').on('show.bs.modal', function (e) {
                var equipmentId = $(e.relatedTarget).attr("data-equipmentid");

                tableEquipmentAttachment.init(equipmentId);
            });

            this.editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.updateCustomActions();
                }
            });

            this.editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.updateCustomActions();
                }
            });
        };

        this.initEditor = function () {
            this.editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentName" },
                    {
                        name: "PriorityNumber",
                        type: "select",
                        options: priorities,
                        placeholderDisabled: false,
                        placeholder: ""
                    },
                    {
                        name: "ReleaseDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "DrawingNumber", type: "textarea" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" },
                    { name: "TotalWeight", type: "readonly" },
                    { name: "TotalWeightShipped", type: "readonly" },
                    { name: "ReadyToShip" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "LeftToShip", type: "readonly" },
                    { name: "FullyShippedText", type: "readonly" },
                    { name: "ShippedFrom" },
                    { name: "Notes", type: "textarea" },
                    { name: "CustomsValueText" },
                    { name: "SalePriceText" },
                    { name: "HTSCode" },
                    { name: "CountryOfOrigin" },
                    { name: "BillOfLadingNumbers" },
                    {
                        name: "LatestBOLDateShipped",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "EquipmentId", type: "readonly" },
                    { name: "IsHardwareKitText", type: "readonly" },
                    { name: "IsAssociatedToHardwareKitText", type: "readonly" },
                    { name: "HasErrorsText", type: "readonly" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            var hideColumns = false;

            $.ajax({
                url: ROOT_URL + "api/ProjectApi/Get/" + $("#projectId").val(),
                async: false,
                success: function (result) {
                    if (result) {
                        if (!result.IsCustomsProject && !result.IncludeSoftCosts) {
                            hideColumns = true;
                        }
                    }
                }
            });

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Table",
                    dataSrc: ""
                },
                rowId: 'EquipmentId',
                keys: {
                    columns: this.editableColumns
                },
                autoFill: {
                    columns: this.editableColumns
                },
                autoWidth: false,
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }

                    if (data.IsAssociatedToHardwareKit || (data.FullyShippedText == "YES" && data.Quantity != 0)) {
                        $(row).addClass('active');
                    }

                    if (data.IsHardwareKit) {
                        $($this.editorMain.datatable.cell(index, 2).node()).addClass("active");
                    }
                },
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                buttons: [
                    {
                        extend: 'print',
                        title: '',
                        exportOptions: {
                            columns: '.exportable'
                        },
                        customize: function (window) {

                            //open mail to link
                            var link = document.createElement('a');
                            link.href = "mailto:?subject=" + $("#projectNumber").val() + "&body=%0D%0A%0D%0A%0D%0A%0D%0A";

                            document.body.appendChild(link);

                            link.click();

                            setTimeout(function () {

                                //get the table html and put it on the main document
                                var tableHTML = $(window.document.body).find("table")[0].outerHTML;
                                var $div = $("<span id='copyContainer'>").html(tableHTML);
                                $(document.body).append($div);
                                var $table = $("#copyContainer table");

                                //clean the table a bit
                                $table.find(".drawingNumberWidth").css("min-width", "200px");

                                $table.find("th").each(function () {
                                    $(this).text($(this).text().replace(/\s\s+/g, ' '));
                                });

                                window.close();

                                $("#copyModal .modal-body").html('Loading...');
                                $("#copyModal").modal();

                                //html 2 canvas to turn into picture
                                html2canvas(
                                    $table[0],
                                    {
                                        scale: 1
                                    }
                                ).then(function (canvas) {

                                    $div.remove();

                                    $("#copyModal .modal-body").html("<img src='" + canvas.toDataURL() + "'/>");

                                }, function (reason) {
                                    reason.message === 'WHOOPS';
                                });
                            }, 500);
                            
                        },
                        autoPrint: false
                    }
                ],
                order: [[4, 'desc'], [28, 'desc']],
                columnDefs: [
                    {
                        orderable: false,
                        searchable: false,
                        sortable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>";
                        }
                    },
                    {
                        "targets": 1,
                        searchable: false,
                        orderable: false,
                        render: function (datadata, type, row, meta) {
                            return '<span title="Copy Row" class="copy glyphicon glyphicon-duplicate text-primary"></span>';
                        }
                    },
                    {
                        data: "EquipmentName", "targets": 2,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Indicators.EquipmentNameColor);

                            if (rowData.IsAssociatedToHardwareKit) {
                                $cell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + rowData.AssociatedHardwareKitNumber + "</span>");
                            }
                        },
                        className: "equipmentNameWidth"
                    },
                    {
                        data: "PriorityNumber", "targets": 3,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Indicators.PriorityColor);
                        },
                        className: "priorityWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": 4, type: "date",
                        className: "dateWidth"
                    },
                    {
                        data: "DrawingNumber", "targets": 5,
                        className: "drawingNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.DrawingNumberColor);
                        }
                    },
                    {
                        data: "WorkOrderNumber", "targets": 6,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.WorkOrderNumberColor);
                        },
                        className: "workOrderNumberWidth"
                    },
                    {
                        data: "Quantity", "targets": 7, className: "text-right quantityWidth"
                    },
                    {
                        data: "ShippingTagNumber", "targets": 8,
                        className: "shippingTagNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippingTagNumberColor);
                        }
                    },
                    {
                        data: "Description", "targets": 9,
                        className: "descriptionWidth"
                    },
                    {
                        data: "UnitWeightText", "targets": 10, className: "text-right unitWeightWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.UnitWeightColor);
                        }
                    },
                    {
                        data: "TotalWeight", "targets": 11, className: "active text-right totalWeightWidth"
                    },
                    {
                        data: "TotalWeightShipped", "targets": 12, className: "active text-right totalWeightShippedWidth"
                    },
                    {
                        data: "ReadyToShip", "targets": 13, className: "text-right readyToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedQuantity", "targets": 14, className: "active text-right shippedQuantityWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "LeftToShip", "targets": 15, className: "active text-right leftToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "FullyShippedText", "targets": 16, className: "active fullyShippedWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.FullyShippedColor);
                        }
                    },
                    {
                        data: "ShippedFrom", "targets": 17,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "Notes", "targets": 18,
                        className: "notesWidth"
                    },
                    {
                        data: "CustomsValueText", "targets": 19, className: "active text-right customsValueWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.CustomsValueColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$'),
                        visible: !hideColumns
                    },
                    {
                        data: "SalePriceText", "targets": 20, className: "active text-right salePriceWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.SalePriceColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$'),
                        visible: !hideColumns
                    },
                    {
                        data: "HTSCode", "targets": 21,
                        className: "htsCodeWidth",
                        visible: !hideColumns
                    },
                    {
                        data: "CountryOfOrigin", "targets": 22,
                        className: "countryOfOriginWidth",
                        visible: !hideColumns
                    },
                    {
                        data: "BillOfLadingNumbers", "targets": 23,
                        className: "billOfLadingNumbersWidth"
                    },
                    {
                        data: "LatestBOLDateShipped", "targets": 24, type: "date",
                        className: "latestBOLDateShippedWidth"
                    },
                    {
                        "targets": 25,
                        searchable: false,
                        orderable: false,
                        render: function (datadata, type, row, meta) {
                            return '<a data-equipmentid="' + row.EquipmentId + '" href="javascript:void(0);" class="attachments" data-toggle="modal" data-toggle="modal" data-target="#equipmentAttachmentModal">Attachments</a>';
                        }
                    },
                    {
                        data: "HasErrorsText",
                        "targets": 26,
                        sortable: false,
                        visible: false
                    },
                    {
                        data: "IsAssociatedToHardwareKitText",
                        "targets": 27,
                        sortable: false,
                        visible: false
                    },
                    {
                        data: "EquipmentId",
                        "targets": 28,
                        searchable: false,
                        visible: false
                    },
                    {
                        data: "IsDuplicateText",
                        "targets": 29,
                        sortable: false,
                        visible: false
                    }
                ]
            });
        };

        this.updateCustomActions = function () {
            var selectedRowsCount = this.editorMain.datatable.rows({ selected: true }).indexes().length;

            if (selectedRowsCount) {
                $(".custom-actions button, .custom-actions a").removeAttr("disabled");
            } else {
                $(".custom-actions button, .custom-actions a").attr("disabled", "disabled");
            }
        };

        this.initStyles();
        this.initEvents();

    };

    editorMSL = new EditorMSL();

});