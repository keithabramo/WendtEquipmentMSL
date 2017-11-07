$(function () {

    var EditorWorkOrderPrice = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;

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
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    }

    editorWorkOrderPrice = new EditorWorkOrderPrice();

});