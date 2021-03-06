﻿$(function () {

    var TableBOLDetails = function () {

        this.tableMain = new Table();

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;

            //show hide child row
            $('.table.my-datatable').on('click', ".expand", function () {

                var $row = $(this).closest('tr');
                var datatableRow = $this.tableMain.datatable.row($row);

                if (datatableRow.child.isShown()) {
                    // This row is already open - close it
                    datatableRow.child.hide();
                }
                else {
                    // Open this row
                    $.get({
                        url: ROOT_URL + "BillOfLading/EquipmentAssociatedToBOL/",
                        data: { id: datatableRow.id() },
                        success: function (response) {
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
        };

        this.initEditor = function () {

            this.tableMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'BillOfLadingId'
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingApi/DetailsTable",
                    dataSrc: "",
                    data: {
                        billOfLadingNumber: billOfLadingNumber
                    }
                },
                rowId: 'BillOfLadingId',
                order: [[1, 'desc']],
                columns: [
                    { data: "BillOfLadingId" },
                    { data: "Revision" },
                    { data: "DateShipped" },
                    { data: "Carrier" },
                    { data: "TrailerNumber" },
                    { data: "ShippedFrom" },
                    { data: "ShippedTo" },
                    { data: "FreightTerms" }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        searchable: false,
                        sortable: false,
                        render: function (datadata, type, row, meta) {
                            return '<span class="expand glyphicon glyphicon-plus text-primary"></span>';
                        }
                    },
                    {
                        "targets": 1,
                        render: function (data, type, row, meta) {
                            return row.IsCurrentRevision ? "Current" : row.Revision;
                        }
                    },
                    {
                        "targets": 2, type: "date",
                        render: function (data, type, row, meta) {
                            return row.DateShipped ? moment(row.DateShipped).format("M/D/YY") : '';
                        }
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableBOLDetails = new TableBOLDetails();

});