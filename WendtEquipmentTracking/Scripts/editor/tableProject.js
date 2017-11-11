$(function () {

    var TableProject = function () {

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
                    url: ROOT_URL + "api/ProjectApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'ProjectId'
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ProjectApi/Table",
                    dataSrc: ""
                },
                rowId: 'ProjectId',
                columnDefs: [
                    { data: "ProjectNumber", targets: 0},
                    { data: "FreightTerms", targets: 1 },
                    { data: "ShipToCompany", targets: 2,
                        render: function (data, type, row, meta) {
                            return row.ShipToCompany ? row.ShipToCompany + "<br/>" + row.ShipToAddress + "<br/>" + row.ShipToCSZ : '';
                        } 
                    },
                    { data: "ShipToContact1", targets: 3,
                        render: function (data, type, row, meta) {
                            return row.ShipToContact1 ? row.ShipToContact1 + "<br/>" + row.ShipToContact1PhoneFax + "<br/>" + row.ShipToContact1Email : '';
                        } 
                    },
                    { data: "ShipToContact2", targets: 3,
                        render: function (data, type, row, meta) {
                            return row.ShipToContact2 ? row.ShipToContact2 + "<br/>" + row.ShipToContact2PhoneFax + "<br/>" + row.ShipToContact2Email : '';
                        } 
                    },
                    { data: "ShipToBroker", targets: 4,
                        render: function (data, type, row, meta) {
                            return row.ShipToBroker ? row.ShipToBroker + "<br/>" + row.ShipToBrokerPhoneFax + "<br/>" + row.ShipToBrokerEmail : '';
                        } 
                    },
                    { data: "ShipToBrokerEmail", targets: 5 },
                    { data: "IsCustomsProject", targets: 6,
                        render: function (data, type, row, meta) {
                            return row.IsCustomsProject ? "Yes" : "No";
                        } 
                    },
                    { data: "IncludeSoftCosts", targets: 7,
                        render: function (data, type, row, meta) {
                            return row.IncludeSoftCosts ? "Yes" : "No";
                        } 
                    },
                    {
                        "targets": 8,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="' + ROOT_URL + 'Project/Edit/' + row.ProjectId + '" >Edit</a>' :'';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    }

    tableProject = new TableProject();

});