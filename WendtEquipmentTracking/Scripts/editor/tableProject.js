$(function () {

    var TableProject = function () {

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
                    url: ROOT_URL + "api/ProjectApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'ProjectId'
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ProjectApi/Table",
                    dataSrc: ""
                },
                rowId: 'ProjectId',
                order: [[1, 'asc']],
                columnDefs: [
                    {
                        "targets": 0,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'Project/Edit/' + row.ProjectId + '" >Edit</a>' : '';
                        },
                        className: "text-nowrap"
                    },
                    { data: "ProjectNumber", targets: 1 },
                    { data: "PM", targets: 2 },
                    { data: "FreightTerms", targets: 3, className: "text-nowrap" },
                    {
                        data: "ShipToCompany", targets: 4,
                        render: function (data, type, row, meta) {
                            return row.ShipToCompany ? "<span class='text-nowrap'>" + row.ShipToCompany + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToAddress || '') + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToCSZ || '') + "</span>" : '';
                        }
                    },
                    { data: "ReceivingHours", targets: 5 },
                    {
                        data: "ShipToContact1", targets: 6,
                        render: function (data, type, row, meta) {
                            return row.ShipToContact1 ? "<span class='text-nowrap'>" + row.ShipToContact1 + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToContact1PhoneFax || '') + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToContact1Email || '') + "</span>" : '';
                        }
                    },
                    {
                        data: "ShipToContact2", targets: 7,
                        render: function (data, type, row, meta) {
                            return row.ShipToContact2 ? "<span class='text-nowrap'>" + row.ShipToContact2 + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToContact2PhoneFax || '') + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToContact2Email || '') + "</span>" : '';
                        }
                    },
                    { data: "Notes", targets: 8 },
                    {
                        data: "ShipToBroker", targets: 9,
                        render: function (data, type, row, meta) {
                            return row.ShipToBroker ? "<span class='text-nowrap'>" + row.ShipToBroker + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToBrokerPhoneFax || '') + "</span>" + "<br/>" + "<span class='text-nowrap'>" + (row.ShipToBrokerEmail || '') + "</span>" : '';
                        }
                    },
                    {
                        data: "IsCustomsProject", targets: 10,
                        render: function (data, type, row, meta) {
                            return row.IsCustomsProject ? "Yes" : "No";
                        }
                    },
                    {
                        data: "IncludeSoftCosts", targets: 11,
                        render: function (data, type, row, meta) {
                            return row.IncludeSoftCosts ? "Yes" : "No";
                        }
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableProject = new TableProject();

});