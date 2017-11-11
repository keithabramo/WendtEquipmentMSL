﻿$(function () {

    var Table = function () {

        this.datatable;
        this.editor;

        this.initStyles = function () {
            var $this = this;
        }

        this.initEvents = function () {
            var $this = this;

            $(".table tfoot, .table thead").on("click", function () {
                $this.datatable.cell.blur();
            });

            $(".table").on("click", ".delete", function () {
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

        }

        this.initEditor = function (settings) {

            var editorSettings = $.extend(true, {
                table: '.table.my-datatable'
            }, settings)

            this.editor = new $.fn.dataTable.Editor(editorSettings);
        }

        this.initDatatable = function (settings) {
            var $this = this;

            var datatableSettings = $.extend(true, {
                lengthMenu: [[100, 500, -1], [100, 500, "All"]],
                pageLength: 100,
                deferRender: true,
                fixedHeader: true,
                drawCallback: function () {
                    $(".dataTables_filter input, .dataTables_length select").addClass("form-control input-sm");

                    $this.createColumnFilters();

                    if ($(".pagination li").length === 2) {
                        $(".pagination").parent().hide();
                    } else {
                        $(".pagination").parent().show();
                    }

                },
                dom: "<'row'<'col-sm-5 text-left custom'f><'col-sm-3 text-center'i><'col-sm-4 text-right'l>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row bottom-section'<'col-sm-2 text-left createButtonContainer'><'col-sm-10 text-center'p>>"


            }, settings);

            this.datatable = $(".table.my-datatable").DataTable(datatableSettings);

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

    tableMain = new Table();

});