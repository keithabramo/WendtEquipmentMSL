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

                        if (this.inError()) {
                            return false;
                        }
                    }

                    data.doSubmit = $this.canSubmit;
                }
            });

            $("#import").on("click", function () {

                $this.canSubmit = true;

                editorMain.editor.edit(
                    editorMain.datatable.rows({ selected: true }).indexes(), false
                ).submit(function () {
                    $this.canSubmit = false;
                    location.href = ROOT_URL + "Equipment/?ajaxSuccess=true"
                }, function () {
                    $this.canSubmit = false;
                    $(".global-message").html('<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>ERROR! </strong>There was an error whill trying to import</div>');
                });

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




        this.initStyles();
        this.initEvents();
    }
});