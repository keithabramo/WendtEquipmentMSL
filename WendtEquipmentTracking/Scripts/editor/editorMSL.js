$(function () {

    var EditorMSL = function () {

        this.editableColumns = [1, 2, 3, 4,5, 6, 7, 8, 9, 12, 16, 19, 20, 21];


        this.initStyles = function () {
            var $this = this;

            timer.startTimer(60 * 15); //15 Mins

            this.initEditor();
            this.initDatatable();

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var leftToShip = data[13];
                        var isAssociatedToHardwareKit = data[25];

                        if (leftToShip && parseInt(leftToShip, 10) > 0 && !isAssociatedToHardwareKit) {
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
                        var equipment = data[0];

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

                    if (!errorFilter) {
                        return true;
                    } else {
                        return data[23] == "True";
                    }
                }
            );

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Work In Progress</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="hardwareFilter" /> Hide Hardware</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="errorFilter" />Show Me All Rows With Errors</label>');
            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

        }

        this.initEvents = function () {
            var $this = this;

            $('#readyToShipFilter').on("change", function () {
                editorMain.datatable.draw();
            });

            $('#hardwareFilter').on("change", function () {
                editorMain.datatable.draw();
            });

            $('#errorFilter').on("change", function () {
                editorMain.datatable.draw();
            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = editorMain.editor.create(false);
                form.set('EquipmentName', $row.find("input[name='EquipmentName']").val());
                form.set('PriorityNumber', $row.find("select[name='PriorityNumber']").val());
                form.set('ReleaseDate', $row.find("input[name='ReleaseDate']").val());
                form.set('DrawingNumber', $row.find("textarea[name='DrawingNumber']").val());
                form.set('WorkOrderNumber', $row.find("input[name='WorkOrderNumber']").val());
                form.set('Quantity', $row.find("input[name='Quantity']").val());
                form.set('ShippingTagNumber', $row.find("textarea[name='ShippingTagNumber']").val());
                form.set('Description', $row.find("textarea[name='Description']").val());
                form.set('UnitWeightText', $row.find("input[name='UnitWeight']").val());
                form.set('ReadyToShipText', $row.find("input[name='ReadyToShip']").val());
                form.set('ShippedFrom', $row.find("input[name='ShippedFrom']").val());
                form.set('HTSCode', $row.find("input[name='HTSCode']").val());
                form.set('CountryOfOrigin', $row.find("input[name='CountryOfOrigin']").val());
                form.set('Notes', $row.find("textarea[name='Notes']").val());
                form.submit();

                editorMain.datatable.draw();
            });

            //show hide child row
            $('.table.my-datatable').on('click', ".expand", function () {

                var $row = $(this).closest('tr');
                var datatableRow = editorMain.datatable.row($row);

                if (datatableRow.child.isShown()) {
                    // This row is already open - close it
                    datatableRow.child.hide();
                }
                else {
                    // Open this row
                    $.get({
                        url: ROOT_URL + "Equipment/BOLsAssociatedToEquipment/",
                        data: { id: datatableRow.id() },
                        success: function (response) {
                            datatableRow.child(response).show();
                        }
                    });
                }


                var $icon = $(this);

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });

            $('.table.my-datatable').on('click', ".copy", function () {

                var $row = $(this).closest('tr');
                var $createRow = $(".table.my-datatable tfoot tr");
                var rowData = editorMain.datatable.row($row).data();

                $createRow.find("input[name='EquipmentName']").val(rowData.EquipmentName);
                $createRow.find("select[name='PriorityNumber']").val(rowData.PriorityNumber);
                $createRow.find("input[name='ReleaseDate']").val(rowData.ReleaseDate);
                $createRow.find("textarea[name='DrawingNumber']").val(rowData.DrawingNumber);
                $createRow.find("input[name='WorkOrderNumber']").val(rowData.WorkOrderNumber);
                $createRow.find("input[name='Quantity']").val(rowData.Quantity);
                $createRow.find("textarea[name='ShippingTagNumber']").val(rowData.ShippingTagNumber);
                $createRow.find("textarea[name='Description']").val(rowData.Description);
                $createRow.find("input[name='UnitWeight']").val(rowData.UnitWeightText);
                $createRow.find("input[name='ReadyToShip']").val(rowData.ReadyToShipText);
                $createRow.find("input[name='ShippedFrom']").val(rowData.ShippedFrom);
                $createRow.find("input[name='HTSCode']").val(rowData.HTSCode);
                $createRow.find("input[name='CountryOfOrigin']").val(rowData.CountryOfOrigin);
                $createRow.find("textarea[name='Notes']").val(rowData.Notes);

                window.scrollTo(0, document.body.scrollHeight);
            });


            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var equipmentName = this.field('EquipmentName');
                    var releaseDate = this.field('ReleaseDate');
                    var drawingNumber = this.field('DrawingNumber');
                    var workOrderNumber = this.field('WorkOrderNumber');
                    var shippingTagNumber = this.field('ShippingTagNumber');
                    var description = this.field('Description');
                    var unitWeight = this.field('UnitWeightText');
                    var quantity = this.field('Quantity');
                    var readyToShip = this.field('ReadyToShipText');

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
                            } else if (!moment(releaseDate.val(), 'MM/DD/YYYY', true).isValid()) {
                                releaseDate.error('The release date must be in the format mm/dd/yyyy');
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

                        var error = this.inError();
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
                        } else if (!moment(releaseDate.val(), 'MM/DD/YYYY', true).isValid()) {
                            $("#releaseDate").siblings("span").html('The release date must be in the format mm/dd/yyyy').show();
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

            editorMain.editor.on('preOpen', function (e, mode, action) {
                var rowData = editorMain.datatable.row(editorMain.editor.modifier().row).data();
                var columnIndex = editorMain.editor.modifier().column;

                var editable = true;

                if (rowData.IsAssociatedToHardwareKit || (rowData.FullyShippedText == "YES" && rowData.Quantity != 0)) {
                    editable = false;
                }

                if ($.inArray(editorMain.editor.modifier().column, $this.editableColumns) < 0 && action !== "remove") {
                    editable = false;
                } else if ((rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit) && (columnIndex == 0 || columnIndex == 5)) {
                    editable = false;
                }

                return editable;

            });

            editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();
            });

            editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
                $createRow.find("select").prop('selectedIndex', 0);
                $createRow.find("#ReadyToShip").val("0");
                $createRow.find("#ShippedFrom").val("WENDT");
            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                var row = editorMain.datatable.row("#" + data.EquipmentId);

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

                $(editorMain.datatable.cell(row.index(), 0).node()).attr("class", data.Indicators.EquipmentNameColor);
                $(editorMain.datatable.cell(row.index(), 4).node()).attr("class", data.Indicators.WorkOrderNumberColor);
                $(editorMain.datatable.cell(row.index(), 8).node()).attr("class", "text-right " + data.Indicators.UnitWeightColor);
                $(editorMain.datatable.cell(row.index(), 11).node()).attr("class", "text-right " + data.Indicators.ReadyToShipColor);
                $(editorMain.datatable.cell(row.index(), 12).node()).attr("class", "text-right active " + data.Indicators.ShippedQtyColor);
                $(editorMain.datatable.cell(row.index(), 13).node()).attr("class", "text-right active " + data.Indicators.LeftToShipColor);
                $(editorMain.datatable.cell(row.index(), 14).node()).attr("class", "active " + data.Indicators.FullyShippedColor);
                $(editorMain.datatable.cell(row.index(), 16).node()).attr("class", "text-right active " + data.Indicators.CustomsValueColor);
                $(editorMain.datatable.cell(row.index(), 17).node()).attr("class", "text-right active " + data.Indicators.SalePriceColor);
            });

            $(".table").on("mousedown", "td.focus", function (e) {

                setTimeout(function () {
                    if ($(".dt-autofill-select").length) {
                        editorMain.datatable.cell.blur();
                    }
                }, 100);
                
                
            });
        }

        this.initEditor = function () {
            editorMain.initEditor({
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
                        options: priorities
                    },
                    { name: "ReleaseDate", type: "datetime", format: "MM/DD/YYYY" },
                    { name: "DrawingNumber", type: "textarea" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" },
                    { name: "TotalWeight", type: "readonly" },
                    { name: "TotalWeightShipped", type: "readonly" },
                    { name: "ReadyToShipText" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "LeftToShip", type: "readonly" },
                    { name: "FullyShippedText", type: "readonly" },
                    { name: "ShippedFrom" },
                    { name: "CustomsValueText" },
                    { name: "SalePriceText" },
                    { name: "HTSCode" },
                    { name: "CountryOfOrigin" },
                    { name: "Notes", type: "textarea" },
                    { name: "EquipmentId", type: "readonly" },
                    { name: "IsHardwareKitText", type: "readonly" },
                    { name: "IsAssociatedToHardwareKitText", type: "readonly" },
                    { name: "HasErrorsText", type: "readonly" }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;


            editorMain.initDatatable({
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
                        $(editorMain.datatable.cell(index, 0).node()).addClass("active");
                        $(editorMain.datatable.cell(index, 5).node()).addClass("active");
                    }
                },
                order: [[3, 'desc']],
                columnDefs: [
                    {
                        "targets": 0,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return '<span class="copy glyphicon glyphicon-duplicate text-primary"></span>';
                        }
                    },
                    {
                        data: "EquipmentName", "targets": 1,
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
                        data: "PriorityNumber", "targets": 2,
                        className: "priorityWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": 3,
                        className: "releaseDateWidth"
                    },
                    {
                        data: "DrawingNumber", "targets": 4,
                        className: "drawingNumberWidth"
                    },
                    {
                        data: "WorkOrderNumber", "targets": 5,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.WorkOrderNumberColor);
                        },
                        className: "workOrderNumberWidth"
                    },
                    {
                        data: "Quantity", "targets": 6, className: "text-right quantityWidth"
                    },
                    {
                        data: "ShippingTagNumber", "targets": 7,
                        className: "shippingTagNumberWidth"
                    },
                    {
                        data: "Description", "targets": 8,
                        className: "descriptionWidth"
                    },
                    {
                        data: "UnitWeightText", "targets": 9, className: "text-right unitWeightWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.UnitWeightColor);
                        }
                    },
                    {
                        data: "TotalWeight", "targets": 10, className: "active text-right totalWeightWidth"
                    },
                    {
                        data: "TotalWeightShipped", "targets": 11, className: "active text-right totalWeightShippedWidth"
                    },
                    {
                        data: "ReadyToShipText", "targets": 12, className: "text-right readyToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedQuantity", "targets": 13, className: "active text-right shippedQuantityWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "LeftToShip", "targets": 14, className: "active text-right leftToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "FullyShippedText", "targets": 15, className: "active fullyShippedWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.FullyShippedColor);
                        }
                    },
                    {
                        data: "ShippedFrom", "targets": 16,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "CustomsValueText", "targets": 17, className: "active text-right customsValueWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.CustomsValueColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$')
                    },
                    {
                        data: "SalePriceText", "targets": 18, className: "active text-right salePriceWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.SalePriceColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$')
                    },
                    {
                        data: "HTSCode", "targets": 19,
                        className: "htsCodeWidth"
                    },
                    {
                        data: "CountryOfOrigin", "targets": 20,
                        className: "countryOfOriginWidth"
                    },
                    {
                        data: "Notes", "targets": 21,
                        className: "notesWidth"
                    },
                    {
                        "targets": 22,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    },
                    {
                        "targets": 23,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return row.HasBillOfLading ? '<span class="expand glyphicon glyphicon-plus text-primary"></span>' : '';
                        }
                    },
                    {
                        data: "HasErrorsText",
                        "targets": 24,
                        searchable: false,
                        sortable: false,
                        visible: false
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
        
    }

    editorMSL = new EditorMSL();

});