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
                if (action !== 'remove') {
                    var workOrderNumber = this.field('WorkOrderNumber');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!workOrderNumber.isMultiValue()) {
                        if (!workOrderNumber.val()) {
                            workOrderNumber.error('A work order number is required');
                        }
                    }

                   
                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }

                data.doSubmit = $this.canSubmit;
            });

            $("#save").on("click", function () {

                $this.canSubmit = true;

                editorMain.editor.edit(
                    editorMain.datatable.rows({ selected: true }).indexes(), false
                ).submit(function () {
                    $this.canSubmit = false;
                    location.href = ROOT_URL + "WorkOrderPrice/?ajaxSuccess=true"
                }, function () {
                    $this.canSubmit = false;
                    $(".global-message").html('<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>ERROR! </strong>There was an error whill trying to import</div>');
                });
                
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
                        $(row).addClass('danger');
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




        this.initStyles();
        this.initEvents();
    }

    importWorkOrderPrice = new ImportWorkOrderPrice();

});