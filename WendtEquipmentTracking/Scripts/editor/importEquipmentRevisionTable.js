$(function () {

    ImportEquipmentRevisionTable = function () {

        this.canSubmit = false;
        this.columnIndexes = {
            NewEquipmentId: 0,
            EquipmentName: 1,
            PriorityNumber: 2,
            ReleaseDate: 3,
            DrawingNumber: 4,
            WorkOrderNumber: 5,
            Quantity: 6,
            ShippedQuantity: 7,
            ShippingTagNumber: 8,
            Description: 9,
            UnitWeightText: 10,
            ShippedFrom: 11,
            NewEquipmentName: 12,
            NewPriorityNumber: 13,
            NewReleaseDate: 14,
            NewDrawingNumber: 15,
            NewWorkOrderNumber: 16,
            NewQuantity: 17,
            NewShippingTagNumber: 18,
            NewDescription: 19,
            NewUnitWeightText: 20,
            NewShippedFrom: 21,
            EquipmentId: 22,
            Revsion: 23
        };
        this.editableColumns = [
            this.columnIndexes.NewEquipmentName,
            this.columnIndexes.NewPriorityNumber,
            this.columnIndexes.NewReleaseDate,
            this.columnIndexes.NewDrawingNumber,
            this.columnIndexes.NewWorkOrderNumber,
            this.columnIndexes.NewQuantity,
            this.columnIndexes.NewShippingTagNumber,
            this.columnIndexes.NewDescription,
            this.columnIndexes.NewUnitWeightText,
            this.columnIndexes.NewShippedFrom
        ];
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

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
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.PriorityNumber).node()).attr("class", "active " + data.RevisionIndicators.PriorityNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ReleaseDate).node()).attr("class", "active " + data.RevisionIndicators.ReleaseDateColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.DrawingNumber).node()).attr("class", "active " + data.RevisionIndicators.DrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.WorkOrderNumber).node()).attr("class", "active " + data.RevisionIndicators.WorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.Quantity).node()).attr("class", "text-right active " + data.RevisionIndicators.QuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippedQuantity).node()).attr("class", "text-right active " + data.RevisionIndicators.ShippedQuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippingTagNumber).node()).attr("class", "active " + data.RevisionIndicators.ShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.Description).node()).attr("class", "active " + data.RevisionIndicators.DescriptionColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.UnitWeightText).node()).attr("class", "text-right active " + data.RevisionIndicators.UnitWeightColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippedFrom).node()).attr("class", "active " + data.RevisionIndicators.ShippedFromColor);

                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewEquipmentName).node()).attr("class", data.RevisionIndicators.NewEquipmentNameColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewPriorityNumber).node()).attr("class", data.RevisionIndicators.NewPriorityNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewReleaseDate).node()).attr("class", data.RevisionIndicators.NewReleaseDateColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewDrawingNumber).node()).attr("class", data.RevisionIndicators.NewDrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewWorkOrderNumber).node()).attr("class", data.RevisionIndicators.NewWorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewQuantity).node()).attr("class", "text-right " + data.RevisionIndicators.NewQuantityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewShippingTagNumber).node()).attr("class", data.RevisionIndicators.NewShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewDescription).node()).attr("class", data.RevisionIndicators.NewDescriptionColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewUnitWeightText).node()).attr("class", "text-right " + data.RevisionIndicators.NewUnitWeightColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.NewShippedFrom).node()).attr("class", data.RevisionIndicators.NewShippedFromColor);

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
                    { name: "PriorityNumber", type: "readonly" },
                    { name: "ReleaseDate", type: "readonly" },
                    { name: "DrawingNumber", type: "readonly" },
                    { name: "WorkOrderNumber", type: "readonly" },
                    { name: "Quantity", type: "readonly" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "ShippingTagNumber", type: "readonly" },
                    { name: "Description", type: "readonly" },
                    { name: "UnitWeightText", type: "readonly" },
                    { name: "ShippedFrom", type: "readonly"},

                    { name: "NewEquipmentId", type: "readonly" },
                    { name: "NewEquipmentName" },
                    {
                        name: "NewPriorityNumber",
                        type: "select",
                        options: priorities,
                        placeholderDisabled: false,
                        placeholder: ""
                    },
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
                    { name: "NewShippedFrom" },
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
                            Equipment: $("#Equipment").val(),
                            FilePath: $("input[name='FilePath']").val(),
                            PriorityId: $("#PriorityId").val(),
                            QuantityMultiplier: $("#QuantityMultiplier").val(),
                            WorkOrderNumber: $("#WorkOrderNumber").val(),
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
                        data: "NewEquipmentId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: this.columnIndexes.NewEquipmentId,
                        render: function () {
                            return "<span></span>";
                        }
                    },
                    {
                        data: "EquipmentName", "targets": this.columnIndexes.EquipmentName,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.EquipmentNameColor);
                        },
                        className: "active equipmentNameWidth"
                    },
                    {
                        data: "PriorityNumber", "targets": this.columnIndexes.PriorityNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.PriorityNumberColor);
                        },
                        className: "active priorityWidth"
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
                        className: "active workOrderNumberWidth"
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
                        className: "active descriptionWidth"
                    },
                    {
                        data: "UnitWeightText", "targets": this.columnIndexes.UnitWeightText, 
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.UnitWeightColor);
                        },
                        className: "active text-right unitWeightWidth"
                    },
                    {
                        data: "ShippedFrom", "targets": this.columnIndexes.ShippedFrom,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.ShippedFromColor);
                        },
                        className: "active shippedFromWidth"
                    },



                    
                    {
                        data: "NewEquipmentName", "targets": this.columnIndexes.NewEquipmentName,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewEquipmentNameColor);
                        },
                        className: "equipmentNameWidth datatable-border-left" 
                    },
                    {
                        data: "NewPriorityNumber", "targets": this.columnIndexes.NewPriorityNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewPriorityNumberColor);
                        },
                        className: "priorityWidth"
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
                        className: "workOrderNumberWidth"
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
                        className: "descriptionWidth"
                    },
                    {
                        data: "NewUnitWeightText", "targets": this.columnIndexes.NewUnitWeightText, 
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewUnitWeightColor);
                        },
                        className: "text-right unitWeightWidth"
                    },
                    {
                        data: "NewShippedFrom", "targets": this.columnIndexes.NewShippedFrom,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.RevisionIndicators.NewShippedFromColor);
                        },
                        className: "shippedFromWidth"
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
                    selector: 'td:first-child'
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