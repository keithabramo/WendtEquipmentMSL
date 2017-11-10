$(function () {

    var TableBOL = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;
        }

        this.initEditor = function () {

            tableMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'BillOfLadingId'
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/Table",
                    dataSrc: ""
                },
                rowId: 'BillOfLadingId',
                columns: [
                    { data: "BillOfLadingNumber" },
                    { data: "ToStorage" },
                    { data: "DateShipped" },
                    { data: "FreightTerms" },
                    { data: "Carrier" },
                    { data: "TrailerNumber" },
                    { data: "BillOfLadingId" }
                ],
                columnDefs: [
                    {
                        "targets": 1,
                        render: function (data, type, row, meta) {
                            return row.ToStorage ? "Yes" : "No";
                        }
                    },
                    
                    {
                        "targets": 2,
                        render: function (data, type, row, meta) {
                            return row.DateShipped ? moment(row.DateShipped).format("DD/MM/YYYY") : '';
                        }
                    },
                    {
                        "targets": 6,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'BillOfLading/Edit/' + row.BillOfLadingId + '" >Edit</a> | <a href="' + ROOT_URL + 'BillOfLading/Details/' + row.BillOfLadingId + '">Details</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    }

    tableBOL = new TableBOL();

});