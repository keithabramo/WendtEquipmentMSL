$(function () {

    var TableBOL = function () {

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
                    url: ROOT_URL + "api/BillOfLadingApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'BillOfLadingId'
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/Table",
                    dataSrc: ""
                },
                rowId: 'BillOfLadingId',
                columns: [
                    { data: "BillOfLadingNumber" },
                    { data: "DateShipped" },
                    { data: "Carrier" },
                    { data: "TrailerNumber" },
                    { data: "ShippedFrom" },
                    { data: "ShippedTo" },
                    { data: "FreightTerms" },
                    { data: "BillOfLadingId" },
                    { data: "ToStorage" },
                ],
                columnDefs: [
                    {
                        "targets": 8,
                        render: function (data, type, row, meta) {
                            return row.ToStorage ? "Yes" : "No";
                        }
                    },

                    {
                        "targets": 1, type: "date",
                        render: function (data, type, row, meta) {
                            return row.DateShipped ? moment(row.DateShipped).format("M/D/YY") : '';
                        }
                    },
                    {
                        "targets": 7,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'BillOfLading/Edit/' + row.BillOfLadingId + '" >Edit</a> | <a href="' + ROOT_URL + 'BillOfLading/Details/' + row.BillOfLadingId + '">Details</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableBOL = new TableBOL();

});