﻿waitingDialog.show();

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
                    drawCallback: function (settings) {
                        if (form) {
                            form.initStyles();
                        }

                        
                        $(".dataTables_scrollBody [id='releaseDateCREATE']").remove();

                        if ($(".datePicker").length > 2) {

                            setTimeout(function () {
                                if ($("[id='releaseDate0']").length && !$("[id='releaseDateCREATE']").length) {
                                    var element = $(".dataTables_scrollFoot .datePicker");

                                    var clone = element.clone();
                                    clone.attr("id", "releaseDateCREATE").datepicker();

                                    $(".dataTables_scrollFoot .releaseDateColumn").append(clone);

                                    $("[id='releaseDate0']").remove();
                                }
                            }, 10000);
                        }
                    },
                    deferRender: true,
                    scrollY: 400,
                    scrollX: "100%",
                    scrollCollapse: true,
                    //scroller: {
                    //    displayBuffer: 500
                    //},
                    pageLength: 25,
                    "columnDefs": [
                        {
                            "data": "EquipmentName", "targets": 0,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.EquipmentNameRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Priority", "targets": 1,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.PriorityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ReleaseDate", "targets": 2,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ReleaseDateRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "DrawingNumber", "targets": 3,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DrawingNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "WorkOrderNumber", "targets": 4,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.WorkOrderNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Quantity", "targets": 5,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.QuantityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ShippingTagNumber", "targets": 6,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ShippingTagNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Description", "targets": 7,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DescriptionRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "UnitWeight", "targets": 8,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.UnitWeightRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "TotalWeight", "targets": 9,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.TotalWeightRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "TotalWeightShipped", "targets": 10,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.TotalWeightShippedRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ReadyToShip", "targets": 11,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ReadyToShipRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "ShippedQuantity", "targets": 12,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.ShippedQuantityRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "LeftToShip", "targets": 13,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.LeftToShipRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "FullyShipped", "targets": 14,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.FullyShippedRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "CustomsValue", "targets": 15,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.CustomsValueRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "SalePrice", "targets": 16,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.SalePriceRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "Notes", "targets": 17,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.NotesRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "SalesOrderNumber", "targets": 18,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.SalesOrderNumberRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "AutoShipFile", "targets": 19,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.AutoShipFileRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "EquipmentId", "targets": 20, searchable: false, sortable: false,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.DeleteRender($(cell), rowData);
                            }
                        },
                        {
                            "data": "HasBillOfLading", "targets": 21, searchable: false,
                            createdCell: function (cell, data, rowData, rowIndex, colIndex) {
                                mslRender.HasBillOfLadingRender($(cell), rowData);
                            }
                        },
                        { "data": "IsHardwareKit", "targets": 22, visible: false, searchable: false },
                        { "data": "IsAssociatedToHardwareKit", "targets": 23, visible: false, searchable: false },
                        { "data": "AssociatedHardwareKitNumber", "targets": 24, visible: false, searchable: false },
                        { "data": "Indicators", "targets": 25, visible: false, searchable: false }
                    ],
                    autoFill: {
                        update: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 17, 18, 19]
                    },
                    dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                                             "<'row'<'col-sm-12'tr>>" +
                                             "<'row'<'col-sm-12 text-center'p>>"
                    //dom: "<'row'<'col-sm-6 text-left custom'f><'col-sm-6 text-center'i>>" + 
                    //     "<'row'<'col-sm-12'tr>>"
                };

                this.dataTable = $(".table.my-datatable").DataTable(mslSettings);
            } else {
                this.dataTable = $(".table.my-datatable").DataTable({
                    pageLength: 25,
                    drawCallback: function (settings) {
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


            // Apply the search
            this.dataTable.columns().every(function () {
                var column = this;

                var $input = $("thead tr").eq(0).find("th").eq(this.index()).find("input");

                var timeout = 0;
                $input.on('keyup change input search', function () {
                    clearTimeout(timeout);
                    var searchInput = this;

                    timeout = setTimeout(function () {

                        if (column.search() !== searchInput.value) {
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