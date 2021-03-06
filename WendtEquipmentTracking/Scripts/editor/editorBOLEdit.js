﻿$(function () {

    var EditorBOL = function () {

        this.canSubmit = false;
        this.lockInterval;
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            $.validator.unobtrusive.parse('#BOLForm');

            this.lock();

            this.lockInterval = setInterval(this.lock, 30 * 1000); //run every 30 seconds

            this.initEditor();
            this.initDatatable();

            $.ajax({
                url: ROOT_URL + "api/AutocompleteApi/Vendor/",
                success: function (results) {

                    var shippedFromResults = ["WENDT"].concat(results);
                    var shippedToResults = [$("#projectNumber").val()].concat(results);

                    $(".shippedfrom.vendor-autocomplete").autocomplete({
                        minLength: 0,
                        source: shippedFromResults
                    }).focus(function () {
                        $(this).data("uiAutocomplete").search($(this).val());
                    });

                    $(".shippedto.vendor-autocomplete").autocomplete({
                        minLength: 0,
                        source: shippedToResults
                    }).focus(function () {
                        $(this).data("uiAutocomplete").search($(this).val());
                    });

                }
            });
        };

        this.initEvents = function () {
            var $this = this;

            window.addEventListener("beforeunload", function (event) {
                $this.unlock.call($this);
            });

            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var quantity = this.field('Quantity');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!quantity.isMultiValue()) {
                        if (!quantity.val()) {
                            quantity.error('A quantity is required');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }

                data.doSubmit = $this.canSubmit;
                if ($this.canSubmit) {


                    data.BillOfLadingNumber = $("#BillOfLadingNumber").val();
                    data.Carrier = $("#Carrier").val();
                    data.DateShipped = $("#DateShipped").val();
                    data.FreightTerms = $("#FreightTerms").val();
                    data.ToStorage = $("#ToStorage").is(":checked");
                    data.TrailerNumber = $("#TrailerNumber").val();
                    data.ShippedFrom = $("#ShippedFrom").val();
                    data.ShippedTo = $("#ShippedTo").val();
                    data.BillOfLadingId = $("#BillOfLadingId").val() || "";
                }

            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.Equipment.EquipmentId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

                if (data.Quantity > 0) {
                    row.select();
                } else {
                    row.deselect();
                }

            });

            this.editorMain.datatable.on("draw", function () {
                $this.updateSelectedDisplay();
            });

            this.editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $.each(indexes, function () {
                        if (dt.cell(this, 1).data() === 0) {
                            var leftToShip = dt.cell(this, 13).data();
                            dt.cell(this, 1).data(leftToShip);
                        }
                    });

                    $this.updateSelectedDisplay();
                }
            });

            this.editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.updateSelectedDisplay();
                }
            });

            $("#Save").on("click", function () {

                if ($("#BOLForm").valid()) {
                    $this.canSubmit = true;

                    var selectedRows = $this.editorMain.datatable.rows({ selected: true }).indexes();
                    if (selectedRows.length) {

                        $this.editorMain.editor.edit(
                            selectedRows, false
                        ).submit(function () {
                            $this.canSubmit = false;
                            $("#Save").button("reset");
                            location.href = ROOT_URL + "BillOfLading/?ajaxSuccess=true";
                        }, function () {
                            $this.canSubmit = false;
                            $("#Save").button("reset");

                            main.error("There was an error whill trying to save this bill of lading");
                        });
                    } else {
                        $("#Save").button("reset");

                        main.error("You must select at least one equipment record");
                    }
                } else {
                    $("#Save").button("reset");

                    main.error("Please address the validation errors before saving");
                }
            });

        };

        this.initEditor = function () {

            this.editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'Equipment.EquipmentId',
                fields: [
                    { name: "BillOfLadingEquipmentId" },
                    { name: "Quantity" },
                    { name: "Equipment.EquipmentName" },
                    { name: "Equipment.PriorityNumber" },
                    {
                        name: "Equipment.ReleaseDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "Equipment.DrawingNumber" },
                    { name: "Equipment.WorkOrderNumber" },
                    { name: "Equipment.Quantity" },
                    { name: "Equipment.ShippingTagNumber" },
                    { name: "Equipment.Description" },
                    { name: "Equipment.UnitWeightText" },
                    { name: "Equipment.TotalWeight" },
                    { name: "Equipment.TotalWeightShipped" },
                    { name: "Equipment.ReadyToShip" },
                    { name: "Equipment.ShippedQuantity" },
                    { name: "Equipment.LeftToShip" },
                    { name: "Equipment.FullyShippedText" },
                    { name: "Equipment.ShippedFrom" },
                    { name: "Equipment.CustomsValueText" },
                    { name: "Equipment.SalePriceText" },
                    {
                        name: "Equipment.HTSCode",
                        type: "autoComplete",
                        openOnFocus: true,
                        opts: {
                            source: htsCodes,
                            minLength: 0,
                            focus: function (event, ui) {
                                $(this).val(ui.item.value);
                            }
                        }
                    },
                    { name: "Equipment.CountryOfOrigin" },
                    { name: "Equipment.Notes" },
                    { name: "Equipment.EquipmentId" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/EditTable",
                    dataSrc: "",
                    data: {
                        billOfLadingId: $("#BillOfLadingId").val()
                    }
                },
                rowId: 'Equipment.EquipmentId',
                order: [[1, 'desc'], [4, 'desc']],
                autoWidth: false,
                columnDefs: [
                    {
                        data: "Equipment.EquipmentId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>"
                        }
                    },
                    {
                        data: "Quantity", "targets": 1,
                        className: "text-right quantityWidth"
                    },
                    {
                        data: "Equipment.EquipmentName", "targets": 2,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Equipment.Indicators.EquipmentNameColor);

                            if (rowData.Equipment.IsAssociatedToHardwareKit) {
                                $cell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + rowData.Equipment.AssociatedHardwareKitNumber + "</span>");
                            }
                        },
                        className: "equipmentNameWidth active"
                    },
                    {
                        data: "Equipment.PriorityNumber", "targets": 3,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Equipment.Indicators.PriorityColor);
                        },
                        className: "priorityWidth active"
                    },
                    {
                        data: "Equipment.ReleaseDate", "targets": 4, type: "date",
                        className: "dateWidth active"
                    },
                    {
                        data: "Equipment.DrawingNumber", "targets": 5,
                        className: "drawingNumberWidth active"
                    },
                    {
                        data: "Equipment.WorkOrderNumber", "targets": 6,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.WorkOrderNumberColor);
                        },
                        className: "workOrderNumberWidth active"
                    },
                    {
                        data: "Equipment.Quantity", "targets": 7, className: "text-right quantityWidth active"
                    },
                    {
                        data: "Equipment.ShippingTagNumber", "targets": 8,
                        className: "shippingTagNumberWidth active"
                    },
                    {
                        data: "Equipment.Description", "targets": 9,
                        className: "descriptionWidth active"
                    },
                    {
                        data: "Equipment.UnitWeightText", "targets": 10, className: "text-right unitWeightWidth active",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.UnitWeightColor);
                        }
                    },
                    {
                        data: "Equipment.TotalWeight", "targets": 11, className: "active text-right totalWeightWidth"
                    },
                    {
                        data: "Equipment.TotalWeightShipped", "targets": 12, className: "active text-right totalWeightShippedWidth"
                    },
                    {
                        data: "Equipment.ReadyToShip", "targets": 13, className: "text-right readyToShipWidth active",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "Equipment.ShippedQuantity", "targets": 14, className: "active text-right shippedQuantityWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "Equipment.LeftToShip", "targets": 15, className: "active text-right leftToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "Equipment.FullyShippedText", "targets": 16, className: "active fullyShippedWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.FullyShippedColor);
                        }
                    },
                    {
                        data: "Equipment.ShippedFrom", "targets": 17,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "Equipment.CustomsValueText", "targets": 18, className: "active text-right customsValueWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.CustomsValueColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$')
                    },
                    {
                        data: "Equipment.SalePriceText", "targets": 19, className: "active text-right salePriceWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Equipment.Indicators.SalePriceColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$')
                    },
                    {
                        data: "Equipment.HTSCode", "targets": 20,
                        className: "htsCodeWidth"
                    },
                    {
                        data: "Equipment.CountryOfOrigin", "targets": 21,
                        className: "countryOfOriginWidth"
                    },
                    {
                        data: "Equipment.Notes", "targets": 22,
                        className: "notesWidth active"
                    }
                ],
                initComplete: function (settings, json) {

                    $this.editorMain.datatable.rows()
                        .every(function (rowIdx, tableLoop, rowLoop) {
                            var data = this.data();
                            if (data.BillOfLadingEquipmentId > 0) {
                                $this.editorMain.datatable.row(rowIdx).select();
                            }
                        });

                },
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }


                },
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                keys: {
                    columns: [1, 17, 20, 21]
                },
                autoFill: {
                    editor: null,
                    columns: [1, 17, 20, 21]
                }
            });
        }

        this.updateSelectedDisplay = function () {
            var quantity = 0;
            this.editorMain.datatable.rows({ selected: true })
                .every(function (rowIdx, tableLoop, rowLoop) {
                    var data = this.data();
                    quantity += data.Quantity;
                });

            if ($(".select-info").length) {
                $(".select-info").each(function () {
                    $(this).find(".select-item").eq(1).html("- Quantity: " + quantity);
                });
            }
        };

        this.lock = function () {
            $.get(ROOT_URL + "Api/BillOfLadingApi/Lock", { id: $("#BillOfLadingId").val() });
        };

        this.unlock = function () {
            clearInterval(this.lockInterval);
            $.get(ROOT_URL + "Api/BillOfLadingApi/Unlock", { id: $("#BillOfLadingId").val() });
        };

        this.initStyles();
        this.initEvents();
    };

    editorBOL = new EditorBOL();

});