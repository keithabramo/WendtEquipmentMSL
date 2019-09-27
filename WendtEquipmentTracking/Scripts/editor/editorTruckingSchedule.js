$(function () {

    var EditorTruckingSchedule = function () {

        this.initStyles = function () {
            var $this = this;

            

            this.initEditor();
            this.initDatatable();

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

        };

        this.initEvents = function () {
            var $this = this;

            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    //var name = this.field('Name');

                    //// Only validate user input values - different values indicate that
                    //// the end user has not entered a value
                    //if (!name.isMultiValue()) {
                    //    if (!name.val()) {
                    //        name.error('A vendor name is required');
                    //    }
                    //}


                    //// If any error was reported, cancel the submission so it can be corrected
                    //if (this.inError()) {
                    //    return false;
                    //}
                }
            });

            //editorMain.editor.on('postEdit', function (e, json, data) {

            //    var row = editorMain.datatable.row("#" + data.VendorId);

            //    var project = $this.getProject(data.ProjectNumber);
            //    var projectOption = {
            //        value: project.ProjectId,
            //        label: project.ProjectNumber,
            //        desc: project.ShippedToAddress
            //    };

            //    var shipToResults = [projectOption].concat(results);

            //    row.

            //});

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = editorMain.editor.create(false);
                form.set('Carrier', $row.find("input[name='Carrier']").val());
                form.set('Comments', $row.find("input[name='Comments']").val());
                form.set('Description', $row.find("input[name='Description']").val());
                form.set('Dimensions', $row.find("input[name='Dimensions']").val());
                form.set('PurchaseOrder', $row.find("input[name='PurchaseOrder']").val());
                form.set('ShipFrom', $row.find("input[name='ShipFrom']").val());
                form.set('ShipTo', $row.find("input[name='ShipTo']").val());
                form.set('Status', $row.find("input[name='Status']").val());
                form.set('RequestedBy', $row.find("input[name='RequestedBy']").val());
                form.set('WorkOrder', $row.find("input[name='WorkOrder']").val());
                form.set('WeightText', $row.find("input[name='WeightText']").val());
                form.set('NumPiecesText', $row.find("input[name='NumPiecesText']").val());
                form.set('PickUpDate', $row.find("input[name='PickUpDate']").val());
                form.set('RequestDate', $row.find("input[name='RequestDate']").val());
                form.set('ProjectNumber', $row.find("input[name='ProjectNumber']").val());
                
                form.submit();

                editorMain.datatable.draw();
            });

            editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
            });
        };

        this.initEditor = function () {

            editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/TruckingScheduleApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'TruckingScheduleId',
                fields: [
                    {
                        name: "TruckingScheduleId"
                    }, {
                        name: "RequestDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    }, {
                        name: "ProjectNumber"
                    }, {
                        name: "WorkOrder"
                    }, {
                        name: "PurchaseOrder"
                    }, {
                        name: "RequestedBy"
                    }, {
                        name: "ShipFrom",
                        type: "autoComplete",
                        "opts": {
                            "source": vendors
                        }
                    }, {
                        name: "ShipTo",
                        type: "autoComplete",
                        "opts": {
                            "source": vendors
                        }
                    }, {
                        name: "Description"
                    }, {
                        name: "NumPiecesText"
                    }, {
                        name: "Dimensions"
                    }, {
                        name: "WeightText"
                    }, {
                        name: "Carrier"
                    }, {
                        name: "PickUpDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    }, {
                        name: "Comments"
                    }, {
                        name: "Status"
                    }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/TruckingScheduleApi/Table",
                    dataSrc: ""
                },
                rowId: 'TruckingScheduleId',
                columnDefs: [
                    {
                        "targets": 0,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return '<span title="Copy Row" class="copy glyphicon glyphicon-duplicate text-primary"></span>';
                        }
                    },
                    {
                        data: "RequestDate",
                        targets: 1
                    },
                    {
                        data: "ProjectNumber",
                        targets: 2
                    },
                    {
                        data: "WorkOrder",
                        targets: 3
                    },
                    {
                        data: "PurchaseOrder",
                        targets: 4
                    },
                    {
                        data: "RequestedBy",
                        targets: 5
                    },
                    {
                        data: "ShipFrom",
                        targets: 6
                    },
                    {
                        data: "ShipTo",
                        targets: 7
                    },
                    {
                        data: "Description",
                        targets: 8
                    },
                    {
                        data: "NumPiecesText",
                        targets: 9
                    },
                    {
                        data: "Dimensions",
                        targets: 10
                    },
                    {
                        data: "WeightText",
                        targets: 11
                    },
                    {
                        data: "Carrier",
                        targets: 12
                    },
                    {
                        data: "PickUpDate",
                        targets: 13
                    },
                    {
                        data: "Comments",
                        targets: 14
                    },
                    {
                        data: "Status",
                        targets: 15
                    },
                    {
                        "targets": 16,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    }
                ]
            });
        };

        this.getProject = function (projectNumber) {

            var project = {};

            $.ajax({
                url: ROOT_URL + "api/ProjectApi/FindByProjectNumber" + projectNumber,
                async: false,
                success: function (results) {

                    project = results;
                }
            });

            return project;
        };

        this.initStyles();
        this.initEvents();
    };

    editorTruckingSchedule = new EditorTruckingSchedule();

});