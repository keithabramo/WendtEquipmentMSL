﻿$(function () {

    var EditorTruckingSchedule = function () {

        this.snipping = false;

        this.columnIndexes = {
            Select: 0,
            Copy: 1,
            RequestDate: 2,
            ProjectNumber: 3,
            PurchaseOrder: 4,
            RequestedBy: 5,
            ShipFrom: 6,
            ShipTo: 7,
            Description: 8,
            NumPiecesText: 9,
            Dimensions: 10,
            WeightText: 11,
            Carrier: 12,
            PickUpDate: 13,
            Comments: 14,
            Status: 15,
            Delete: 16
        };
        this.editableColumns = [
            this.columnIndexes.RequestDate,
            this.columnIndexes.ProjectNumber,
            this.columnIndexes.PurchaseOrder,
            this.columnIndexes.RequestedBy,
            this.columnIndexes.ShipFrom,
            this.columnIndexes.ShipTo,
            this.columnIndexes.Description,
            this.columnIndexes.NumPiecesText,
            this.columnIndexes.Dimensions,
            this.columnIndexes.WeightText,
            this.columnIndexes.Carrier,
            this.columnIndexes.PickUpDate,
            this.columnIndexes.Comments,
            this.columnIndexes.Status
        ];
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var rfpStatus = $('#RFPStatusFilter').is(":checked");
                    var plannedStatus = $('#PlannedStatusFilter').is(":checked");
                    var confirmedStatus = $('#ConfirmedStatusFilter').is(":checked");
                    var hideClosedStatus = $('#HideClosedStatusFilter').is(":checked");

                    if (!rfpStatus && !plannedStatus && !confirmedStatus && !hideClosedStatus) {
                        return true;
                    } else {
                        var status = data[$this.columnIndexes.Status];

                        var availableStatuses = [];

                        rfpStatus && availableStatuses.push("RFP");
                        plannedStatus && availableStatuses.push("Planned");
                        confirmedStatus && availableStatuses.push("Confirmed");

                        //if none of the filters have been checked yet and hide closed is checked then add all except "closed"
                        availableStatuses = !availableStatuses.length && hideClosedStatus ? ["RFP", "Planned", "Confirmed"] : availableStatuses;

                        return availableStatuses.indexOf(status) > -1;
                    }
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex, rowData) {

                    if (!$this.snipping) {
                        return true;
                    } else {
                        return $this.editorMain.datatable.rows(dataIndex, { selected: true }).any();
                    }
                }
            );

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="RFPStatusFilter" /> RFP</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="PlannedStatusFilter" /> Planned</label>');
            $("div.custom").append('<br/>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="ConfirmedStatusFilter" /> Confirmed</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="HideClosedStatusFilter" checked="checked" /> Hide Closed</label>');

            var $customActions = $("<div class='custom-actions'></div>");
            $customActions.append('<span>Bulk Actions:</span>');
            $customActions.append('<button id="snipTable" class="btn btn-primary btn-xs btn-disabled" disabled="disabled" type="button">Snip Checked Rows For Email</button>');

            $("div.custom").append($customActions);

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

            $(".shipFrom-autocomplete").autocomplete({
                source: shipFromVendors,
                minLength: 0
            }).focus(function () {
                $(this).data("uiAutocomplete").search($(this).val());
            });

            $(".shipTo-autocomplete").autocomplete({
                source: shipToVendors,
                minLength: 0
            }).focus(function () {
                $(this).data("uiAutocomplete").search($(this).val());
            });

            $(".project-autocomplete").autocomplete({
                source: projects,
                minLength: 0,
                change: function (event, ui) {
                    if (ui.item) {
                        var project = $this.getProject(ui.item.value);

                        var shipToResults = $this.getShipToList(project);

                        $(".shipTo-autocomplete").autocomplete("option", "source", shipToResults);
                    }
                }
            }).focus(function () {
                $(this).data("uiAutocomplete").search($(this).val());
            });

            var clipboard = new ClipboardJS('.copy-snippet', {
                target: function (trigger) {
                    return $(".snippet-container")[0];
                }
            });

            clipboard.on('success', function (e) {

                $(".snip-alert").hide();

                e.clearSelection();

                $this.openEmail();

                var isIE11 = !!window.MSInputMethodContext && !!document.documentMode;
                if (isIE11) {
                    $(".snip-alert.alert-warning").show();
                } else {
                    $(".snip-alert.alert-success").show();
                }
            });

            clipboard.on('error', function (e) {

                $(".snip-alert").hide();

                e.clearSelection();

                $this.openEmail();

                $(".snip-alert.alert-danger").show();
            });

        };

        this.initEvents = function () {
            var $this = this;

            $('#RFPStatusFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#PlannedStatusFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#ConfirmedStatusFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#HideClosedStatusFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            this.editorMain.editor.on('preSubmit', function (e, data, action) {
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

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = $this.editorMain.editor.create(false);
                form.set('Carrier', $row.find("input[name='Carrier']").val());
                form.set('Comments', $row.find("input[name='Comments']").val());
                form.set('Description', $row.find("input[name='Description']").val());
                form.set('Dimensions', $row.find("input[name='Dimensions']").val());
                form.set('PurchaseOrder', $row.find("input[name='PurchaseOrder']").val());
                form.set('ShipFrom', $row.find("input[name='ShipFrom']").val());
                form.set('ShipTo', $row.find("input[name='ShipTo']").val());
                form.set('Status', $row.find("select[name='Status']").val());
                form.set('RequestedBy', $row.find("input[name='RequestedBy']").val());
                form.set('WeightText', $row.find("input[name='Weight']").val());
                form.set('NumPiecesText', $row.find("input[name='NumPieces']").val());
                form.set('PickUpDate', $row.find("input[name='PickUpDate']").val());
                form.set('RequestDate', $row.find("input[name='RequestDate']").val());
                form.set('ProjectNumber', $row.find("input[name='ProjectNumber']").val());
                
                form.submit();

                $this.editorMain.datatable.draw();
            });

            this.editorMain.editor.on('preOpen', function (e, mode, action) {

                var editable = true;

                if (action !== "remove") {
                    var columnIndex = $this.editorMain.editor.modifier().column;
                    var rowData = $this.editorMain.datatable.row($this.editorMain.editor.modifier().row).data();

                    if (columnIndex === $this.columnIndexes.ShipTo) {
                        var project = $this.getProject(rowData.ProjectNumber);

                        var shipToResults = $this.getShipToList(project);

                        this.field("ShipTo").update(shipToResults);
                    }

                    if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                        editable = false;
                    }
                }

                return editable;

            });

            this.editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();

                // If any of these cells can't be edited, set their values back to original
                $.each(cells, function (i, cell) {
                    var columnIndex = cell[0].index.column;

                    if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                        cell[0].set = cell[0].data;
                    }
                });

            });

            this.editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
                $createRow.find("select").prop('selectedIndex', 0);
                $createRow.find(".datePickerTable").datepicker("setDate", new Date());
            });

            $(".table").on("mousedown", "td.focus", function (e) {

                setTimeout(function () {
                    if ($(".dt-autofill-select").length) {
                        $this.editorMain.datatable.cell.blur();
                    }
                }, 100);
            });

            $('.table.my-datatable').on('click', ".copy", function () {

                var $row = $(this).closest('tr');
                var $createRow = $(".table.my-datatable tfoot tr");
                var rowData = $this.editorMain.datatable.row($row).data();

                $createRow.find("input[name='ProjectNumber']").val(rowData.ProjectNumber);

                var project = $this.getProject(rowData.ProjectNumber);
                var shipToResults = $this.getShipToList(project);
                $(".shipTo-autocomplete").autocomplete("option", "source", shipToResults);

                $createRow.find("input[name='Carrier']").val(rowData.Carrier);
                $createRow.find("input[name='Comments']").val(rowData.Comments);
                $createRow.find("input[name='Description']").val(rowData.Description);
                $createRow.find("input[name='Dimensions']").val(rowData.Dimensions);
                $createRow.find("input[name='PurchaseOrder']").val(rowData.PurchaseOrder);
                $createRow.find("input[name='ShipFrom']").val(rowData.ShipFrom);
                $createRow.find("input[name='ShipTo']").val(rowData.ShipTo)
                $createRow.find("select[name='Status']").val(rowData.Status);
                $createRow.find("input[name='RequestedBy']").val(rowData.RequestedBy);
                $createRow.find("input[name='Weight']").val(rowData.WeightText);
                $createRow.find("input[name='NumPieces']").val(rowData.NumPiecesText);
                $createRow.find("input[name='PickUpDate']").val(rowData.PickUpDate);
                $createRow.find("input[name='RequestDate']").val(rowData.RequestDate);

                window.scrollTo(0, document.body.scrollHeight);
            });

            $("#snipTable").on("click", function () {
                $("#copyModal").modal();

                $this.prepareTableForSnip.apply($this);

                // Turn table into image

                html2canvas(
                    $($this.editorMain.selector)[0],
                    {
                        scale: 1
                    }
                ).then(function (canvas) {

                    var dataURL = canvas.toDataURL();

                    $(".snippet-container").html("<img id='snip-image' src='" + dataURL + "'/>");

                    $this.simulateClick($(".copy-snippet")[0]);

                    $this.revertTableFromSnip.apply($this);

                }, function (reason) {
                    $this.revertTableFromSnip.apply($this);

                    $("#copyModal").modal('hide');

                    main.error("There was an issue creating this snippet.");
                });
            });

            $('#copyModal').on('hide.bs.modal', function (e) {
                $(".snippet-container").html('Loading...');
                $(".snip-alert").hide();
            });

            this.editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.updateCustomActions();
                }
            });

            this.editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.updateCustomActions();
                }
            });
        };

        this.initEditor = function () {

            this.editorMain.initEditor({
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
                        openOnFocus: true,
                        opts: {
                            source: projects,
                            minLength: 0,
                            focus: function (event, ui) {
                                $(this).val(ui.item.value);
                            }
                        }
                    }, {
                        name: "PurchaseOrder"
                    }, {
                        name: "RequestedBy"
                    }, {
                        name: "ShipFrom",
                        type: "autoComplete",
                        openOnFocus: true,
                        opts: {
                            source: shipFromVendors,
                            minLength: 0,
                            focus: function (event, ui) {
                                $(this).val(ui.item.value);
                            }
                        }
                    }, {
                        name: "ShipTo",
                        type: "autoComplete",
                        openOnFocus: true,
                        opts: {
                            source: shipToVendors,
                            minLength: 0,
                            focus: function (event, ui) {
                                $(this).val(ui.item.value);
                            }
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
                        name: "Status",
                        type: "select",
                        options: statuses,
                        placeholderDisabled: false,
                        placeholder: ""
                    }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/TruckingScheduleApi/Table",
                    dataSrc: ""
                },
                order: [[this.columnIndexes.RequestDate, 'asc']],
                rowId: 'TruckingScheduleId',
                keys: {
                    columns: this.editableColumns
                },
                autoFill: {
                    columns: this.editableColumns
                },
                autoWidth: false,
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                columnDefs: [
                    {
                        orderable: false,
                        searchable: false,
                        sortable: false,
                        className: 'select-checkbox',
                        targets: this.columnIndexes.Select,
                        render: function () {
                            return "<span></span>";
                        }
                    },
                    {
                        "targets": this.columnIndexes.Copy,
                        searchable: false,
                        orderable: false,
                        render: function (datadata, type, row, meta) {
                            return '<span title="Copy Row" class="copy glyphicon glyphicon-duplicate text-primary"></span>';
                        }
                    },
                    {
                        data: "RequestDate",
                        targets: this.columnIndexes.RequestDate,
                        type: "date",
                        className: "dateWidth"
                    },
                    {
                        data: "ProjectNumber",
                        targets: this.columnIndexes.ProjectNumber
                    },
                    {
                        data: "PurchaseOrder",
                        targets: this.columnIndexes.PurchaseOrder
                    },
                    {
                        data: "RequestedBy",
                        targets: this.columnIndexes.RequestedBy
                    },
                    {
                        data: "ShipFrom",
                        targets: this.columnIndexes.ShipFrom,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "ShipTo",
                        targets: this.columnIndexes.ShipTo,
                        className: "shippedToWidth"
                    },
                    {
                        data: "Description",
                        targets: this.columnIndexes.Description,
                        className: "truckingScheduleDescriptionWidth"
                    },
                    {
                        data: "NumPiecesText",
                        targets: this.columnIndexes.NumPiecesText
                    },
                    {
                        data: "Dimensions",
                        targets: this.columnIndexes.Dimensions,
                        className: "dimensionsWidth"
                    },
                    {
                        data: "WeightText",
                        targets: this.columnIndexes.WeightText
                    },
                    {
                        data: "Carrier",
                        targets: this.columnIndexes.Carrier
                    },
                    {
                        data: "PickUpDate",
                        targets: this.columnIndexes.PickUpDate,
                        type: "date",
                        className: "dateWidth"
                    },
                    {
                        data: "Comments",
                        targets: this.columnIndexes.Comments,
                        className: "commentsWidth"
                    },
                    {
                        data: "Status",
                        targets: this.columnIndexes.Status
                    },
                    {
                        "targets": this.columnIndexes.Delete,
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

            projectLabel = (project.ShipToCompany || "") + (project.ShipToAddress ? " " + project.ShipToAddress : "") + (project.ShipToCSZ ? " " + project.ShipToCSZ : "") + (project.ReceivingHours ? " : " + project.ReceivingHours : "");

            var projectOption = {
                value: projectLabel,
                label: projectLabel
            };

            var shipToResults = [projectOption].concat(shipToVendors);

            return shipToResults;
        };

        this.updateCustomActions = function () {
            var selectedRowsCount = this.editorMain.datatable.rows({ selected: true }).indexes().length;

            if (selectedRowsCount) {
                $(".custom-actions button, .custom-actions a").removeAttr("disabled");
            } else {
                $(".custom-actions button, .custom-actions a").attr("disabled", "disabled");
            }
        };

        this.openEmail = function () {
            var link = document.createElement('a');
            link.href = "mailto:?body=%0D%0A%0D%0A%0D%0A%0D%0A";

            document.body.appendChild(link);

            link.click();

            $(link).remove();
        };

        this.prepareTableForSnip = function () {
            // Filter datatable
            this.snipping = true;
            this.editorMain.datatable.draw();

            // Modify table to only show snipping stuff
            $(this.editorMain.selector).find("thead tr").first().hide();
            $(this.editorMain.selector).find("tfoot").hide();
            $(this.editorMain.selector).find("tbody tr.selected").removeClass("selected");

            this.editorMain.datatable.columns([
                this.columnIndexes.Select,
                this.columnIndexes.Copy,
                this.columnIndexes.Delete
            ]).visible(false);

            
        };

        this.revertTableFromSnip = function () {
            this.snipping = false;

            this.editorMain.datatable.columns([
                this.columnIndexes.Select,
                this.columnIndexes.Copy,
                this.columnIndexes.Delete
            ]).visible(true);

            $(this.editorMain.selector).find("thead tr").first().show();
            $(this.editorMain.selector).find("tfoot").show();
            $(this.editorMain.selector).find("tbody tr").addClass("selected");

            this.editorMain.datatable.draw();

        };

        this.simulateClick = function (elem) {
            // Create our event (with options)
            var evt = new MouseEvent('click', {
                bubbles: true,
                cancelable: true,
                view: window
            });
            // If cancelled, don't dispatch our event
            var canceled = !elem.dispatchEvent(evt);
        };

        this.initStyles();
        this.initEvents();
    };

    editorTruckingSchedule = new EditorTruckingSchedule();

});