$(function () {

    var EditorMSL = function () {

        this.dataTable;
        this.editor;
        this.editableColumns = [0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 15, 18, 19, 20];

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var filterReadyToShip = $('#readyToShipFilter').is(":checked");

                    if (!filterReadyToShip) {
                        return true;
                    } else {
                        var leftToShip = data[13];
                        var isAssociatedToHardwareKit = data[25];

                        if (leftToShip && parseInt(leftToShip, 10) > 0 && !isAssociatedToHardwareKit) {
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
                        var equipment = data[0];

                        if (equipment.toLowerCase() !== "hardware") {
                            return true;
                        }
                    }

                    return false;
                }
            );

            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="readyToShipFilter" /> Work In Progress</label>');
            $("div.custom").append('<label class="checkbox-inline"><input type="checkbox" id="hardwareFilter" /> Hide Hardware</label>');
            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

        }

        this.initEvents = function () {
            var $this = this;

            $('#readyToShipFilter').on("change", function () {
                $this.DataTable().draw();
            });
            $('#hardwareFilter').on("change", function () {
                $this.DataTable().draw();
            });

            $(".table tfoot").on("click", function () {
                $this.dataTable.cell.blur();
            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = $this.editor.create(false);
                form.set('EquipmentName', $row.find("input[name='EquipmentName']").val());
                form.set('Priority', $row.find("select[name='Priority']").val());
                form.set('ReleaseDate', $row.find("input[name='ReleaseDate']").val());
                form.set('DrawingNumber', $row.find("input[name='DrawingNumber']").val());
                form.set('WorkOrderNumber', $row.find("input[name='WorkOrderNumber']").val());
                form.set('Quantity', $row.find("input[name='Quantity']").val());
                form.set('ShippingTagNumber', $row.find("textarea[name='ShippingTagNumber']").val());
                form.set('Description', $row.find("textarea[name='Description']").val());
                form.set('UnitWeightText', $row.find("input[name='UnitWeight']").val());
                form.set('ReadyToShipText', $row.find("input[name='ReadyToShip']").val());
                form.set('ShippedFrom', $row.find("input[name='ShippedFrom']").val());
                form.set('HTSCode', $row.find("input[name='HTSCode']").val());
                form.set('CountryOfOrigin', $row.find("input[name='CountryOfOrigin']").val());
                form.set('Notes', $row.find("textarea[name='Notes']").val());
                form.submit();

                $this.dataTable.draw();
            });

            $(".table").on("click", ".delete", function () {
                $this.editor
                    .title( 'Delete row' )
                    .buttons( 'Confirm delete' )
                    .message( 'Are you sure you want to delete this equipment record?' )
                    .remove( $(this).closest("tr") );
            })

            //show hide child row
            $('.table.my-datatable').on('click', ".expand", function () {

                var $row = $(this).closest('tr');
                var datatableRow = $this.dataTable.row($row);

                if (datatableRow.child.isShown()) {
                    // This row is already open - close it
                    datatableRow.child.hide();
                }
                else {
                    // Open this row
                    $.get({
                        url: ROOT_URL + "Equipment/BOLsAssociatedToEquipment/",
                        data: { id: datatableRow.id() },
                        success: function(response) {
                            datatableRow.child(response).show();
                        }
                    });
                }


                var $icon = $(this);

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });

            this.editor.on('preOpen', function (e, mode, action) {
                var rowData = $this.dataTable.row($this.editor.modifier().row).data();

                var editable = true;

                if ($.inArray($this.editor.modifier().column, $this.editableColumns) < 0 && action !== "remove") {
                    editable = false;
                } else if (rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit) {
                    editable = false;
                }

                return editable;

            });

            this.editor.on('postEdit', function (e, json, data) {

                    var row = $this.dataTable.row("#" + data.EquipmentId);

                    if (data.IsDuplicate) {
                        $(row.node()).attr("class", 'warning');
                    } else {
                        $(row.node()).removeClass('warning');
                    }

                    $($this.dataTable.cell(row.index(), 0).node()).attr("class", data.Indicators.EquipmentNameColor);
                    $($this.dataTable.cell(row.index(), 4).node()).attr("class", data.Indicators.WorkOrderNumberColor);
                    $($this.dataTable.cell(row.index(), 8).node()).attr("class", "text-right " + data.Indicators.UnitWeightColor);
                    $($this.dataTable.cell(row.index(), 11).node()).attr("class", "text-right " + data.Indicators.ReadyToShipColor);
                    $($this.dataTable.cell(row.index(), 12).node()).attr("class", "text-right active " + data.Indicators.ShippedQtyColor);
                    $($this.dataTable.cell(row.index(), 13).node()).attr("class", "text-right active " + data.Indicators.LeftToShipColor);
                    $($this.dataTable.cell(row.index(), 14).node()).attr("class", "active " + data.Indicators.FullyShippedColor);
                    $($this.dataTable.cell(row.index(), 16).node()).attr("class", "text-right active " + data.Indicators.CustomsValueColor);
                    $($this.dataTable.cell(row.index(), 17).node()).attr("class", "text-right active " + data.Indicators.SalePriceColor);


               
            });

            //search events
            this.dataTable.columns().every(function () {
                var column = this;

                var $input = $("thead tr").eq(0).find("th").eq(this.index()).find("input");

                //var timeout = 0;
                $input.on('keyup change input search', function () {
                    //clearTimeout(timeout);
                    var searchInput = this;

                    if ($(this).closest("thead").length > 0) {
                        $this.index = $("thead input[type='text']").index($(this));
                    } else {
                        $this.index = -1;
                    }

                    //timeout = setTimeout(function () {

                        if (column.search() !== searchInput.value) {
                            column.search(searchInput.value).draw();
                        }
                    //}, 1000);


                });
            });


        }

        this.initEditor = function () {
            this.editor = new $.fn.dataTable.Editor({
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Editor",
                    dataSrc: ""
                },
                table: '.table.my-datatable',
                idSrc: 'EquipmentId',
                fields: [
                    { name: "EquipmentName" },
                    {
                        name: "Priority",
                        type: "select",
                        options: priorities
                    },
                    { name: "ReleaseDate", type: "datetime", format: "MM/DD/YYYY" },
                    { name: "DrawingNumber" },
                    { name: "WorkOrderNumber" },
                    { name: "Quantity" },
                    { name: "ShippingTagNumber", type: "textarea" },
                    { name: "Description", type: "textarea" },
                    { name: "UnitWeightText" },
                    { name: "TotalWeight", type: "readonly" },
                    { name: "TotalWeightShipped", type: "readonly" },
                    { name: "ReadyToShipText" },
                    { name: "ShippedQuantity", type: "readonly" },
                    { name: "LeftToShip", type: "readonly" },
                    { name: "FullyShippedText", type: "readonly" },
                    { name: "ShippedFrom" },
                    { name: "CustomsValueText" },
                    { name: "SalePriceText" },
                    { name: "HTSCode" },
                    { name: "CountryOfOrigin" },
                    { name: "Notes", type: "textarea" },
                    { name: "EquipmentId", type: "readonly" }
                ],
                formOptions: {
                    inline: {
                        onReturn: "submit",
                        onBlur: "submit",
                        submit: "allIfChanged"
                    },
                    //used by autofill
                    bubble: {
                        submit: "allIfChanged"
                    }
                }
            });
        }

        this.initDatatable = function () {
            var $this = this;
            var keyCodes = [];

            for (var i = 8; i <= 222; i++) {
                if (i !== 37 && i !== 39) {
                    keyCodes.push(i);
                }
            }

            mslSettings = {
                ajax: {
                    url: ROOT_URL + "api/EquipmentApi/Table",
                    dataSrc: ""
                },
                rowId: 'EquipmentId',
                keys: {
                    editor: this.editor,
                    columns: this.editableColumns,
                    editOnFocus: true,
                    keys: keyCodes
                },
                autoFill: {
                    editor: this.editor,
                    columns: this.editableColumns
                },
                autoWidth: false,
                drawCallback: function (settings) {

                    $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");

                    $this.createColumnFilters();

                    if ($(".pagination li").length === 2) {
                        $(".pagination").parent().hide();
                    } else {
                        $(".pagination").parent().show();
                    }
                },
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                    if (data.IsAssociatedToHardwareKit || data.IsHardwareKit) {
                        $(row).addClass('active');
                    }
                },
                deferRender: true,
                fixedHeader: true,
                lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                pageLength: 100,
                order: [[2, 'desc']],
                columnDefs: [
                    {
                        data: "EquipmentName", "targets": 0,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            var $cell = $(cell);

                            $cell.addClass(rowData.Indicators.EquipmentNameColor);

                            if (rowData.IsAssociatedToHardwareKit) {
                                $cell.append("<br><span class='text-info hardwarekitLabel'>Hardware Kit: " + rowData.AssociatedHardwareKitNumber + "</span>");
                            }
                        }
                    },
                    { data: "Priority", "targets": 1 },
                    { data: "ReleaseDate", "targets": 2, },
                    { data: "DrawingNumber", "targets": 3 },
                    {
                        data: "WorkOrderNumber", "targets": 4,
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.WorkOrderNumberColor);
                        }
                    },
                    { data: "Quantity", "targets": 5, className: "text-right" },
                    { data: "ShippingTagNumber", "targets": 6 },
                    { data: "Description", "targets": 7 },
                    {
                        data: "UnitWeightText", "targets": 8, className: "text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.UnitWeightColor);
                        }
                    },
                    { data: "TotalWeight", "targets": 9, className: "active text-right" },
                    { data: "TotalWeightShipped", "targets": 10, className: "active text-right" },
                    {
                        data: "ReadyToShipText", "targets": 11, className: "text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ReadyToShipColor);
                        }
                    },
                    {
                        data: "ShippedQuantity", "targets": 12, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.ShippedQtyColor);
                        }
                    },
                    {
                        data: "LeftToShip", "targets": 13, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.LeftToShipColor);
                        }
                    },
                    {
                        data: "FullyShippedText", "targets": 14, className: "active",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.FullyShippedColor);
                        }
                     },
                    { data: "ShippedFrom", "targets": 15 },
                    {
                        data: "CustomsValueText", "targets": 16, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.CustomsValueColor);
                        }
                    },
                    {
                        data: "SalePriceText", "targets": 17, className: "active text-right",
                        createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                            $(cell).addClass(rowData.Indicators.SalePriceColor);
                        }
                    },
                    { data: "HTSCode", "targets": 18 },
                    { data: "CountryOfOrigin", "targets": 19 },
                    { data: "Notes", "targets": 20 },
                    {
                        "targets": 21, 
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    },
                    {
                        "targets": 22,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return row.HasBillOfLading ? '<span class="expand glyphicon glyphicon-plus text-primary"></span>' : '';
                        }
                    }
                ],
                dom: "<'row'<'col-sm-5 text-left custom'f><'col-sm-3 text-center'i><'col-sm-4 text-right'l>>" +
                                         "<'row'<'col-sm-12'tr>>" +
                                         "<'row bottom-section'<'col-sm-2 text-left createButtonContainer'><'col-sm-10 text-center'p>>"
            };

            this.dataTable = $(".table.my-datatable").DataTable(mslSettings);

            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;
        }

        this.createColumnFilters = function () {
            if ($(".table.my-datatable thead tr").length === 1) {
                var $searchHeader = $(".table.my-datatable thead tr").clone();

                $searchHeader.find("th").each(function () {
                    var title = $.trim($(this).text());
                    var noSearch = $(this).hasClass("noSearch");

                    if (title && !noSearch) {
                        $(this).html('<input class="form-control" type="text" />');
                    } else if (noSearch) {
                        $(this).html("");
                    }
                });

                $searchHeader.find("th").removeClass("sorting sorting_desc sorting_asc");

                $(".table.my-datatable thead").prepend($searchHeader);
            }
        };

        this.initStyles();
        this.initEvents();
    }

    editorMSL = new EditorMSL();

});