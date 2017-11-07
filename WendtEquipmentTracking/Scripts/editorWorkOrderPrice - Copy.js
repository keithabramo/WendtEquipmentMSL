$(function () {

    var EditorWorkOrderPrice = function () {

        this.dataTable;
        this.editor;

        this.DataTable = function () {
            return this.dataTable;
        };

        this.initStyles = function () {
            var $this = this;


            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;

            $(".table").on("click", ".delete", function () {
                $this.editor
                    .title('Delete row')
                    .buttons('Confirm delete')
                    .message('Are you sure you want to delete this equipment record?')
                    .remove($(this).closest("tr"));
            });

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
        }

        this.initDatatable = function () {
            var $this = this;
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
                fixedHeader: true,
                ajax: {
                    url: ROOT_URL + "api/WorkOrderPriceApi/Table",
                    dataSrc: ""
                },
                rowId: 'WorkOrderPriceId',
                keys: {
                    editor: editor,
                    keys: keyCodes,
                    editOnFocus: true
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
                drawCallback: function () {
                    $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");

                    $this.createColumnFilters();

                    if ($(".pagination li").length === 2) {
                        $(".pagination").parent().hide();
                    } else {
                        $(".pagination").parent().show();
                    }

                },
                dom: "<'row'<'col-sm-4 text-left custom'f><'col-sm-4 text-center'i><'col-sm-4 text-right'l>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row bottom-section'<'col-sm-12 text-center'p>>"

            });


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

    editorWorkOrderPrice = new EditorWorkOrderPrice();

});