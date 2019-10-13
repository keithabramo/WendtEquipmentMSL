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

            $('#billOfLadingAttachmentModal').on('show.bs.modal', function (e) {
                var billofladingid = $(e.relatedTarget).attr("data-billofladingid");

                tableBOLAttachment.init(billofladingid);
            });
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
                    { data: "ToStorage" }
                ],
                columnDefs: [

                    { data: "BillOfLadingNumber", "targets": 0 },
                    {
                        data: "DateShipped", "targets": 1, type: "date",
                        render: function (data, type, row, meta) {
                            return row.DateShipped ? moment(row.DateShipped).format("M/D/YY") : '';
                        }
                    },
                    { data: "Carrier", "targets": 2 },
                    { data: "TrailerNumber", "targets": 3 },
                    { data: "ShippedFrom", "targets": 4 },
                    { data: "ShippedTo", "targets": 5 },
                    { data: "FreightTerms", "targets": 6 },
                    {
                        data: "BillOfLadingId", "targets": 7,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'BillOfLading/Edit/' + row.BillOfLadingId + '" >Edit</a> | <a href="' + ROOT_URL + 'BillOfLading/Details/' + row.BillOfLadingId + '">Details</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    },
                    {
                        "targets": 8,
                        searchable: false,
                        orderable: false,
                        render: function (datadata, type, row, meta) {
                            return '<a data-billofladingid="' + row.BillOfLadingId + '" href="javascript:void(0);" class="attachments" data-toggle="modal" data-toggle="modal" data-target="#billOfLadingAttachmentModal">Attachments</a>';
                        }
                    },
                    {
                        data: "ToStorage", "targets": 9,
                        render: function (data, type, row, meta) {
                            return row.ToStorage ? "Yes" : "No";
                        }
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableBOL = new TableBOL();

});