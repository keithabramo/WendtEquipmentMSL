waitingDialog.show();

jQuery.fn.putCursorAtEnd = function () {

    return this.each(function () {

        // Cache references
        var $el = $(this),
            el = this;

        // Only focus if input isn't already
        if (!$el.is(":focus")) {
            $el.focus();
        }

        // If this function exists... (IE 9+)
        if (el.setSelectionRange) {

            // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
            var len = $el.val().length * 2;

            // Timeout seems to be required for Blink
            setTimeout(function () {
                el.setSelectionRange(len, len);
            }, 1);

        } else {

            // As a fallback, replace the contents with itself
            // Doesn't work in Chrome, but Chrome supports setSelectionRange
            $el.val($el.val());

        }

        // Scroll to the bottom, in case we're in a tall textarea
        // (Necessary for Firefox and Chrome)
        this.scrollTop = 999999;

    });

};


$(function () {

    var Table = function () {

        this.dataTable;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $searchHeader = $(".table.my-datatable thead tr").clone();
            var $this = this;

            $searchHeader.find("th").each(function () {
                var title = $.trim($(this).text());
                var noSearch = $(this).hasClass("noSearch");

                if (title && !noSearch) {
                    $(this).html('<input class="form-control" type="text" placeholder="Search ' + title + '" />');
                } else if (noSearch) {
                    $(this).html("");
                }
            });

            $(".table.my-datatable thead").prepend($searchHeader);

            var mslSettings = {};
            var isMSL = $(".masterShipList").length;
            if (isMSL) {
                mslSettings = {
                    ajax: {
                        url: ROOT_URL + "api/EquipmentApi/Table",
                        dataSrc: ""
                    },
                    drawCallback: function (settings) {
                        if ($this.DataTable()) {
                            $this.DataTable().fixedHeader.enable();
                        }

                        if ($this.index >= 0) {
                            var $newInput = $("thead input[type='text']").eq($this.index);
                            if (!$newInput.length) {
                                $this.index -= $("thead input[type='text']").length;
                                $newInput = $("thead input[type='text']").eq($this.index);
                            }

                            $newInput.putCursorAtEnd();
                            $this.index = -1;
                        }


                        if ($(".pagination li").length === 2) {
                            $(".pagination").parent().hide();
                        } else {
                            $(".pagination").parent().show();
                        }

                        if (form) {
                            form.initStyles();
                        }
                    },
                    createdRow: function ( row, data, index ) {
                        if (data.IsDuplicate) {
                            $(row).addClass('warning');
                        }
                        if (data.FullyShipped === true) {
                            $(row).find("input").attr("readOnly", "readOnly");
                        }
                    },
                    deferRender: true,
                    fixedHeader: true,
                    lengthMenu: [ [100, 500, -1], [100, 500, "All"] ],
                    pageLength: 100,
                    order: [[2, 'desc']],
                    autoWidth: false,
                    columnDefs: [
                        //{width: "10px", targets: [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22]},
                        {
                            "data": "EquipmentName", "targets": 0,
                            className: "equipmentNameWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.EquipmentNameRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Priority", "targets": 1,
                            className: "priorityWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.PriorityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ReleaseDate", "targets": 2,
                            className: "releaseDateWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ReleaseDateRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "DrawingNumber", "targets": 3,
                            className: "drawingNumberWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DrawingNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "WorkOrderNumber", "targets": 4,
                            className: "workOrderNumberWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.WorkOrderNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Quantity", "targets": 5,
                            className: "quantityWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.QuantityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ShippingTagNumber", "targets": 6,
                            className: "shippingTagNumberWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ShippingTagNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Description", "targets": 7,
                            className: "descriptionWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DescriptionRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "UnitWeight", "targets": 8,
                            className: "unitWeightWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.UnitWeightRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "TotalWeight", "targets": 9,
                            className: "totalWeightWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.TotalWeightRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "TotalWeightShipped", "targets": 10,
                            className: "totalWeightShippedWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.TotalWeightShippedRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ReadyToShip", "targets": 11,
                            className: "readyToShipWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ReadyToShipRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ShippedQuantity", "targets": 12,
                            className: "shippedQuantityWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ShippedQuantityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "LeftToShip", "targets": 13,
                            className: "leftToShipWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.LeftToShipRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "FullyShipped", "targets": 14,
                            className: "fullyShippedWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.FullyShippedRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ShippedFrom", "targets": 15,
                            className: "shippedFromWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ShippedFromRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "CustomsValue", "targets": 16,
                            className: "customsValueWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.CustomsValueRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "SalePrice", "targets": 17,
                            className: "salePriceWidth text-right",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.SalePriceRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "HTSCode", "targets": 18,
                            className: "htsCodeWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.HTSCodeRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "CountryOfOrigin", "targets": 19,
                            className: "countryOfOriginWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.CountryOfOriginRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Notes", "targets": 20,
                            className: "notesWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.NotesRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "EquipmentId", "targets": 21, searchable: false, sortable: false,
                            className: "deleteWidth",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DeleteRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "HasBillOfLading", "targets": 22, searchable: false,
                            className: "hasBillOfLadingWidth expand",
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.HasBillOfLadingRender($(cell), rowData);
                            }
                        },
                        { "data": "IsHardwareKit", "targets": 23, visible: false, searchable: false },
                        { "data": "IsAssociatedToHardwareKit", "targets": 24, visible: false, searchable: false },
                        { "data": "AssociatedHardwareKitNumber", "targets": 25, visible: false, searchable: false },
                        { "data": "Indicators", "targets": 26, visible: false, searchable: false },
                        { "data": "IsDuplicate", "targets": 27, visible: false, searchable: false }
                    ],
                    autoFill: {
                        update: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 15, 18, 19, 20]
                    },
                    dom: "<'row'<'col-sm-5 text-left custom'f><'col-sm-3 text-center'i><'col-sm-4 text-right'l>>" +
                                             "<'row'<'col-sm-12'tr>>" +
                                             "<'row'<'col-sm-2 text-left createButtonContainer'><'col-sm-10 text-center'p>>"
                };

                this.dataTable = $(".table.my-datatable").DataTable(mslSettings);
            } else {
                this.dataTable = $(".table.my-datatable").DataTable({
                    lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                    pageLength: 100,
                    deferRender: true,
                    fixedHeader: true,
                    drawCallback: function (settings) {
                        if ($this.DataTable()) {
                            $this.DataTable().fixedHeader.enable();
                        }

                        if ($this.index >= 0) {
                            var $newInput = $("thead input[type='text']").eq($this.index);
                            if (!$newInput.length) {
                                $this.index -= $("thead input[type='text']").length;
                                $newInput = $("thead input[type='text']").eq($this.index);
                            }

                            $newInput.putCursorAtEnd();
                            $this.index = -1;
                        }


                        if ($(".pagination li").length === 2) {
                            $(".pagination").parent().hide();
                        } else {
                            $(".pagination").parent().show();
                        }

                        if (!form === undefined) {
                            form.initStyles();
                        }
                    },
                    dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                         "<'row'<'col-sm-12'tr>>" +
                         "<'row'<'col-sm-12 text-center'p>>"

                });
            }

            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;

            waitingDialog.hide();
        }

        this.initEvents = function () {
            var $this = this;



            $('.table.my-datatable').on('click', ".expand", function () {
                var id = $(this).attr("data-id");

                var $row = $(this).closest('tr');
                var datatableRow = $this.dataTable.row($row);

                if (datatableRow.child.isShown()) {
                    // This row is already open - close it
                    datatableRow.child.hide();
                }
                else {
                    // Open this row
                    datatableRow.child($("#" + id).html()).show();
                }


                var $icon = $(this).find(".glyphicon");

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });

            $('.table.my-datatable').on('focus', "td", function (e) {
                $(this).find("input").focus();
            });

            $('.table.my-datatable').on('keydown', "input", function (e) {
                var currCell = $(this);
                var c = "";

                if (e.which == 38) {
                    // Up Arrow
                    c = currCell.closest('tr').prev().find('td:eq(' +
                        currCell.closest("td").index() + ')').find("input");
                } else if (e.which == 40) {
                    // Down Arrow
                    c = currCell.closest('tr').next().find('td:eq(' +
                        currCell.closest("td").index() + ')').find("input");
                }

                // If we didn't hit a boundary, update the current cell
                if (c.length > 0) {
                    currCell = c;
                    currCell.focus();
                }
            });

            // Apply the search
            this.dataTable.columns().every(function () {
                var column = this;

                var $input = $("thead tr").eq(0).find("th").eq(this.index()).find("input");

                var timeout = 0;
                $input.on('keyup change input search', function () {
                    clearTimeout(timeout);
                    var searchInput = this;
                    $this.index = $("thead input[type='text']").index($(this));

                    timeout = setTimeout(function () {

                        if (column.search() !== searchInput.value) {
                            $this.DataTable().fixedHeader.disable();
                            column.search(searchInput.value).draw();
                        }
                    }, 500);

                    
                });
            });
        }


        this.initStyles();
        this.initEvents();
    }

    table = new Table();


    































});

function DeleteSuccess() {
    $(this).closest("tr").remove();
};