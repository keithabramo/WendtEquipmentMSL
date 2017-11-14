$(function () {

    ImportEquipmentTable = function () {

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

                if (!$this.validationErrors()) {
                    $this.canSubmit = true;

                    editorMain.editor.edit(
                        editorMain.datatable.rows({ selected: true }).indexes(), false
                    ).submit(function () {
                        $this.canSubmit = false;
                        location.href = ROOT_URL + "Equipment/?ajaxSuccess=true"
                    }, function () {
                        $this.canSubmit = false;
                        main.error("There was an error whill trying to import");
                    });
                } else {
                    main.error("Please address all rows with validation errors before importing.")
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
                    url: ROOT_URL + "api/ImportApi/EquipmentEditor",
                    dataSrc: ""
                },
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentId", type: "readonly" },
                    { name: "EquipmentName" },
                    {
                        name: "Priority",
                        type: "select",
                        options: priorities
                    },
                    { name: "ReleaseDate", type: "datetime", format: "MM/DD/YYYY" },
                    { name: "DrawingNumber", type: "textarea" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;

            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetEquipmentFromImport",
                    dataSrc: "",
                    data: function () {
                        return {
                            DrawingNumber: $("#DrawingNumber").val(),
                            Equipment: $("#Equipment").val(),
                            FilePath: $("#FilePath").val(),
                            Priority: $("#Priority").val(),
                            QuantityMultiplier: $("#QuantityMultiplier").val(),
                            WorkOrderNumber: $("#WorkOrderNumber").val()
                        }
                    }
                },
                order: [[2, 'desc']],
                rowId: 'EquipmentId',
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('danger');
                    }
                },
                autoWidth: false,
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
                        data: "Priority", "targets": 2,
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

                if (!unitWeight) {
                    error = true;
                    $this.addError(rowIdx, 9);
                } else if (isNaN(unitWeight)) {
                    error = true;
                    $this.addError(rowIdx, 9);
                } else {
                    $this.removeError(rowIdx, 9);
                }

                if (!quantity) {
                    error = true;
                    $this.addError(rowIdx, 6);
                } else if (isNaN(quantity)) {
                    error = true;
                    $this.addError(rowIdx, 6);
                } else {
                    $this.removeError(rowIdx, 6);
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