$(function () {

    var Editor = function () {

        this.dataTable;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $this = this;

            var editor = new $.fn.dataTable.Editor({
                ajax: {
                    url: ROOT_URL + "api/WorkOrderPriceApi/Editor",
                    dataSrc: ""
                },
                table: '.table.my-datatable',
                idSrc: 'WorkOrderPriceId',
                fields: [
                    {
                        label: "Work Order Price Id",
                        name: "WorkOrderPriceId"
                    }, {
                        label: "Project Id",
                        name: "ProjectId"
                    }, {
                        label: "Work Order Number",
                        name: "WorkOrderNumber"
                    }, {
                        label: "Sales+Soft Costs",
                        name: "CostPrice"
                    }, {
                        label: "Sale Price",
                        name: "SalePrice"
                    }, {
                        label: "Released %",
                        name: "ReleasedPercent"
                    }, {
                        label: "Shipped %",
                        name: "ShippedPercent"
                    }
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
            
            //$('.table.my-datatable').on('click', 'tbody td', function (e) {
            //    editor.inline(this, {
            //        submit: 'allIfChanged'
            //    });

            //    $(".dt-autofill-handle").style("top", $(".dt-autofill-handle").top() + 12 + "px");
            //    $("td.focus").removeClass("focus");
            //});
            var keyCodes = [];

            for (var i = 8; i <= 222; i++) {
                if (i !== 37 && i !== 39) {
                    keyCodes.push(i);
                }
            }

            this.dataTable = $(".table.my-datatable").DataTable({
                    lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                    pageLength: 100,
                    deferRender: true,
                    ajax: {
                        url: ROOT_URL + "api/WorkOrderPriceApi/Table",
                        dataSrc: ""
                    },
                    rowId: 'WorkOrderPriceId',
                    keys: {
                        editor: editor,
                        keys: keyCodes
                        //keys: [27/*esc*/, 9/*Tab*/, /*37left*/, 38/*up*/, /*39right*/, 40/*down*/],
                    },
                    autoFill: {
                        editor: editor
                    },

                    columns: [
                        { data: "WorkOrderNumber" },
                        { data: "CostPrice" },
                        { data: "SalePrice" },
                        { data: "ReleasedPercent" },
                        { data: "ShippedPercent" }
                    ],
                    drawCallback : function () {
                        $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");
                    },
                    dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                         "<'row'<'col-sm-12'tr>>" +
                         "<'row'<'col-sm-12 text-center'p>>"

                });
           

            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;

        }

        this.initEvents = function () {
            var $this = this;

        }


        this.initStyles();
        this.initEvents();
    }

    editor = new Editor();


    































});

function DeleteSuccess() {
    $(this).closest("tr").remove();
};