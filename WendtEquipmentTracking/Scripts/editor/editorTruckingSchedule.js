$(function () {

    var EditorTruckingSchedule = function () {

        this.editableColumns = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

            $(".shipFrom-autocomplete").autocomplete({
                source: shipFromVendors,
                minLength: 0
            });

            $(".shipTo-autocomplete").autocomplete({
                source: shipToVendors,
                minLength: 0
            });

            $(".project-autocomplete").autocomplete({
                source: projects,
                minLength: 0,
                change: function (event, ui) {
                    var project = $this.getProject(ui.item.value);

                    var shipToResults = $this.getShipToList(project);

                    $(".shipTo-autocomplete").autocomplete("option", "source", shipToResults);
                }
            });

        };

        this.initEvents = function () {
            var $this = this;

            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    //var equipmentName = this.field('EquipmentName');
                    //var releaseDate = this.field('ReleaseDate');
                    //var drawingNumber = this.field('DrawingNumber');
                    //var workOrderNumber = this.field('WorkOrderNumber');
                    //var shippingTagNumber = this.field('ShippingTagNumber');
                    //var description = this.field('Description');
                    //var unitWeight = this.field('UnitWeightText');
                    //var quantity = this.field('Quantity');
                    //var readyToShip = this.field('ReadyToShip');

                    var error = false;
                    if (action === "edit") {

                        //if (!equipmentName.isMultiValue()) {
                        //    if (!equipmentName.val()) {
                        //        equipmentName.error('The equipment field is required');
                        //    }
                        //}

                        //if (!releaseDate.isMultiValue()) {
                        //    if (!releaseDate.val()) {
                        //        releaseDate.error('The release date field is required');
                        //    } else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                        //        releaseDate.error('The release date must be in the format m/d/yy');
                        //    }
                        //}

                        //if (!drawingNumber.isMultiValue()) {
                        //    if (!drawingNumber.val()) {
                        //        drawingNumber.error('The drawing # field is required');
                        //    }
                        //}

                        //if (!workOrderNumber.isMultiValue()) {
                        //    if (!workOrderNumber.val()) {
                        //        workOrderNumber.error('The work order # field is required');
                        //    }
                        //}

                        //if (!shippingTagNumber.isMultiValue()) {
                        //    if (!shippingTagNumber.val()) {
                        //        shippingTagNumber.error('The ship tag # field is required');
                        //    }
                        //}

                        //if (!description.isMultiValue()) {
                        //    if (!description.val()) {
                        //        description.error('The description field is required');
                        //    }
                        //}

                        //if (!unitWeight.isMultiValue()) {
                        //    if (!unitWeight.val()) {
                        //        unitWeight.error('The unit weight field is required');
                        //    } else if (isNaN(unitWeight.val())) {
                        //        unitWeight.error('The unit weight is not a valid number');
                        //    }
                        //}

                        //if (!quantity.isMultiValue()) {
                        //    if (!quantity.val()) {
                        //        quantity.error('The quantity field is required');
                        //    } else if (isNaN(quantity.val())) {
                        //        quantity.error('The quantity is not a valid number');
                        //    }
                        //}

                        //if (!readyToShip.isMultiValue()) {
                        //    if (isNaN(readyToShip.val())) {
                        //        readyToShip.error('The ready to ship field is not a valid number');
                        //    }
                        //}

                        error = this.inError();
                    } else {

                        //if (!equipmentName.val()) {
                        //    $("#EquipmentName").siblings("span").html('The Equipment field is required.').show();
                        //    error = true;
                        //} else {
                        //    $("#EquipmentName").siblings("span").html('').hide();
                        //}

                        //if (!releaseDate.val()) {
                        //    $("#releaseDate").siblings("span").html('The release date field is required').show();
                        //    error = true;
                        //} else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                        //    $("#releaseDate").siblings("span").html('The release date must be in the format m/d/yy').show();
                        //    error = true;
                        //} else {
                        //    $("#releaseDate").siblings("span").html('').hide();
                        //}
                        //if (!drawingNumber.val()) {
                        //    $("#DrawingNumber").siblings("span").html('The drawing # field is required').show();
                        //    error = true;
                        //} else {
                        //    $("#DrawingNumber").siblings("span").html('').hide();
                        //}
                        //if (!workOrderNumber.val()) {
                        //    $("#WorkOrderNumber").siblings("span").html('The work order # field is required').show();
                        //    error = true;
                        //} else {
                        //    $("#WorkOrderNumber").siblings("span").html('').hide();
                        //}
                        //if (!shippingTagNumber.val()) {
                        //    $("#ShippingTagNumber").siblings("span").html('The ship tag # field is required').show();
                        //    error = true;
                        //} else {
                        //    $("#ShippingTagNumber").siblings("span").html('').hide();
                        //}
                        //if (!description.val()) {
                        //    $("#Description").siblings("span").html('The description field is required').show();
                        //    error = true;
                        //} else {
                        //    $("#Description").siblings("span").html('').hide();
                        //}
                        //if (!unitWeight.val()) {
                        //    $("#UnitWeight").siblings("span").html('The unit weight date field is required').show();
                        //    error = true;
                        //} else if (isNaN(unitWeight.val())) {
                        //    $("#UnitWeight").siblings("span").html('The unit weight is not a valid number').show();
                        //    error = true;
                        //} else {
                        //    $("#UnitWeight").siblings("span").html('').hide();
                        //}
                        //if (!quantity.val()) {
                        //    $("#Quantity").siblings("span").html('The quantity field is required').show();
                        //    error = true;
                        //} else if (isNaN(quantity.val())) {
                        //    $("#Quantity").siblings("span").html('The quantity is not a valid number').show();
                        //    error = true;
                        //} else {
                        //    $("#Quantity").siblings("span").html('').hide();
                        //}
                        //if (isNaN(readyToShip.val())) {
                        //    $("#ReadyToShip").siblings("span").html('The ready to ship field is not a valid number').show();
                        //    error = true;
                        //} else {
                        //    $("#ReadyToShip").siblings("span").html('').hide();
                        //}
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (error) {
                        return false;
                    }
                }
            });


            editorMain.editor.on('postEdit', function (e, json, data) {
                var project = $this.getProject(data.ProjectNumber);

                var shipToResults = $this.getShipToList(project);

                this.field("ShipTo").update(shipToResults);

            });

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

            editorMain.editor.on('preOpen', function (e, mode, action) {

                var editable = true;

                if (action !== "remove") {
                    var columnIndex = editorMain.editor.modifier().column;

                    if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                        editable = false;
                    }
                }

                return editable;

            });

            editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();

                // If any of these cells can't be edited, set their values back to original
                $.each(cells, function (i, cell) {
                    var columnIndex = cell[0].index.column;

                    if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                        cell[0].set = cell[0].data;
                    }
                });

            });

            editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
            });

            $(".table").on("mousedown", "td.focus", function (e) {

                setTimeout(function () {
                    if ($(".dt-autofill-select").length) {
                        editorMain.datatable.cell.blur();
                    }
                }, 100);
            });

            $('.table.my-datatable').on('click', ".copy", function () {

                var $row = $(this).closest('tr');
                var $createRow = $(".table.my-datatable tfoot tr");
                var rowData = editorMain.datatable.row($row).data();

                $createRow.find("input[name='Carrier']").val(rowData.Carrier);
                $createRow.find("select[name='Comments']").val(rowData.Comments);
                $createRow.find("select[name='Description']").val(rowData.Description);
                $createRow.find("select[name='Dimensions']").val(rowData.Dimensions);
                $createRow.find("select[name='PurchaseOrder']").val(rowData.PurchaseOrder);
                $createRow.find("select[name='ShipFrom']").val(rowData.ShipFrom);
                $createRow.find("select[name='ShipTo']").val(rowData.ShipTo);
                $createRow.find("select[name='Status']").val(rowData.Status);
                $createRow.find("select[name='RequestedBy']").val(rowData.RequestedBy);
                $createRow.find("select[name='WorkOrder']").val(rowData.WorkOrder);
                $createRow.find("select[name='WeightText']").val(rowData.WeightText);
                $createRow.find("select[name='NumPiecesText']").val(rowData.NumPiecesText);
                $createRow.find("select[name='PickUpDate']").val(rowData.PickUpDate);
                $createRow.find("select[name='RequestDate']").val(rowData.RequestDate);
                $createRow.find("select[name='ProjectNumber']").val(rowData.ProjectNumber);

                window.scrollTo(0, document.body.scrollHeight);
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
                        name: "ProjectNumber",
                        type: "autoComplete",
                        opts: {
                            source: projects,
                            minLength: 0
                        }
                    }, {
                        name: "WorkOrder"
                    }, {
                        name: "PurchaseOrder"
                    }, {
                        name: "RequestedBy"
                    }, {
                        name: "ShipFrom",
                        type: "autoComplete",
                        opts: {
                            source: shipFromVendors,
                            minLength: 0
                        }
                    }, {
                        name: "ShipTo",
                        type: "autoComplete",
                        opts: {
                            source: shipToVendors,
                            minLength: 0
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
                order: [[1, 'asc']],
                rowId: 'TruckingScheduleId',
                keys: {
                    columns: this.editableColumns
                },
                autoFill: {
                    columns: this.editableColumns
                },
                createdRow: function (row, data, index) {

                    if (data.ProjectNumber) {

                        var project = $this.getProject(data.ProjectNumber);

                        var shipToResults = $this.getShipToList(project);

                        editorMain.editor.field("ShipTo").update(shipToResults);
                    }
                },
                columnDefs: [
                    {
                        "targets": 0,
                        searchable: false,
                        orderable: false,
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
                url: ROOT_URL + "api/ProjectApi/FindByProjectNumber/",
                data: {
                    projectNumber: projectNumber
                },
                method: "POST",
                async: false,
                success: function (results) {

                    project = results;
                }
            });

            return project;
        };

        this.getShipToList = function (project) {

            projectLabel = (project.ShipToCompany || "") + (project.ShipToAddress ? " " + project.ShipToAddress : "") + (project.ShipToCSZ ? " " + project.ShipToCSZ : "");

            var projectOption = {
                value: projectLabel,
                label: projectLabel
            };

            var shipToResults = [projectOption].concat(shipToVendors);

            return shipToResults;
        };

        this.initStyles();
        this.initEvents();
    };

    editorTruckingSchedule = new EditorTruckingSchedule();

});