$(function () {

    var EditorWorkOrderPrice = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;

            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var workOrderNumber = this.field('WorkOrderNumber');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!workOrderNumber.isMultiValue()) {
                        if (!workOrderNumber.val()) {
                            workOrderNumber.error('A work order number is required');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }
            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                var row = editorMain.datatable.row("#" + data.WorkOrderPriceId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

            });
        }

        this.initEditor = function () {

            editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/WorkOrderPriceApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'WorkOrderPriceId',
                fields: [
                    {
                        label: "Work Order Price Id",
                        name: "WorkOrderPriceId"
                    }, {
                        label: "Project Id",
                        name: "ProjectId"
                    }, {
                        label: "Work Order Number",
                        name: "WorkOrderNumber"
                    }, {
                        label: "Sales+Soft Costs",
                        name: "CostPrice"
                    }, {
                        label: "Sale Price",
                        name: "SalePrice"
                    }, {
                        label: "Released %",
                        name: "ReleasedPercent"
                    }, {
                        label: "Shipped %",
                        name: "ShippedPercent"
                    }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/WorkOrderPriceApi/Table",
                    dataSrc: ""
                },
                rowId: 'WorkOrderPriceId',
                columns: [
                    { data: "WorkOrderNumber" },
                    { data: "CostPrice" },
                    { data: "SalePrice" },
                    { data: "ReleasedPercent" },
                    { data: "ShippedPercent" }
                ],
                columnDefs: [
                    {
                        "targets": 5,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    },
                    {
                        targets: [1,2],
                        render: $.fn.dataTable.render.number(',', '.', 2, '$')
                    }
                ],
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                },
            });
        }

        this.initStyles();
        this.initEvents();
    }

    editorWorkOrderPrice = new EditorWorkOrderPrice();

});