$(function () {

    var Editor = function () {

        this.datatable;
        this.editor;

        this.initStyles = function () {
            var $this = this;
        }

        this.initEvents = function () {
            var $this = this;

            $(".table thead th.select-checkbox").on("click", function () {

                if ($(this).closest("tr").hasClass("selected")) {
                    $(this).closest("tr").removeClass("selected")
                    $this.datatable.rows({ page: 'current' }).deselect();
                } else {
                    $(this).closest("tr").addClass("selected")
                    $this.datatable.rows({ page: 'current' }).select();
                }
            });

            $(".table tfoot, .table thead").on("click", function () {
                $this.datatable.cell.blur();
            });

            $(document).on("click", ".table .delete", function () {
                $this.editor
                    .title('Delete row')
                    .buttons('Confirm delete')
                    .message('Are you sure you want to delete this record?')
                    .remove($(this).closest("tr"));
            });

            this.datatable.columns().every(function () {
                var column = this;

                var $input = $("thead tr").eq(0).find("th").eq(this.index()).find("input");

                $input.on('keyup change input search', function () {
                    var searchInput = this;

                    if ($(this).closest("thead").length > 0) {
                        $this.index = $("thead input[type='text']").index($(this));
                    } else {
                        $this.index = -1;
                    }

                    if (column.search() !== searchInput.value) {
                        column.search(searchInput.value).draw();
                    }
                });
            });

            this.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();
            });

        }

        this.initEditor = function (settings) {

            var editorSettings = $.extend(true, {
                table: '.table.my-datatable',
                formOptions: {
                    inline: {
                        onReturn: "submit",
                        onBlur: "submit",
                        submit: "allIfChanged",
                        drawType: 'page'
                    },
                    //used by autofill
                    bubble: {
                        submit: "allIfChanged",
                        drawType: 'page'
                    }
                }
            }, settings)

            this.editor = new $.fn.dataTable.Editor(editorSettings);
        }

        this.initDatatable = function (settings) {
            var $this = this;
            var keyCodes = [];

            for (var i = 8; i <= 222; i++) {
                if (i !== 37 && i !== 39) {
                    keyCodes.push(i);
                }
            }

            var datatableSettings = $.extend(true, {
                lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                pageLength: 100,
                deferRender: true,
                fixedHeader: true,
                keys: {
                    editor: this.editor,
                    keys: keyCodes,
                    editOnFocus: true
                },
                autoFill: {
                    editor: this.editor
                },
                drawCallback: function () {
                    $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");

                    $this.createColumnFilters();

                    if ($(".paginate_button").length === 2) {
                        $(".dataTables_paginate").hide();
                    } else {
                        $(".dataTables_paginate").show();
                    }

                },
                dom: "<'row'<'col-sm-5 text-left custom'f><'col-sm-5'i><'col-sm-2 text-right'l>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row bottom-section'<'col-sm-2 text-left createButtonContainer'><'col-sm-5 text-right'i><'col-sm-5 text-right'p>>"


            }, settings);

            this.datatable = $(".table.my-datatable").DataTable(datatableSettings);


            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;

            this.initStyles();
            this.initEvents();
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

                $searchHeader.find("th").attr("tabindex", "-1").removeClass("sorting sorting_desc sorting_asc select-checkbox");

                $(".table.my-datatable thead").prepend($searchHeader);
            }
        };

        
    }

    editorMain = new Editor();

});