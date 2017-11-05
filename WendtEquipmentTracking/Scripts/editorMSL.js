$(function () {

    var EditorMSL = function () {

        this.dataTable;
        this.editor;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

        }

        this.initEvents = function () {
            var $this = this;

        }

        this.initEditor = function () {
            this.editor = new $.fn.dataTable.Editor({
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Editor",
                    dataSrc: ""
                },
                table: '.table.my-datatable',
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentName" },
                    { name: "Priority" },
                    { name: "ReleaseDate" },
                    { name: "DrawingNumber" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber" },
                    { name: "Description" },
                    { name: "UnitWeight" },
                    { name: "TotalWeight" },
                    { name: "TotalWeightShipped" },
                    { name: "ReadyToShip" },
                    { name: "ShippedQuantity" },
                    { name: "LeftToShip" },
                    { name: "FullyShippedText" },
                    { name: "ShippedFrom" },
                    { name: "CustomsValue" },
                    { name: "SalePrice" },
                    { name: "HTSCode" },
                    { name: "CountryOfOrigin" },
                    { name: "Notes" },
                    { name: "EquipmentId" },
                    { name: "HasBillOfLading" },
                    { name: "IsHardwareKit" },
                    { name: "IsAssociatedToHardwareKit" },
                    { name: "AssociatedHardwareKitNumber" },
                    { name: "Indicators" },
                    { name: "IsDuplicate" },
                    { name: "FullyShipped" },
                    { name: "HasBillOfLadingInStorage" }
                ],
                formOptions: {
                    inline: {
                        onReturn: "submit",
                        onBlur: "submit",
                        submit: "allIfChanged"
                    },
                    //used by autofill
                    bubble: {
                        submit: "allIfChanged"
                    }
                }
            });
        }

        this.initDatatable = function () {
            var keyCodes = [];

            for (var i = 8; i <= 222; i++) {
                if (i !== 37 && i !== 39) {
                    keyCodes.push(i);
                }
            }

            mslSettings = {
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Table",
                    dataSrc: ""
                },
                rowId: 'EquipmentId',
                keys: {
                    editor: this.editor,
                    keys: keyCodes
                },
                autoWidth: false,
                drawCallback: function (settings) {

                    $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");



                    if ($(".pagination li").length === 2) {
                        $(".pagination").parent().hide();
                    } else {
                        $(".pagination").parent().show();
                    }
                },
                //createdRow: function (row, data, index) {
                //    if (data.IsDuplicate) {
                //        $(row).addClass('warning');
                //    }
                //    //if (data.FullyShipped === true) {
                //    //    $(row).find("input, select, textarea").attr("readOnly", "readOnly").attr("disabled", "disabled");
                //    //}
                //},
                deferRender: true,
                fixedHeader: true,
                lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                pageLength: 100,
                order: [[2, 'desc']],
                columnDefs: [
                    {
                        data: "EquipmentName", "targets": 0,
                        //className: "equipmentNameWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.EquipmentNameRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "Priority", "targets": 1,
                        //className: "priorityWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.PriorityRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "ReleaseDate", "targets": 2, "type": "date",
                        //className: "releaseDateWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.ReleaseDateRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "DrawingNumber", "targets": 3,
                        //className: "drawingNumberWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.DrawingNumberRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "WorkOrderNumber", "targets": 4,
                        //className: "workOrderNumberWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.WorkOrderNumberRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "Quantity", "targets": 5,
                        //className: "quantityWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.QuantityRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "ShippingTagNumber", "targets": 6,
                        //className: "shippingTagNumberWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.ShippingTagNumberRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "Description", "targets": 7,
                        className: "descriptionWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.DescriptionRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "UnitWeight", "targets": 8,
                        className: "unitWeightWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.UnitWeightRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "TotalWeight", "targets": 9,
                        //className: "totalWeightWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.TotalWeightRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "TotalWeightShipped", "targets": 10,
                        //className: "totalWeightShippedWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.TotalWeightShippedRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "ReadyToShip", "targets": 11,
                        //className: "readyToShipWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.ReadyToShipRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "ShippedQuantity", "targets": 12,
                        //className: "shippedQuantityWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.ShippedQuantityRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "LeftToShip", "targets": 13,
                        //className: "leftToShipWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.LeftToShipRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "FullyShippedText", "targets": 14,
                        //className: "fullyShippedWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.FullyShippedRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "ShippedFrom", "targets": 15,
                        //className: "shippedFromWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.ShippedFromRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "CustomsValue", "targets": 16,
                        //className: "customsValueWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.CustomsValueRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "SalePrice", "targets": 17,
                        //className: "salePriceWidth text-right",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.SalePriceRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "HTSCode", "targets": 18,
                        //className: "htsCodeWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.HTSCodeRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "CountryOfOrigin", "targets": 19,
                        //className: "countryOfOriginWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.CountryOfOriginRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "Notes", "targets": 20,
                        //className: "notesWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.NotesRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "EquipmentId", "targets": 21, searchable: false, sortable: false,
                        //className: "deleteWidth",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.DeleteRender($(cell), rowData);
                        //}
                    },
                    {
                        data: "HasBillOfLading", "targets": 22, searchable: false,
                        //className: "hasBillOfLadingWidth expand",
                        //createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                        //    mslRender.HasBillOfLadingRender($(cell), rowData);
                        //}
                    },
                    { data: "IsHardwareKit", "targets": 23, visible: false, searchable: false },
                    { data: "IsAssociatedToHardwareKit", "targets": 24, visible: false, searchable: false },
                    { data: "AssociatedHardwareKitNumber", "targets": 25, visible: false },
                    { data: "Indicators", "targets": 26, visible: false, searchable: false },
                    { data: "IsDuplicate", "targets": 27, visible: false, searchable: false },
                    { data: "FullyShipped", "targets": 28, visible: false, searchable: false },
                    { data: "HasBillOfLadingInStorage", "targets": 29, visible: false, searchable: false }
                ],
                autoFill: {
                    editor: this.editor,
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 15, 18, 19, 20]
                },
                dom: "<'row'<'col-sm-5 text-left custom'f><'col-sm-3 text-center'i><'col-sm-4 text-right'l>>" +
                                         "<'row'<'col-sm-12'tr>>" +
                                         "<'row'<'col-sm-2 text-left createButtonContainer'><'col-sm-10 text-center'p>>"
            };

            this.dataTable = $(".table.my-datatable").DataTable(mslSettings);

            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;
        }

        this.initStyles();
        this.initEvents();
    }

    editorMSL = new EditorMSL();


    































});

function DeleteSuccess() {
    $(this).closest("tr").remove();
};