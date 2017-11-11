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
                    { data: "EquipmentName" },
                    { data: "PriorityId" }
                ],
                columnDefs: [
                    {
                        "targets": 3,
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