$(function () {

    Editor = function (selector) {

        this.selector = selector || '.table.my-datatable';
        this.datatable;
        this.editor;

        this.initStyles = function () {
            var $this = this;
        };

        this.initEvents = function () {
            var $this = this;

            $(this.selector).on("click", "thead th.select-checkbox", function () {

                if ($(this).closest("tr").hasClass("selected")) {
                    $(this).closest("tr").removeClass("selected");
                    $this.datatable.rows({ page: 'current' }).deselect();
                } else {
                    $(this).closest("tr").addClass("selected");
                    $this.datatable.rows({ page: 'current' }).select();
                }
            });

            $(this.selector).on("click", "tfoot, thead", function () {
                $this.datatable.cell.blur();
            });

            $(this.selector).on("click", ".delete", function () {
                $this.editor
                    .title('Delete row')
                    .buttons('Confirm delete')
                    .message('Are you sure you want to delete this record?')
                    .remove($(this).closest("tr"));
            });

            this.datatable.columns().every(function () {
                var column = this;

                var $input = $($this.selector).find("thead tr").eq(0).find("th").eq(this.index()).find("input");

                $input.on('keyup change input search', function () {
                    var searchInput = this;

                    if ($(this).closest("thead").length > 0) {
                        $this.index = $($this.selector).find("thead input[type='text']").index($(this));
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

            $(".fontSize").on("fontadjusted", function () {
                $this.datatable.columns.adjust();
            });

            this.editor.on('open', function (e, mode, action) {
                $($this.datatable.cell(".focus").node()).addClass("editing");
            });

            this.editor.on('preClose', function (e, mode, action) {
                $($this.datatable.cell(".editing").node()).removeClass("editing");
            });

            $(this.selector).on("keydown", "tbody .focus .DTE_Field_InputControl", function (e) {
                switch (e.which) {

                    case 38: // up
                        $this.datatable.keys.move("up");
                        break;

                    case 40: // down
                        $this.datatable.keys.move("down");

                        break;
                    default: return;
                }

                e.preventDefault();
            });
        };

        this.initEditor = function (settings) {

            var editorSettings = $.extend(true, {
                table: this.selector,
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
            }, settings);

            this.editor = new $.fn.dataTable.Editor(editorSettings);
        };

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

                    $this.createColumnFilters.apply($this);

                    if ($(".paginate_button").length === 2) {
                        $(".dataTables_paginate").hide();
                    } else {
                        $(".dataTables_paginate").show();
                    }

                },
                dom: "<'row'<'col-sm-6 text-left custom'f><'col-sm-4'i><'col-sm-2 text-right'l>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row bottom-section'<'col-sm-2 text-left createButtonContainer'><'col-sm-5 text-right'i><'col-sm-5 text-right'p>>"


            }, settings);

            this.datatable = $(this.selector).DataTable(datatableSettings);


            delete $.fn.dataTable.AutoFill.actions.fillHorizontal;
            delete $.fn.dataTable.AutoFill.actions.increment;
            //delete $.fn.dataTable.AutoFill.actions.fill;
            //delete $.fn.dataTable.AutoFill.actions.cancel;

            this.initStyles();
            this.initEvents();
        };

        this.createColumnFilters = function () {
            if ($(this.selector).find("thead tr").length === 1) {
                var $searchHeader = $(this.selector).find("thead tr").clone();

                $searchHeader.find("th").each(function () {
                    var title = $.trim($(this).text());
                    var noSearch = $(this).hasClass("noSearch");

                    if (title && !noSearch) {
                        $(this).html('<input class="form-control" type="text" />');
                    } else if (noSearch) {
                        $(this).html("");
                    }
                });

                $searchHeader.find("th").attr("tabindex", "-1").removeClass("sorting sorting_desc sorting_asc select-checkbox dateWidth");

                $(this.selector).find("thead").prepend($searchHeader);
            }
        };
    };
});