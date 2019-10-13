$(function () {

    var TableHardwareCommercialCode = function () {

        this.tableMain = new Table();

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;
        };

        this.initEditor = function () {

            this.tableMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/HardwareCommercialCodeApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'HardwareCommercialCodeId'
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/HardwareCommercialCodeApi/Table",
                    dataSrc: ""
                },
                rowId: 'HardwareCommercialCodeId',
                columns: [
                    { data: "PartNumber" },
                    { data: "Description" },
                    { data: "CommodityCode" },
                    { data: "HardwareCommercialCodeId" }
                ],
                columnDefs: [
                    {
                        "targets": 3,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'HardwareCommercialCode/Edit/' + row.HardwareCommercialCodeId + '" >Edit</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableHardwareCommercialCode = new TableHardwareCommercialCode();

});