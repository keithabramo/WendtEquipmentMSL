$(function () {

    var TablePriority = function () {

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
                    url: ROOT_URL + "api/PriorityApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'PriorityId'
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/PriorityApi/Table",
                    dataSrc: ""
                },
                rowId: 'PriorityId',
                columns: [
                    { data: "PriorityNumber" },
                    { data: "DueDate" },
                    { data: "EndDate" },
                    { data: "ContractualShipDate" },
                    { data: "EquipmentName" },
                    { data: "PriorityId" }
                ],
                columnDefs: [
                    {
                        "targets": 0, className: "priorityWidth"
                    },
                    {
                        "targets": 1, className: "dateWidth",
                        render: function (data, type, row, meta) {
                            return row.DueDate ? moment(row.DueDate).format("MM/DD/YYYY") : '';
                        }
                    },
                    {
                        "targets": 2, className: "dateWidth",
                        render: function (data, type, row, meta) {
                            return row.EndDate ? moment(row.EndDate).format("MM/DD/YYYY") : '';
                        }
                    },
                    {
                        "targets": 3, className: "dateWidth",
                        render: function (data, type, row, meta) {
                            return row.ContractualShipDate ? moment(row.ContractualShipDate).format("MM/DD/YYYY") : '';
                        }
                    },
                    {
                        "targets": 5,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'Priority/Edit/' + row.PriorityId + '" >Edit</a> | <a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    }

    tablePriority = new TablePriority();

});