﻿$(function () {

    var EditorMSL = function () {

        this.editableColumns = [0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 15, 18, 19, 20];


        this.initStyles = function () {
            var $this = this;

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

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Work In Progress</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="hardwareFilter" /> Hide Hardware</label>');
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

            $(".table tfoot").on("click", function () {
                editorMain.datatable.cell.blur();
            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = editorMain.editor.create(false);
                form.set('EquipmentName', $row.find("input[name='EquipmentName']").val());
                form.set('Priority', $row.find("select[name='Priority']").val());
                form.set('ReleaseDate', $row.find("input[name='ReleaseDate']").val());
                form.set('DrawingNumber', $row.find("input[name='DrawingNumber']").val());
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
                        success: function(response) {
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

            editorMain.editor.on('preOpen', function (e, mode, action) {
                var rowData = editorMain.datatable.row(editorMain.editor.modifier().row).data();

                var editable = true;

                if ($.inArray(editorMain.editor.modifier().column, $this.editableColumns) < 0 && action !== "remove") {
                    editable = false;
                } else if (rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit) {
                    editable = false;
                }

                return editable;

            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                    var row = editorMain.datatable.row("#" + data.EquipmentId);

                    if (data.IsDuplicate) {
                        $(row.node()).attr("class", 'warning');
                    } else {
                        $(row.node()).removeClass('warning');
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
                        name: "Priority",
                        type: "select",
                        options: priorities
                    },
                    { name: "ReleaseDate", type: "datetime", format: "MM/DD/YYYY" },
                    { name: "DrawingNumber" },
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
                    { name: "EquipmentId", type: "readonly" }
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
                    if (data.IsAssociatedToHardwareKit || data.IsHardwareKit) {
                        $(row).addClass('active');
                    }
                },
                order: [[2, 'desc']],
                columnDefs: [
                    {
                        data: "EquipmentName", "targets": 0,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Indicators.EquipmentNameColor);

                            if (rowData.IsAssociatedToHardwareKit) {
                                $cell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + rowData.AssociatedHardwareKitNumber + "</span>");
                            }
                        }
                    },
                    { data: "Priority", "targets": 1 },
                    { data: "ReleaseDate", "targets": 2, },
                    { data: "DrawingNumber", "targets": 3 },
                    {
                        data: "WorkOrderNumber", "targets": 4,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.WorkOrderNumberColor);
                        }
                    },
                    { data: "Quantity", "targets": 5, className: "text-right" },
                    { data: "ShippingTagNumber", "targets": 6 },
                    { data: "Description", "targets": 7 },
                    {
                        data: "UnitWeightText", "targets": 8, className: "text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.UnitWeightColor);
                        }
                    },
                    { data: "TotalWeight", "targets": 9, className: "active text-right" },
                    { data: "TotalWeightShipped", "targets": 10, className: "active text-right" },
                    {
                        data: "ReadyToShipText", "targets": 11, className: "text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedQuantity", "targets": 12, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "LeftToShip", "targets": 13, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "FullyShippedText", "targets": 14, className: "active",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.FullyShippedColor);
                        }
                     },
                    { data: "ShippedFrom", "targets": 15 },
                    {
                        data: "CustomsValueText", "targets": 16, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.CustomsValueColor);
                        }
                    },
                    {
                        data: "SalePriceText", "targets": 17, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.SalePriceColor);
                        }
                    },
                    { data: "HTSCode", "targets": 18 },
                    { data: "CountryOfOrigin", "targets": 19 },
                    { data: "Notes", "targets": 20 },
                    {
                        "targets": 21, 
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    },
                    {
                        "targets": 22,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return row.HasBillOfLading ? '<span class="expand glyphicon glyphicon-plus text-primary"></span>' : '';
                        }
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    }

    editorMSL = new EditorMSL();

});