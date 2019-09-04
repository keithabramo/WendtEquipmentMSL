$(function () {

    ImportEquipmentTable = function () {

        this.canSubmit = false;

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;


            editorMain.editor.on('preSubmit', function (e, data, action) {


                if ($this.canSubmit) {
                    $.each(data.data, function (i, rowData) {
                        var row = editorMain.datatable.row("#" + rowData.EquipmentId);

                        var rowNumber = editorMain.datatable.rows({ order: 'applied' }).nodes().indexOf(row.node());
                        rowData.Order = rowNumber;
                    });
                }

                data.doSubmit = $this.canSubmit;
            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                var row = editorMain.datatable.row("#" + data.EquipmentId);

                

                $(editorMain.datatable.cell(row.index(), 1).node()).attr("class", data.Indicators.EquipmentNameColor);
                $(editorMain.datatable.cell(row.index(), 2).node()).attr("class", data.Indicators.PriorityColor);
                $(editorMain.datatable.cell(row.index(), 4).node()).attr("class", data.Indicators.DrawingNumberColor);
                $(editorMain.datatable.cell(row.index(), 5).node()).attr("class", data.Indicators.WorkOrderNumberColor);
                $(editorMain.datatable.cell(row.index(), 7).node()).attr("class", data.Indicators.ShippingTagNumberColor);
                $(editorMain.datatable.cell(row.index(), 9).node()).attr("class", "text-right " + data.Indicators.UnitWeightColor);
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
                            editorMain.datatable.rows({ selected: true }).indexes(),
                            false
                        ).submit(function () {
                            $this.canSubmit = false;
                            location.href = ROOT_URL + "Equipment/?ajaxSuccess=true";
                        }, function () {
                            $this.canSubmit = false;
                            $("#import").button("reset");

                            main.error("There was an error whill trying to import");
                        });
                    } else {
                        $("#import").button("reset");

                        main.error("Please address all rows with validation errors before importing.");
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
                    url: ROOT_URL + "api/ImportApi/EquipmentEditor",
                    dataSrc: ""
                },
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentId", type: "readonly" },
                    { name: "EquipmentName" },
                    {
                        name: "PriorityNumber",
                        type: "select",
                        options: priorities,
                        placeholderDisabled: false,
                        placeholder: ""
                    },
                    { name: "ReleaseDate", type: "datetime", format: "MM/DD/YYYY" },
                    { name: "DrawingNumber", type: "textarea" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" },
                    { name: "ShippedFrom" },
                    { name: "Order" }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;

            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetEquipmentFromImport",
                    type: "POST",
                    dataSrc: "",
                    data: function () {

                        var filePaths = $("input[name='FilePath']").map(function () {
                            return $(this).val();
                        }).get();

                        return {
                            //DrawingNumber: $("#DrawingNumber").val(),
                            Equipment: $("#Equipment").val(),
                            FilePaths: filePaths,
                            PriorityId: $("#PriorityId").val(),
                            QuantityMultiplier: $("#QuantityMultiplier").val(),
                            WorkOrderNumber: $("#WorkOrderNumber").val()
                        }
                    }
                },
                order: [[2, 'desc']],
                rowId: 'EquipmentId',
                initComplete: function (settings, json) {
                    editorMain.datatable.rows().select();
                    $(".table thead th.select-checkbox").closest("tr").addClass("selected");
                },
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
                        data: "PriorityNumber", "targets": 2,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.PriorityColor);
                        },
                        className: "priorityWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": 3, type: "date",
                        className: "dateWidth"
                    },
                    {
                        data: "DrawingNumber", "targets": 4,
                        className: "drawingNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.DrawingNumberColor);
                        },
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
                        className: "shippingTagNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippingTagNumberColor);
                        },
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
                        data: "ShippedFrom", "targets": 10,
                        className: "shippedFromWidth"
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
            $(editorMain.datatable.cell(row, column).node()).addClass("Error");
        }

        this.removeError = function (row, column) {
            $(editorMain.datatable.cell(row, column).node()).removeClass("Error");
        }

        this.initStyles();
        this.initEvents();
    }
});