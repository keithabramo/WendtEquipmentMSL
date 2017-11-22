﻿$(function () {

    ImportRawEquipmentTable = function () {

        this.canSubmit = false;

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;


            editorMain.editor.on('preSubmit', function (e, data, action) {
                data.doSubmit = $this.canSubmit;
            });

            editorMain.datatable.on('autoFill', function (e, datatable, cells) {
                $this.validationErrors();
            });

            editorMain.editor.on('submitComplete', function () {
                $this.validationErrors();
            });

            $("#import").on("click", function () {
                var selectedRows = editorMain.datatable.rows({ selected: true }).indexes();
                if (selectedRows.length) {
                    if (!$this.validationErrors()) {
                        $this.canSubmit = true;

                        editorMain.editor.edit(
                            editorMain.datatable.rows({ selected: true }).indexes(), false
                        ).submit(function () {
                            $this.canSubmit = false;
                            location.href = ROOT_URL + "Equipment/?ajaxSuccess=true"
                        }, function () {
                            $this.canSubmit = false;
                            $("#import").button("reset");

                            main.error("There was an error whill trying to import");
                        });
                    } else {
                        $("#import").button("reset");

                        main.error("Please address all rows with validation errors before importing.")
                    }
                } else {
                    $("#import").button("reset");

                    main.error("You must select at least one record");
                }
            });

            editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.validationErrors();
                }
            });

            editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.clearValidation();
                }
            });
        }

        this.initEditor = function () {

            editorMain.initEditor({
                formOptions: {},
                ajax: {
                    url: ROOT_URL + "api/ImportApi/RawEquipmentEditor",
                    dataSrc: ""
                },
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentId", type: "readonly" },
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
                    { name: "ReadyToShip" },
                    { name: "ShippedFrom" },
                    { name: "HTSCode" },
                    { name: "CountryOfOrigin" },
                    { name: "Notes" }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetRawEquipmentFromImport",
                    dataSrc: "",
                    data: function () {
                        return {
                            FilePath: $("#FilePath").val()
                        }
                    }
                },
                order: [[2, 'desc']],
                rowId: 'EquipmentId',
                columnDefs: [
                    {
                        data: "EquipmentId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>"
                        }
                    },
                    {
                        data: "EquipmentName", "targets": 1,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.EquipmentNameColor);
                        },
                        className: "equipmentNameWidth"
                    },
                    {
                        data: "PriorityNumber", "targets": 2,
                        className: "priorityWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": 3,
                        className: "dateWidth"
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
                        data: "ReadyToShip", "targets": 10, className: "text-right readyToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedFrom", "targets": 11,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "HTSCode", "targets": 12,
                        className: "htsCodeWidth"
                    },
                    {
                        data: "CountryOfOrigin", "targets": 13,
                        className: "countryOfOriginWidth"
                    },
                    {
                        data: "Notes", "targets": 14,
                        className: "notesWidth"
                    }
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                keys: {
                    columns: ":not(:first-child)"
                },
                autoFill: {
                    editor: null,
                    columns: ":not(:first-child)"
                }
            });
        }

        this.validationErrors = function () {
            var $this = this;

            var errors = false;

            editorMain.datatable.rows({ selected: true }).every(function (rowIdx, tableLoop, rowLoop) {
                var error = false;
                var data = this.data();

                var equipmentName = data.EquipmentName;
                var releaseDate = data.ReleaseDate;
                var drawingNumber = data.DrawingNumber;
                var workOrderNumber = data.WorkOrderNumber;
                var shippingTagNumber = data.ShippingTagNumber;
                var description = data.Description;
                var unitWeight = data.UnitWeightText;
                var quantity = data.Quantity;
                var readyToShip = data.ReadyToShip;

                if (!equipmentName) {
                    error = true;
                    $this.addError(rowIdx, 1);
                } else {
                    $this.removeError(rowIdx, 1);
                }

                if (!releaseDate) {
                    error = true;
                    $this.addError(rowIdx, 3);
                } else if (!moment(releaseDate, 'MM/DD/YYYY', true).isValid()) {
                    error = true;
                    $this.addError(rowIdx, 3);
                } else {
                    $this.removeError(rowIdx, 3);
                }

                if (!drawingNumber) {
                    error = true;
                    $this.addError(rowIdx, 4);
                } else {
                    $this.removeError(rowIdx, 4);
                }

                if (!workOrderNumber) {
                    error = true;
                    $this.addError(rowIdx, 5);
                } else {
                    $this.removeError(rowIdx, 5);
                }

                if (!shippingTagNumber) {
                    error = true;
                    $this.addError(rowIdx, 7);
                } else {
                    $this.removeError(rowIdx, 7);
                }

                if (!description) {
                    error = true;
                    $this.addError(rowIdx, 8);
                } else {
                    $this.removeError(rowIdx, 8);
                }

                if (unitWeight == null) {
                    error = true;
                    $this.addError(rowIdx, 9);
                } else if (isNaN(unitWeight)) {
                    error = true;
                    $this.addError(rowIdx, 9);
                } else {
                    $this.removeError(rowIdx, 9);
                }

                if (quantity == null) {
                    error = true;
                    $this.addError(rowIdx, 6);
                } else if (isNaN(quantity)) {
                    error = true;
                    $this.addError(rowIdx, 6);
                } else {
                    $this.removeError(rowIdx, 6);
                }

                if (readyToShip == null) {
                    error = true;
                    $this.addError(rowIdx, 10);
                } else if (isNaN(readyToShip)) {
                    error = true;
                    $this.addError(rowIdx, 10);
                } else {
                    $this.removeError(rowIdx, 10);
                }

                if (error) {
                    $(this.node()).addClass("danger");
                    errors = true;
                } else {
                    $(this.node()).removeClass("danger");
                }
            });

            return errors;
        }


        this.clearValidation = function () {
            var $this = this;

            editorMain.datatable.rows({ selected: false }).every(function (rowIdx, tableLoop, rowLoop) {

                $this.removeError(rowIdx, 1);
                $this.removeError(rowIdx, 3);
                $this.removeError(rowIdx, 4);
                $this.removeError(rowIdx, 5);
                $this.removeError(rowIdx, 7);
                $this.removeError(rowIdx, 8);
                $this.removeError(rowIdx, 9);
                $this.removeError(rowIdx, 6);
                $this.removeError(rowIdx, 10);
                $(this.node()).removeClass("danger");
            });
        }

        this.addError = function (row, column) {
            $(editorMain.datatable.cell(row, column).node()).addClass("Red");
        }

        this.removeError = function (row, column) {
            $(editorMain.datatable.cell(row, column).node()).removeClass("Red");
        }


        this.initStyles();
        this.initEvents();
    }
});