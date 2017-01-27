waitingDialog.show();

$(function () {

    var Table = function () {

        this.dataTable;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $searchHeader = $(".table.my-datatable thead tr").clone();

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

                    "columnDefs": [
                        {
                            "data": "EquipmentId", "targets": 0,
                            //"render": function ( data, type, full, meta ) {
                            //    return type === "sort" ? full.HasBillOfLading : data;
                            //},
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();
                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

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
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {
                                    var readOnly = rowData.HasHardwareKitAssociation || rowData.HasBillOfLading || rowData.IsHardwareKit;

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").prop("checked", data)

                                    if (readOnly) {
                                        $cell.find("input").prop("disabled", readOnly);
                                    } else {
                                        $cell.find("input[type='hidden']").remove();
                                    }
                                    

                                    if (rowData.HasHardwareKitAssociation) {
                                        $cell.append("Associated to Hardware Kit: " + rowDate.AssociatedHarwareKitNumber);
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
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data).prop("readOnly", rowData.IsHardware || rowData.IsHardwareKit);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "Priority", "targets": 3,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "ReleaseDate", "targets": 4,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data).attr("id", "releaseDate" + rowData.EquipmentId);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "DrawingNumber", "targets": 5,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "WorkOrderNumber", "targets": 6,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "Quantity", "targets": 7,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data).prop("readOnly", rowData.IsHardwareKit);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "ShippingTagNumber", "targets": 8,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "Description", "targets": 9,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "UnitWeight", "targets": 10,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.UnitWeightColor);
                            }
                        },
                        {
                            "data": "TotalWeight", "targets": 11,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "TotalWeightShipped", "targets": 12,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "ReadyToShip", "targets": 13,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.ReadyToShipColor);
                            }
                        },
                        {
                            "data": "ShippedQuantity", "targets": 14,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.ShippedQtyColor);
                            }
                        },
                        {
                            "data": "LeftToShip", "targets": 15,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);
                                    $cell.append(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.LeftToShipColor);
                            }
                        },
                        {
                            "data": "FullyShipped", "targets": 16,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                    var value = "N\A";
                                    if(data === true) {
                                        value = "Yes";
                                    } else if (data === false) {
                                        value = "No";
                                    }

                                    $cell.append(value);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.FullyShippedColor);
                            }
                        },
                        {
                            "data": "CustomsValue", "targets": 17,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.CustomsValueColor);
                            }
                        },
                        {
                            "data": "SalePrice", "targets": 18,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.SalePriceColor);
                            }
                        },
                        {
                            "data": "Notes", "targets": 19,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "AutoShipFile", "targets": 20,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }
                            }
                        },
                        {
                            "data": "SalesOrderNumber", "targets": 21,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

                                    $cell.html($cellTemplate.html());
                                    $cell.find("input").val(data);

                                } else {
                                    $cell.html(data);
                                }

                                $cell.addClass(rowData.Indicators.SalesOrderNumberColor);
                            }
                        },
                        { "data": "HasBillOfLading", "targets": 22,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {

                                var $cell = $(cell);
                                var $cellTemplate = $(".template div").eq(colIndex).clone();

                                if ($cellTemplate.length) {

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
                drawCallback: function( settings ) {
                    $(":focus").blur().focus();
                },
                dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row'<'col-sm-12 text-center'p>>"
            }, mslSettings));

            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;

            if ($(".pagination li").length === 2) {
                $(".pagination").parent().hide();
            }

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

                $input.on('keyup change', function () {
                    if (column.search() !== this.value) {
                        column.search(this.value).draw();
                    }
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