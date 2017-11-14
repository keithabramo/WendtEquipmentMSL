$(function () {

    var ImportWorkOrderPrice = function () {

        this.canSubmit = false;

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;


            editorMain.editor.on('preSubmit', function (e, data, action) {
                data.doSubmit = $this.canSubmit;
            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                var row = editorMain.datatable.row("#" + data.WorkOrderPriceId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

            });

            editorMain.datatable.on('autoFill', function (e, datatable, cells) {
                $this.validationErrors();
            });

            editorMain.editor.on('submitComplete', function () {
                $this.validationErrors();
            });

            $("#Import").on("click", function () {

                if (!$this.validationErrors()) {
                    $this.canSubmit = true;

                    editorMain.editor.edit(
                        editorMain.datatable.rows({ selected: true }).indexes(), false
                    ).submit(function () {
                        $this.canSubmit = false;
                        location.href = ROOT_URL + "WorkOrderPrice/?ajaxSuccess=true"
                    }, function () {
                        $this.canSubmit = false;
                        main.error("There was an error whill trying to import");
                    });
                } else {
                    main.error("Please address all rows with validation errors before importing.")
                }
            });

            editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.validationErrors();
                }
            });

            editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.clearValidation();
                }
            });
        }

        this.initEditor = function () {

            editorMain.initEditor({
                formOptions: {},
                ajax: {
                    url: ROOT_URL + "api/ImportApi/WorkOrderPriceEditor",
                    dataSrc: ""
                },
                idSrc: 'WorkOrderPriceId',
                fields: [
                    {
                        name: "WorkOrderPriceId"
                    }, {
                        name: "WorkOrderNumber"
                    }, {
                        name: "CostPrice"
                    }, {
                        name: "SalePrice"
                    }, {
                        name: "ReleasedPercent"
                    }, {
                        name: "ShippedPercent"
                    }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;
           
            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetWorkOrderPricesFromImport",
                    dataSrc: "",
                    data: {
                        filePath: filePath
                    }
                },
                order: [[2, 'desc']],
                rowId: 'WorkOrderPriceId',
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                },
                columnDefs: [
                    {
                        data: "WorkOrderPriceId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>"
                        }
                    },
                    { data: "WorkOrderNumber", targets: 1 },
                    { data: "CostPrice", targets: 2 },
                    { data: "SalePrice", targets: 3 },
                    { data: "ReleasedPercent", targets: 4 },
                    { data: "ShippedPercent", targets: 5 }
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                keys: {
                    columns: ":not(:first-child)"
                },
                autoFill: {
                    editor: null,
                    columns: ":not(:first-child)"
                }
            });
        }

        this.validationErrors = function () {
            var $this = this;

            var errors = false;

            editorMain.datatable.rows({ selected: true }).every(function (rowIdx, tableLoop, rowLoop) {
                var error = false;
                var data = this.data();

                var workOrderNumber = data.WorkOrderNumber;


                if (!workOrderNumber) {
                    error = true;
                    $this.addError(rowIdx, 1);
                } else {
                    $this.removeError(rowIdx, 1);
                }


                if (error) {
                    $(this.node()).addClass("danger");
                    errors = true;
                } else {
                    $(this.node()).removeClass("danger");
                }
            });

            return errors;
        }


        this.clearValidation = function () {
            var $this = this;

            editorMain.datatable.rows({ selected: false }).every(function (rowIdx, tableLoop, rowLoop) {

                $this.removeError(rowIdx, 1);
                $(this.node()).removeClass("danger");
            });
        }

        this.addError = function (row, column) {
            $(editorMain.datatable.cell(row, column).node()).addClass("Red");
        }

        this.removeError = function (row, column) {
            $(editorMain.datatable.cell(row, column).node()).removeClass("Red");
        }


        this.initStyles();
        this.initEvents();
    }

    importWorkOrderPrice = new ImportWorkOrderPrice();

});