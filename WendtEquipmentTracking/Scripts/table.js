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
        this.$focusedInput;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $searchHeader = $(".table.my-datatable thead tr").clone();
            var $this = this;

            $searchHeader.find("th").each(function () {
                var title = $.trim($(this).text());

                if (title) {
                    $(this).html('<input class="form-control" type="text" placeholder="Search ' + title + '" />');
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
                    drawCallback: function () {
                        

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
                        
                    },
                    "columnDefs": [
                        {
                            "data": "HasBillOfLading", "targets": 0,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(rowData.EquipmentId);

                                    if (rowData.HasBillOfLading) {
                                        $cell.append("<span class='glyphicon glyphicon-plus text-primary'></span>");
                                    }
                                } else {
                                    $cell.html("");
                                }

                                $cell.attr("data-toggle", "collapse").attr("data-id", rowData.EquipmentId).addClass("expand");
                            }
                        },
                        {
                            "data": "IsHardware", "targets": 1,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    var readOnly = rowData.IsAssociatedToHardwareKit || rowData.HasBillOfLading || rowData.IsHardwareKit;

                                    $cell.html($cellTemplate.html());

                                    $cell.find("input").val(data);
                                    $cell.find("input[type='checkbox']").prop("checked", data);

                                    if (readOnly) {
                                        $cell.find("input[type='checkbox']").prop("disabled", readOnly);
                                    } else {
                                        $cell.find("input[type='hidden']").remove();
                                    }
                                    

                                    if (rowData.IsAssociatedToHardwareKit) {
                                        $cell.append("<br> Associated to Hardware Kit: " + rowData.AssociatedHardwareKitNumber);
                                    }

                                } else {

                                    $cell.html("<input type='checkbox'/>");
                                    $cell.find("input").prop("checked", data)
                                    $cell.prop("disabled", true);

                                    if (rowData.HasHardwareKitAssociation) {
                                        $cell.append("Associated to Hardware Kit: " + rowDate.AssociatedHarwareKitNumber);
                                    }
                                }
                            }
                        },
                        {
                            "data": "EquipmentName", "targets": 2,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data).prop("readOnly", rowData.IsHardware || rowData.IsHardwareKit);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("long");
                            }
                        },
                        {
                            "data": "Priority", "targets": 3,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("select").val(data);
                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "ReleaseDate", "targets": 4,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");
                                var value = data;
                                if (data) {
                                    value = $.datepicker.formatDate('mm/dd/yy', new Date(data))
                                }

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(value).attr("id", "releaseDate" + rowData.EquipmentId);

                                } else {
                                    $cell.html(value);
                                }

                                $cell.addClass("medium");
                            }
                        },
                        {
                            "data": "DrawingNumber", "targets": 5,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("long");
                            }
                        },
                        {
                            "data": "WorkOrderNumber", "targets": 6,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("medium");
                            }
                        },
                        {
                            "data": "Quantity", "targets": 7,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data).prop("readOnly", rowData.IsHardwareKit);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "ShippingTagNumber", "targets": 8,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("long");
                            }
                        },
                        {
                            "data": "Description", "targets": 9,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("long");
                            }
                        },
                        {
                            "data": "UnitWeight", "targets": 10,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");
                                var value = (data ? data.toFixed(2) : (0).toFixed(2));

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(value);

                                } else {
                                    $cell.html(value);
                                }

                                $cell.addClass(rowData.Indicators.UnitWeightColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "TotalWeight", "targets": 11,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");
                                var value = (data ? data.toFixed(2) : (0).toFixed(2));

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(value);
                                    $cell.append(value);

                                } else {
                                    $cell.html(value);
                                }

                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "TotalWeightShipped", "targets": 12,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");
                                var value = (data ? data.toFixed(2) : (0).toFixed(2));

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(value);

                                } else {
                                    $cell.html(value);
                                }

                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "ReadyToShip", "targets": 13,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.ReadyToShipColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "ShippedQuantity", "targets": 14,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.ShippedQtyColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "LeftToShip", "targets": 15,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.LeftToShipColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "FullyShipped", "targets": 16,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                var value = "N\A";
                                if (data === true) {
                                    value = "Yes";
                                } else if (data === false) {
                                    value = "No";
                                }

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(value);
                                } else {
                                    $cell.html(value);
                                }

                                $cell.addClass(rowData.Indicators.FullyShippedColor);
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "CustomsValue", "targets": 17,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                    var value = "$" + (data ? data.toFixed(2) : 0);
                                    $cell.append(value);

                                } else {
                                    var value = "$" + (data ? data.toFixed(2) : 0);
                                    $cell.html(value);
                                }

                                $cell.addClass(rowData.Indicators.CustomsValueColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "SalePrice", "targets": 18,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                    var value = "$" + (data ? data.toFixed(2) : 0);
                                    $cell.append(value);

                                } else {
                                    var value = "$" + (data ? data.toFixed(2) : 0);
                                    $cell.html(value);
                                }

                                $cell.addClass(rowData.Indicators.SalePriceColor);
                                $cell.addClass("text-right");
                                $cell.addClass("small");
                            }
                        },
                        {
                            "data": "Notes", "targets": 19,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "SalesOrderNumber", "targets": 20,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.SalesOrderNumberColor);
                            }
                        },
                        {
                            "data": "AutoShipFile", "targets": 21,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass("small");
                            }
                        },
                        { "data": "EquipmentId", "targets": 22,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $template = $(".template");

                                if ($template.length) {
                                    var $cellTemplate = $template.find("div").eq(colIndex).clone();

                                    $cell.html($cellTemplate.html());
                                    $cell.find("a").attr("href", $cell.find("a").attr("href") + "/" + rowData.EquipmentId);

                                } else {
                                    $cell.html("");
                                }
                            }
                        },
                        { "data": "IsHardwareKit", "targets": 23, visible: false },
                        { "data": "IsAssociatedToHardwareKit", "targets": 24, visible: false },
                        { "data": "AssociatedHardwareKitNumber", "targets": 25, visible: false },
                        { "data": "Indicators", "targets": 26, visible: false }
                    ],
                    autoFill: {
                        update: false,
                        columns: [2, 3, 4, 5, 6, 7, 8, 9, 10, 13, 19, 20, 21]
                    }
                };
            }

            this.dataTable = $(".table.my-datatable").DataTable($.extend({
                pageLength: 25,
                deferRender: true,
                fixedHeader: true,
                drawCallback: function (settings) {
                    if ($this.$focuedInput) {
                        //$this.$focusedInput[0].focus();
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
                dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row'<'col-sm-12 text-center'p>>"
            }, mslSettings));

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