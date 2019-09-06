$(function () {

    var TableHardwareKit = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;
        };

        this.initEditor = function () {

            tableMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/HardwareKitApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'HardwareKitId'
            });
        };

        this.initDatatable = function () {
            var $this = this;

            tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/HardwareKitApi/Table",
                    dataSrc: ""
                },
                rowId: 'HardwareKitId',
                columns: [
                    { data: "HardwareKitNumber" },
                    { data: "ExtraQuantityPercentage" },
                    { data: "HardwareKitId" }
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'HardwareKit/Edit/' + row.HardwareKitId + '" >Edit</a> | <a href="' + ROOT_URL + 'HardwareKit/Details/' + row.HardwareKitId + '">Details</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableHardwareKit = new TableHardwareKit();

});