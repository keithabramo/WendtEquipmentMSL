$(function () {

    var EditorMSL = function () {

        this.snipping = false;
        this.hideColumns = false;

        this.columnIndexes = {
            Select: 0,
            Copy: 1,
            EquipmentName: 2,
            PriorityNumber: 3,
            ReleaseDate: 4,
            DrawingNumber: 5,
            Revision: 6,
            WorkOrderNumber: 7,
            Quantity: 8,
            ShippingTagNumber: 9,
            Description: 10,
            UnitWeightText: 11,
            TotalWeight: 12,
            TotalWeightShipped: 13,
            ReadyToShip: 14,
            ShippedQuantity: 15,
            LeftToShip: 16,
            FullyShippedText: 17,
            ShippedFrom: 18,
            Notes: 19,
            CustomsValueText: 20,
            SalePriceText: 21,
            HTSCode: 22,
            CountryOfOrigin: 23,
            BillOfLadingNumbers: 24,
            LatestBOLDateShipped: 25,
            Attachments: 26,
            HasErrorsText: 27,
            IsAssociatedToHardwareKitText: 28,
            EquipmentId: 29,
            IsDuplicateText: 30,
            AttachmentCount: 31
        };
        this.editableColumns = [
            this.columnIndexes.EquipmentName,
            this.columnIndexes.PriorityNumber,
            this.columnIndexes.ReleaseDate,
            this.columnIndexes.DrawingNumber,
            this.columnIndexes.WorkOrderNumber,
            this.columnIndexes.Quantity,
            this.columnIndexes.ShippingTagNumber,
            this.columnIndexes.Description,
            this.columnIndexes.UnitWeightText,
            this.columnIndexes.ReadyToShip,
            this.columnIndexes.ShippedFrom,
            this.columnIndexes.Notes,
            this.columnIndexes.HTSCode,
            this.columnIndexes.CountryOfOrigin
        ];
        this.alwaysEditableColumns = [
            this.columnIndexes.Notes,
            this.columnIndexes.HTSCode,
            this.columnIndexes.CountryOfOrigin
        ];
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            timer.startTimer(60 * 15); //15 Mins

            this.initEditor();
            this.initDatatable();

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterWorkInProgress = $('#workInProgressFilter').is(":checked");

                    if (!filterWorkInProgress) {
                        return true;
                    } else {
                        var leftToShip = data[$this.columnIndexes.LeftToShip];
                        var isAssociatedToHardwareKit = data[$this.columnIndexes.IsAssociatedToHardwareKitText];

                        if (leftToShip && parseInt(leftToShip, 10) > 0 && isAssociatedToHardwareKit === "False") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var fullyShipped = data[$this.columnIndexes.FullyShippedText];
                        var readyToShip = data[$this.columnIndexes.ReadyToShip];
                        var isAssociatedToHardwareKit = data[$this.columnIndexes.IsAssociatedToHardwareKitText];

                        if (fullyShipped === "NO" && readyToShip && parseInt(readyToShip, 10) > 0 && isAssociatedToHardwareKit === "False") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var hardwareFilter = $('#hardwareFilter').is(":checked");

                    if (!hardwareFilter) {
                        return true;
                    } else {
                        var equipment = data[$this.columnIndexes.EquipmentName];

                        if (equipment.toLowerCase() !== "hardware") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var errorFilter = $('#errorFilter').is(":checked");

                    var isDuplicateText = data[$this.columnIndexes.IsDuplicateText];
                    var hasErrorsText = data[$this.columnIndexes.HasErrorsText];


                    if (!errorFilter) {
                        return true;
                    } else {
                        return hasErrorsText == "True" || isDuplicateText == "True";
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

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Ready To Ship</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="workInProgressFilter" /> Work In Progress</label>');
            $("div.custom").append('<br/>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="hardwareFilter" /> Hide Hardware</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="errorFilter" />Rows With Errors</label>');

            var $customActions = $("<div class='custom-actions'></div>");
            $customActions.append('<span>Bulk Actions:</span>');
            $customActions.append('<button id="snipTable" class="btn btn-primary btn-xs btn-disabled" disabled="disabled" type="button">Snip Checked Rows For Email</button>');
            $customActions.append('<button id="deleteRecords" class="btn btn-danger btn-xs btn-disabled" disabled="disabled" type="button">Delete Checked Lines</button>');

            $("div.custom").append($customActions);

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

            $(".workorderprice-autocomplete").autocomplete({
                source: "/api/AutocompleteApi/WorkOrderPrice"
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

            $('#readyToShipFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#workInProgressFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#hardwareFilter').on("change", function () {
                $this.editorMain.datatable.draw();
            });

            $('#errorFilter').on("change", function () {
                $this.editorMain.datatable.draw();
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

                    main.error("There was an issue creating this equipment snippet.");
                });
            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = $this.editorMain.editor.create(false);
                form.set('EquipmentName', $row.find("input[name='EquipmentName']").val());
                form.set('PriorityNumber', $row.find("select[name='PriorityNumber']").val());
                form.set('ReleaseDate', $row.find("input[name='ReleaseDate']").val());
                form.set('DrawingNumber', $row.find("textarea[name='DrawingNumber']").val());
                form.set('WorkOrderNumber', $row.find("input[name='WorkOrderNumber']").val());
                form.set('Quantity', $row.find("input[name='Quantity']").val());
                form.set('ShippingTagNumber', $row.find("textarea[name='ShippingTagNumber']").val());
                form.set('Description', $row.find("textarea[name='Description']").val());
                form.set('UnitWeightText', $row.find("input[name='UnitWeight']").val());
                form.set('ReadyToShip', $row.find("input[name='ReadyToShip']").val());
                form.set('ShippedFrom', $row.find("input[name='ShippedFrom']").val());
                form.set('HTSCode', $row.find("input[name='HTSCode']").val());
                form.set('CountryOfOrigin', $row.find("input[name='CountryOfOrigin']").val());
                form.set('Notes', $row.find("textarea[name='Notes']").val());
                form.set('Revision', $row.find("input[name='Revision']").val());
                form.submit();

                $this.editorMain.datatable.draw();
            });

            $('.table.my-datatable').on('click', ".copy", function () {

                var $row = $(this).closest('tr');
                var $createRow = $(".table.my-datatable tfoot tr");
                var rowData = $this.editorMain.datatable.row($row).data();

                $createRow.find("input[name='EquipmentName']").val(rowData.EquipmentName);
                $createRow.find("select[name='PriorityNumber']").val(rowData.PriorityNumber);
                $createRow.find("input[name='ReleaseDate']").val(moment().format("M/D/YY"));
                $createRow.find("textarea[name='DrawingNumber']").val(rowData.DrawingNumber);
                $createRow.find("input[name='WorkOrderNumber']").val(rowData.WorkOrderNumber);
                $createRow.find("input[name='Quantity']").val(rowData.Quantity);
                $createRow.find("textarea[name='ShippingTagNumber']").val(rowData.ShippingTagNumber);
                $createRow.find("textarea[name='Description']").val(rowData.Description);
                $createRow.find("input[name='UnitWeight']").val(rowData.UnitWeightText);
                $createRow.find("input[name='ReadyToShip']").val(rowData.ReadyToShip);
                $createRow.find("input[name='ShippedFrom']").val(rowData.ShippedFrom);
                $createRow.find("input[name='HTSCode']").val(rowData.HTSCode);
                $createRow.find("input[name='CountryOfOrigin']").val(rowData.CountryOfOrigin);
                $createRow.find("textarea[name='Notes']").val(rowData.Notes);

                window.scrollTo(0, document.body.scrollHeight);
            });

            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var equipmentName = this.field('EquipmentName');
                    var releaseDate = this.field('ReleaseDate');
                    var drawingNumber = this.field('DrawingNumber');
                    var workOrderNumber = this.field('WorkOrderNumber');
                    var shippingTagNumber = this.field('ShippingTagNumber');
                    var description = this.field('Description');
                    var unitWeight = this.field('UnitWeightText');
                    var quantity = this.field('Quantity');
                    var readyToShip = this.field('ReadyToShip');
                    var revision = this.field('Revision');

                    var error = false;
                    if (action === "edit") {

                        if (!equipmentName.isMultiValue()) {
                            if (!equipmentName.val()) {
                                equipmentName.error('The equipment field is required');
                            }
                        }

                        if (!releaseDate.isMultiValue()) {
                            if (!releaseDate.val()) {
                                releaseDate.error('The release date field is required');
                            } else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                                releaseDate.error('The release date must be in the format m/d/yy');
                            }
                        }

                        if (!drawingNumber.isMultiValue()) {
                            if (!drawingNumber.val()) {
                                drawingNumber.error('The drawing # field is required');
                            }
                        }

                        if (!workOrderNumber.isMultiValue()) {
                            if (!workOrderNumber.val()) {
                                workOrderNumber.error('The work order # field is required');
                            }
                        }

                        if (!shippingTagNumber.isMultiValue()) {
                            if (!shippingTagNumber.val()) {
                                shippingTagNumber.error('The ship tag # field is required');
                            }
                        }

                        if (!description.isMultiValue()) {
                            if (!description.val()) {
                                description.error('The description field is required');
                            }
                        }

                        if (!unitWeight.isMultiValue()) {
                            if (!unitWeight.val()) {
                                unitWeight.error('The unit weight field is required');
                            } else if (isNaN(unitWeight.val())) {
                                unitWeight.error('The unit weight is not a valid number');
                            }
                        }

                        if (!quantity.isMultiValue()) {
                            if (!quantity.val()) {
                                quantity.error('The quantity field is required');
                            } else if (isNaN(quantity.val())) {
                                quantity.error('The quantity is not a valid number');
                            }
                        }

                        if (!readyToShip.isMultiValue()) {
                            if (isNaN(readyToShip.val())) {
                                readyToShip.error('The ready to ship field is not a valid number');
                            }
                        }

                        error = this.inError();
                    } else {

                        if (!equipmentName.val()) {
                            $("#EquipmentName").siblings("span").html('The Equipment field is required.').show();
                            error = true;
                        } else {
                            $("#EquipmentName").siblings("span").html('').hide();
                        }

                        if (!releaseDate.val()) {
                            $("#releaseDate").siblings("span").html('The release date field is required').show();
                            error = true;
                        } else if (!moment(releaseDate.val(), 'M/D/YY', true).isValid()) {
                            $("#releaseDate").siblings("span").html('The release date must be in the format m/d/yy').show();
                            error = true;
                        } else {
                            $("#releaseDate").siblings("span").html('').hide();
                        }

                        if (!drawingNumber.val()) {
                            $("#DrawingNumber").siblings("span").html('The drawing # field is required').show();
                            error = true;
                        } else {
                            $("#DrawingNumber").siblings("span").html('').hide();
                        }

                        if (!workOrderNumber.val()) {
                            $("#WorkOrderNumber").siblings("span").html('The work order # field is required').show();
                            error = true;
                        } else {
                            $("#WorkOrderNumber").siblings("span").html('').hide();
                        }

                        if (!shippingTagNumber.val()) {
                            $("#ShippingTagNumber").siblings("span").html('The ship tag # field is required').show();
                            error = true;
                        } else {
                            $("#ShippingTagNumber").siblings("span").html('').hide();
                        }

                        if (!description.val()) {
                            $("#Description").siblings("span").html('The description field is required').show();
                            error = true;
                        } else {
                            $("#Description").siblings("span").html('').hide();
                        }

                        if (!unitWeight.val()) {
                            $("#UnitWeight").siblings("span").html('The unit weight date field is required').show();
                            error = true;
                        } else if (isNaN(unitWeight.val())) {
                            $("#UnitWeight").siblings("span").html('The unit weight is not a valid number').show();
                            error = true;
                        } else {
                            $("#UnitWeight").siblings("span").html('').hide();
                        }

                        if (!quantity.val()) {
                            $("#Quantity").siblings("span").html('The quantity field is required').show();
                            error = true;
                        } else if (isNaN(quantity.val())) {
                            $("#Quantity").siblings("span").html('The quantity is not a valid number').show();
                            error = true;
                        } else {
                            $("#Quantity").siblings("span").html('').hide();
                        }

                        if (isNaN(readyToShip.val())) {
                            $("#ReadyToShip").siblings("span").html('The ready to ship field is not a valid number').show();
                            error = true;
                        } else {
                            $("#ReadyToShip").siblings("span").html('').hide();
                        }

                        if (!revision.val()) {
                            $("#Revision").siblings("span").html('The revision field is required').show();
                            error = true;
                        } else if (isNaN(revision.val())) {
                            $("#Revision").siblings("span").html('The revision is not a valid number').show();
                            error = true;
                        } else {
                            $("#Revision").siblings("span").html('').hide();
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (error) {
                        return false;
                    }
                }
            });

            this.editorMain.editor.on('preOpen', function (e, mode, action) {

                var editable = true;

                if (action !== "remove") {

                    var rowData = $this.editorMain.datatable.row($this.editorMain.editor.modifier().row).data();
                    var columnIndex = $this.editorMain.editor.modifier().column;

                    if ($.inArray($this.editorMain.editor.modifier().column, $this.alwaysEditableColumns) < 0) {
                        if (rowData.IsAssociatedToHardwareKit || (rowData.FullyShippedText == "YES" && rowData.Quantity != 0)) {
                            editable = false;
                        }
                        else if ($.inArray($this.editorMain.editor.modifier().column, $this.editableColumns) < 0) {
                            editable = false;
                        } else if (rowData.IsHardwareKit && columnIndex == $this.columnIndexes.EquipmentName) {
                            editable = false;
                        }
                    }
                }

                return editable;

            });

            this.editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();

                // If any of these cells can't be edited, set their values back to original
                $.each(cells, function (i, cell) {
                    var rowIndex = cell[0].index.row;
                    var columnIndex = cell[0].index.column;

                    var rowData = $this.editorMain.datatable.row(rowIndex).data();
                    
                    //skip this check if we are editing an always editable column
                    if ($.inArray(columnIndex, $this.alwaysEditableColumns) < 0) {

                        if (rowData.IsAssociatedToHardwareKit) {
                            cell[0].set = cell[0].data;
                        }
                        else if (rowData.FullyShippedText == "YES" && rowData.Quantity != 0) {
                            cell[0].set = cell[0].data;
                        }
                        else if ($.inArray(columnIndex, $this.editableColumns) < 0) {
                            cell[0].set = cell[0].data;
                        }
                        else if (rowData.IsHardwareKit && columnIndex == $this.columnIndexes.EquipmentName) {
                            cell[0].set = cell[0].data;
                        }
                    }
                });
                
            });

            this.editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
                $createRow.find("select").prop('selectedIndex', 0);
                $createRow.find("#ReadyToShip").val("0");
                $createRow.find("#Revision").val("00");
                $createRow.find("#ShippedFrom").val("WENDT");
                $createRow.find(".datePickerTable").datepicker("setDate", new Date());
            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.EquipmentId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }



                if (data.IsAssociatedToHardwareKit || (data.FullyShippedText == "YES" && data.Quantity != 0)) {
                    $(row.node()).addClass('active');
                } else {
                    $(row.node()).removeClass('active');
                }


                var $equipmentNameCell = $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.EquipmentName).node());

                if (data.IsAssociatedToHardwareKit) {
                    $equipmentNameCell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + data.AssociatedHardwareKitNumber + "</span>");
                }

                var $attachmentCell = $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.Attachments).node());
                var attachmentText = '<a data-equipmentid="' + data.EquipmentId + '" href="javascript:void(0);" class="attachments" data-toggle="modal" data-toggle="modal" data-target="#equipmentAttachmentModal">Attachments</a>';

                if (data.AttachmentCount) {
                    attachmentText = "<span class='badge'>" + data.AttachmentCount + "</span>" + attachmentText;
                }

                $($attachmentCell).html(attachmentText);


                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.EquipmentName).node()).attr("class", data.Indicators.EquipmentNameColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.PriorityNumber).node()).attr("class", data.Indicators.PriorityColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.DrawingNumber).node()).attr("class", data.Indicators.DrawingNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.WorkOrderNumber).node()).attr("class", data.Indicators.WorkOrderNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippingTagNumber).node()).attr("class", data.Indicators.ShippingTagNumberColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.UnitWeightText).node()).attr("class", "text-right " + data.Indicators.UnitWeightColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ReadyToShip).node()).attr("class", "text-right " + data.Indicators.ReadyToShipColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.ShippedQuantity).node()).attr("class", "text-right active " + data.Indicators.ShippedQtyColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.LeftToShip).node()).attr("class", "text-right active " + data.Indicators.LeftToShipColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.FullyShippedText).node()).attr("class", "active " + data.Indicators.FullyShippedColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.CustomsValueText).node()).attr("class", "text-right active " + data.Indicators.CustomsValueColor);
                $($this.editorMain.datatable.cell(row.index(), $this.columnIndexes.SalePriceText).node()).attr("class", "text-right active " + data.Indicators.SalePriceColor);

            });

            $("#deleteRecords").on("click", function () {
                var selectedRows = $this.editorMain.datatable.rows({ selected: true });
                if (selectedRows.length) {

                    var rawIndexes = selectedRows.indexes();

                    var updatedIndexes = [];
                    $.each(selectedRows.data(), function (i, row) {
                        if (!row.HasBillOfLading && !row.IsAssociatedToHardwareKit && !row.IsHardwareKit) {
                            updatedIndexes.push(rawIndexes[i]);
                        }
                    });

                    $this.editorMain.editor
                        .title('Delete records')
                        .buttons('Confirm delete')
                        .message('Are you sure you want to delete these ' + updatedIndexes.length + ' out of ' + rawIndexes.length + ' selected records? Note – Field Hardware Kits or records associated with BOLs or Hardware Kits cannot be deleted.')
                        .remove(updatedIndexes);

                } else {
                    $("#deleteRecords").button("reset");

                    main.error("You must select at least one equipment record");
                }
            });

            $('#equipmentAttachmentModal').on('show.bs.modal', function (e) {
                var equipmentId = $(e.relatedTarget).attr("data-equipmentid");

                tableEquipmentAttachment.init(equipmentId);
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
                    url: ROOT_URL + "api/EquipmentApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentName" },
                    {
                        name: "PriorityNumber",
                        type: "select",
                        options: priorities,
                        placeholderDisabled: false,
                        placeholder: ""
                    },
                    {
                        name: "ReleaseDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "DrawingNumber", type: "textarea" },
                    { name: "Revision" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" },
                    { name: "TotalWeight", type: "readonly" },
                    { name: "TotalWeightShipped", type: "readonly" },
                    { name: "ReadyToShip" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "LeftToShip", type: "readonly" },
                    { name: "FullyShippedText", type: "readonly" },
                    { name: "ShippedFrom" },
                    { name: "Notes", type: "textarea" },
                    { name: "CustomsValueText" },
                    { name: "SalePriceText" },
                    {
                        name: "HTSCode",
                        type: "autoComplete",
                        openOnFocus: true,
                        opts: {
                            source: htsCodes,
                            minLength: 0,
                            focus: function (event, ui) {
                                $(this).val(ui.item.value);
                            }
                        }
                    },
                    { name: "CountryOfOrigin" },
                    { name: "BillOfLadingNumbers" },
                    {
                        name: "LatestBOLDateShipped",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "EquipmentId", type: "readonly" },
                    { name: "IsHardwareKitText", type: "readonly" },
                    { name: "IsAssociatedToHardwareKitText", type: "readonly" },
                    { name: "HasErrorsText", type: "readonly" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            $.ajax({
                url: ROOT_URL + "api/ProjectApi/Get/" + $("#projectId").val(),
                async: false,
                success: function (result) {
                    if (result) {
                        if (!result.IsCustomsProject && !result.IncludeSoftCosts) {
                            $this.hideColumns = true;
                        }
                    }
                }
            });

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Table",
                    dataSrc: ""
                },
                rowId: 'EquipmentId',
                keys: {
                    columns: this.editableColumns
                },
                autoFill: {
                    columns: this.editableColumns
                },
                autoWidth: false,
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }

                    if (data.IsAssociatedToHardwareKit || (data.FullyShippedText == "YES" && data.Quantity != 0)) {
                        $(row).addClass('active');
                    }

                    if (data.IsHardwareKit) {
                        $($this.editorMain.datatable.cell(index, $this.columnIndexes.EquipmentName).node()).addClass("active");
                    }
                },
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                order: [[this.columnIndexes.ReleaseDate, 'desc'], [this.columnIndexes.EquipmentId, 'desc']],
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
                        data: "EquipmentName", "targets": this.columnIndexes.EquipmentName,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Indicators.EquipmentNameColor);

                            if (rowData.IsAssociatedToHardwareKit) {
                                $cell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + rowData.AssociatedHardwareKitNumber + "</span>");
                            }
                        },
                        className: "equipmentNameWidth"
                    },
                    {
                        data: "PriorityNumber", "targets": this.columnIndexes.PriorityNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.PriorityColor);
                        },
                        className: "priorityWidth"
                    },
                    {
                        data: "ReleaseDate", "targets": this.columnIndexes.ReleaseDate, type: "date",
                        className: "dateWidth"
                    },
                    {
                        data: "DrawingNumber", "targets": this.columnIndexes.DrawingNumber,
                        className: "drawingNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.DrawingNumberColor);
                        }
                    },
                    {
                        data: "Revision", "targets": this.columnIndexes.Revision, className: "active text-right"
                    },
                    {
                        data: "WorkOrderNumber", "targets": this.columnIndexes.WorkOrderNumber,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.WorkOrderNumberColor);
                        },
                        className: "workOrderNumberWidth"
                    },
                    {
                        data: "Quantity", "targets": this.columnIndexes.Quantity, className: "text-right quantityWidth"
                    },
                    {
                        data: "ShippingTagNumber", "targets": this.columnIndexes.ShippingTagNumber,
                        className: "shippingTagNumberWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippingTagNumberColor);
                        }
                    },
                    {
                        data: "Description", "targets": this.columnIndexes.Description,
                        className: "descriptionWidth"
                    },
                    {
                        data: "UnitWeightText", "targets": this.columnIndexes.UnitWeightText, className: "text-right unitWeightWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.UnitWeightColor);
                        }
                    },
                    {
                        data: "TotalWeight", "targets": this.columnIndexes.TotalWeight, className: "active text-right totalWeightWidth"
                    },
                    {
                        data: "TotalWeightShipped", "targets": this.columnIndexes.TotalWeightShipped, className: "active text-right totalWeightShippedWidth"
                    },
                    {
                        data: "ReadyToShip", "targets": this.columnIndexes.ReadyToShip, className: "text-right readyToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedQuantity", "targets": this.columnIndexes.ShippedQuantity, className: "active text-right shippedQuantityWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "LeftToShip", "targets": this.columnIndexes.LeftToShip, className: "active text-right leftToShipWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "FullyShippedText", "targets": this.columnIndexes.FullyShippedText, className: "active fullyShippedWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.FullyShippedColor);
                        }
                    },
                    {
                        data: "ShippedFrom", "targets": this.columnIndexes.ShippedFrom,
                        className: "shippedFromWidth"
                    },
                    {
                        data: "Notes", "targets": this.columnIndexes.Notes,
                        className: "notesWidth always-editable"
                    },
                    {
                        data: "CustomsValueText", "targets": this.columnIndexes.CustomsValueText, className: "active text-right customsValueWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.CustomsValueColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$'),
                        visible: !$this.hideColumns
                    },
                    {
                        data: "SalePriceText", "targets": this.columnIndexes.SalePriceText, className: "active text-right salePriceWidth",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.SalePriceColor);
                        },
                        render: $.fn.dataTable.render.number(',', '.', 2, '$'),
                        visible: !$this.hideColumns
                    },
                    {
                        data: "HTSCode", "targets": this.columnIndexes.HTSCode,
                        className: "htsCodeWidth always-editable",
                        visible: !$this.hideColumns
                    },
                    {
                        data: "CountryOfOrigin", "targets": this.columnIndexes.CountryOfOrigin,
                        className: "countryOfOriginWidth always-editable",
                        visible: !$this.hideColumns
                    },
                    {
                        data: "BillOfLadingNumbers", "targets": this.columnIndexes.BillOfLadingNumbers,
                        className: "billOfLadingNumbersWidth active"
                    },
                    {
                        data: "LatestBOLDateShipped", "targets": this.columnIndexes.LatestBOLDateShipped, type: "date",
                        className: "latestBOLDateShippedWidth active"
                    },
                    {
                        "targets": this.columnIndexes.Attachments,
                        searchable: false,
                        orderable: false,
                        className: "text-nowrap attachments",
                        render: function (datadata, type, row, meta) {
                            var text = '<a data-equipmentid="' + row.EquipmentId + '" href="javascript:void(0);" data-toggle="modal" data-toggle="modal" data-target="#equipmentAttachmentModal">Attachments</a>';

                            if (row.AttachmentCount) {
                                text = "<span class='badge'>" + row.AttachmentCount + "</span>" + text;
                            }

                            return text;
                        }
                    },
                    {
                        data: "HasErrorsText",
                        "targets": this.columnIndexes.HasErrorsText,
                        sortable: false,
                        visible: false
                    },
                    {
                        data: "IsAssociatedToHardwareKitText",
                        "targets": this.columnIndexes.IsAssociatedToHardwareKitText,
                        sortable: false,
                        visible: false
                    },
                    {
                        data: "EquipmentId",
                        "targets": this.columnIndexes.EquipmentId,
                        searchable: false,
                        visible: false
                    },
                    {
                        data: "IsDuplicateText",
                        "targets": this.columnIndexes.IsDuplicateText,
                        sortable: false,
                        visible: false
                    },
                    {
                        data: "AttachmentCount",
                        "targets": this.columnIndexes.AttachmentCount,
                        sortable: false,
                        visible: false
                    }
                ]
            });
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
            link.href = "mailto:?subject=" + $("#projectNumber").val() + "&body=%0D%0A%0D%0A%0D%0A%0D%0A";

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
                this.columnIndexes.Attachments]).visible(false);

            if (!this.hideColumns) {
                this.editorMain.datatable.columns([
                    this.columnIndexes.CustomsValueText,
                    this.columnIndexes.SalePriceText]).visible(false);
            }
        };

        this.revertTableFromSnip = function () {
            this.snipping = false;

            this.editorMain.datatable.columns([
                this.columnIndexes.Select,
                this.columnIndexes.Copy,
                this.columnIndexes.Attachments]).visible(true);

            if (!this.hideColumns) {
                this.editorMain.datatable.columns([
                    this.columnIndexes.CustomsValueText,
                    this.columnIndexes.SalePriceText]).visible(true);
            }

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

    editorMSL = new EditorMSL();

});