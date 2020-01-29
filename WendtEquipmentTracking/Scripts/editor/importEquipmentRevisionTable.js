﻿$(function () {

    ImportEquipmentRevisionTable = function () {

        this.canSubmit = false;
        this.columnIndexes = {
            EquipmentName: 0,
            ReleaseDate: 1,
            DrawingNumber: 2,
            WorkOrderNumber: 3,
            Quantity: 4,
            ShippedQuantity: 5,
            ShippingTagNumber: 6,
            Description: 7,
            UnitWeightText: 8,
            NewEquipmentId: 9,
            NewEquipmentName: 10,
            NewReleaseDate: 11,
            NewDrawingNumber: 12,
            NewWorkOrderNumber: 13,
            NewQuantity: 14,
            NewShippingTagNumber: 15,
            NewDescription: 16,
            NewUnitWeightText: 17,
            EquipmentId: 18,
            Revsion: 19
        };
        this.editableColumns = [
            this.columnIndexes.NewEquipmentName,
            this.columnIndexes.NewReleaseDate,
            this.columnIndexes.NewDrawingNumber,
            this.columnIndexes.NewWorkOrderNumber,
            this.columnIndexes.NewQuantity,
            this.columnIndexes.NewShippingTagNumber,
            this.columnIndexes.NewDescription,
            this.columnIndexes.NewUnitWeightText
        ];
        this.editorMain = new Editor();

        this.initStyles = function () {

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;


            this.editorMain.editor.on('preSubmit', function (e, data, action) {


                if ($this.canSubmit) {
                    $.each(data.data, function (i, rowData) {
                        var row = $this.editorMain.datatable.row("#" + rowData.NewEquipmentId);

                        var rowNumber = $this.editorMain.datatable.rows({ order: 'applied' }).nodes().indexOf(row.node());
                        rowData.Order = rowNumber;
                    });
                }

                data.doSubmit = $this.canSubmit;
            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.NewEquipmentId);



                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.EquipmentName).node()).attr("class", "active " + data.RevisionIndicators.EquipmentNameColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ReleaseDate).node()).attr("class", "active " + data.RevisionIndicators.ReleaseDateColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.DrawingNumber).node()).attr("class", "active " + data.RevisionIndicators.DrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.WorkOrderNumber).node()).attr("class", "active " + data.RevisionIndicators.WorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.Quantity).node()).attr("class", "text-right active " + data.RevisionIndicators.QuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippedQuantity).node()).attr("class", "text-right active " + data.RevisionIndicators.ShippedQuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippingTagNumber).node()).attr("class", "active " + data.RevisionIndicators.ShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.Description).node()).attr("class", "active " + data.RevisionIndicators.DescriptionColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.UnitWeightText).node()).attr("class", "text-right active " + data.RevisionIndicators.UnitWeightColor);

                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewEquipmentName).node()).attr("class", data.RevisionIndicators.NewEquipmentNameColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewReleaseDate).node()).attr("class", data.RevisionIndicators.NewReleaseDateColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewDrawingNumber).node()).attr("class", data.RevisionIndicators.NewDrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewWorkOrderNumber).node()).attr("class", data.RevisionIndicators.NewWorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewQuantity).node()).attr("class", "text-right " + data.RevisionIndicators.NewQuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewShippingTagNumber).node()).attr("class", data.RevisionIndicators.NewShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewDescription).node()).attr("class", data.RevisionIndicators.NewDescriptionColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewUnitWeightText).node()).attr("class", "text-right " + data.RevisionIndicators.NewUnitWeightColor);

            });

            this.editorMain.datatable.on('autoFill', function (e, datatable, cells) {
                $this.validationErrors();
            });

            this.editorMain.editor.on('submitComplete', function () {
                $this.validationErrors();
            });

            $("#import").on("click", function () {
                var selectedRows = $this.editorMain.datatable.rows({ selected: true }).indexes();
                if (selectedRows.length) {
                    if (!$this.validationErrors()) {
                        $this.canSubmit = true;

                        $this.editorMain.editor.edit(
                            $this.editorMain.datatable.rows({ selected: true }).indexes(),
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

            this.editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.validationErrors();
                }
            });

            this.editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.clearValidation();
                }
            });
        };

        this.initEditor = function () {

            this.editorMain.initEditor({
                formOptions: {},
                ajax: {
                    url: ROOT_URL + "api/ImportApi/EquipmentRevisionEditor",
                    dataSrc: ""
                },
                idSrc: 'NewEquipmentId',
                fields: [
                    { name: "EquipmentId", type: "readonly" },
                    { name: "EquipmentName", type: "readonly" },
                    { name: "ReleaseDate", type: "readonly" },
                    { name: "DrawingNumber", type: "readonly" },
                    { name: "WorkOrderNumber", type: "readonly" },
                    { name: "Quantity", type: "readonly" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "ShippingTagNumber", type: "readonly" },
                    { name: "Description", type: "readonly" },
                    { name: "UnitWeightText", type: "readonly" },

                    { name: "NewEquipmentId", type: "readonly" },
                    { name: "NewEquipmentName" },
                    {
                        name: "NewReleaseDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "NewDrawingNumber", type: "textarea" },
                    { name: "NewWorkOrderNumber" },
                    { name: "NewQuantity" },
                    { name: "NewShippingTagNumber", type: "textarea" },
                    { name: "NewDescription", type: "textarea" },
                    { name: "NewUnitWeightText" },
                    { name: "HasExistingEquipment" },
                    { name: "HasNewEquipment" },
                    { name: "Order" },
                    { name: "Revision" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetEquipmentRevisionFromImport",
                    type: "POST",
                    dataSrc: "",
                    data: function () {

                        return {
                            FilePath: $("input[name='FilePath']").val(),
                            DrawingNumber: $("#DrawingNumber").val(),
                            Revision: $("#Revision").val()
                        };
                    }
                },
                order: [[this.columnIndexes.NewEquipmentName, 'desc']],
                rowId: 'NewEquipmentId',
                initComplete: function (settings, json) {
                    $this.editorMain.datatable.rows(function (idx, data, node) {
                        return data.HasChanged;
                    }).select();
                },
                autoWidth: false,
                columnDefs: [
                    
                    {
                        data: "EquipmentName", "targets": this.columnIndexes.EquipmentName,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.EquipmentNameColor);
                        },
                        className: "active equipmentNameRevisionWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": this.columnIndexes.ReleaseDate, type: "date",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.ReleaseDateColor);
                        },
                        className: "active dateWidth"
                    },
                    {
                        data: "DrawingNumber", "targets": this.columnIndexes.DrawingNumber,
                        className: "active drawingNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.DrawingNumberColor);
                        }
                    },
                    {
                        data: "WorkOrderNumber", "targets": this.columnIndexes.WorkOrderNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.WorkOrderNumberColor);
                        },
                        className: "active workOrderNumberRevisionWidth"
                    },
                    {
                        data: "Quantity", "targets": this.columnIndexes.Quantity,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.QuantityColor);
                        },
                        className: "active text-right quantityWidth"
                    },
                    {
                        data: "ShippedQuantity", "targets": this.columnIndexes.ShippedQuantity,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.ShippedQuantityColor);
                        },
                        className: "active text-right shippedQuantityWidth"
                    },
                    {
                        data: "ShippingTagNumber", "targets": this.columnIndexes.ShippingTagNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.ShippingTagNumberColor);
                        },
                        className: "active shippingTagNumberWidth"
                    },
                    {
                        data: "Description", "targets": this.columnIndexes.Description,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.DescriptionColor);
                        },
                        className: "active descriptionRevisionWidth"
                    },
                    {
                        data: "UnitWeightText", "targets": this.columnIndexes.UnitWeightText, 
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.UnitWeightColor);
                        },
                        className: "active text-right unitWeightWidth"
                    },
                    {
                        data: "NewEquipmentId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: this.columnIndexes.NewEquipmentId,
                        render: function () {
                            return "<span></span>";
                        }
                    },


                    
                    {
                        data: "NewEquipmentName", "targets": this.columnIndexes.NewEquipmentName,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewEquipmentNameColor);
                        },
                        className: "equipmentNameRevisionWidth" 
                    },
                    {
                        data: "NewReleaseDate", "targets": this.columnIndexes.NewReleaseDate, type: "date",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewReleaseDateColor);
                        },
                        className: "dateWidth"
                    },
                    {
                        data: "NewDrawingNumber", "targets": this.columnIndexes.NewDrawingNumber,
                        className: "drawingNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewDrawingNumberColor);
                        }
                    },
                    {
                        data: "NewWorkOrderNumber", "targets": this.columnIndexes.NewWorkOrderNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewWorkOrderNumberColor);
                        },
                        className: "workOrderNumberRevisionWidth"
                    },
                    {
                        data: "NewQuantity", "targets": this.columnIndexes.NewQuantity,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewQuantityColor);
                        },
                        className: "text-right quantityWidth"
                    },
                    {
                        data: "NewShippingTagNumber", "targets": this.columnIndexes.NewShippingTagNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewShippingTagNumberColor);
                        },
                        className: "shippingTagNumberWidth"
                    },
                    {
                        data: "NewDescription", "targets": this.columnIndexes.NewDescription,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewDescriptionColor);
                        },
                        className: "descriptionRevisionWidth"
                    },
                    {
                        data: "NewUnitWeightText", "targets": this.columnIndexes.NewUnitWeightText, 
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewUnitWeightColor);
                        },
                        className: "text-right unitWeightWidth"
                    },
                    {
                        data: "EquipmentId",
                        visible: false,
                        targets: this.columnIndexes.EquipmentId
                    },
                    {
                        data: "Revision",
                        visible: false,
                        targets: this.columnIndexes.Revision
                    }
                ],
                select: {
                    style: 'multi',
                    selector: 'td.select-checkbox'
                },
                keys: {
                    columns: this.editableColumns
                },
                autoFill: {
                    editor: null,
                    columns: this.editableColumns
                }
            });
        };

        this.validationErrors = function () {
            var $this = this;

            var errors = false;

            //this.editorMain.datatable.rows({ selected: true }).every(function (rowIdx, tableLoop, rowLoop) {
            //    var error = false;
            //    var data = this.data();

            //    if (data.HasNewEquipment) {
            //        var newEquipmentName = data.NewEquipmentName;
            //        var newReleaseDate = data.NewReleaseDate;
            //        var newDrawingNumber = data.NewDrawingNumber;
            //        var newWorkOrderNumber = data.NewWorkOrderNumber;
            //        var newShippingTagNumber = data.NewShippingTagNumber;
            //        var newDescription = data.NewDescription;
            //        var newUnitWeight = data.NewUnitWeightText;
            //        var newQuantity = data.NewQuantity;

            //        if (!newEquipmentName) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewEquipmentName);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewEquipmentName);
            //        }

            //        if (!newReleaseDate) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewReleaseDate);
            //        } else if (!moment(newReleaseDate, 'M/D/YY', true).isValid()) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewReleaseDate);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewReleaseDate);
            //        }

            //        if (!newDrawingNumber) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewDrawingNumber);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewDrawingNumber);
            //        }

            //        if (!newWorkOrderNumber) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewWorkOrderNumber);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewWorkOrderNumber);
            //        }

            //        if (!newShippingTagNumber) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewShippingTagNumber);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewShippingTagNumber);
            //        }

            //        if (!newDescription) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewDescription);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewDescription);
            //        }

            //        if (!newUnitWeight) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewUnitWeightText);
            //        } else if (isNaN(newUnitWeight)) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewUnitWeightText);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewUnitWeightText);
            //        }

            //        if (!newQuantity) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewQuantity);
            //        } else if (isNaN(newQuantity)) {
            //            error = true;
            //            $this.addError(rowIdx, $this.columnIndexes.NewQuantity);
            //        } else {
            //            $this.removeError(rowIdx, $this.columnIndexes.NewQuantity);
            //        }

            //        if (error) {
            //            errors = true;
            //        }

            //    }
            //});

            return errors;
        };


        this.clearValidation = function () {
            var $this = this;

            this.editorMain.datatable.rows({ selected: false }).every(function (rowIdx, tableLoop, rowLoop) {

                $this.removeError(rowIdx, $this.columnIndexes.NewEquipmentName);
                $this.removeError(rowIdx, $this.columnIndexes.NewReleaseDate);
                $this.removeError(rowIdx, $this.columnIndexes.NewDrawingNumber);
                $this.removeError(rowIdx, $this.columnIndexes.NewWorkOrderNumber);
                $this.removeError(rowIdx, $this.columnIndexes.NewShippingTagNumber);
                $this.removeError(rowIdx, $this.columnIndexes.NewDescription);
                $this.removeError(rowIdx, $this.columnIndexes.NewUnitWeightText);
                $this.removeError(rowIdx, $this.columnIndexes.NewQuantity);
            });
        };

        this.addError = function (row, column) {
            $(this.editorMain.datatable.cell(row, column).node()).addClass("Error");
        };

        this.removeError = function (row, column) {
            $(this.editorMain.datatable.cell(row, column).node()).removeClass("Error");
        };

        this.initStyles();
        this.initEvents();
    };
});