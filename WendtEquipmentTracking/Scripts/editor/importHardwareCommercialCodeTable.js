$(function () {

    ImportHardwareCommercialCodeTable = function () {

        this.canSubmit = false;
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;


            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                data.doSubmit = $this.canSubmit;
            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.HardwareCommercialCodeId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

            });

            this.editorMain.datatable.on('autoFill', function (e, datatable, cells) {
                $this.validationErrors();
            });

            this.editorMain.editor.on('submitComplete', function () {
                $this.validationErrors();
            });

            $("#import").on("click", function () {
                var selectedRows = $this.editorMain.datatable.rows({ selected: true }).indexes();
                if (selectedRows.length) {
                    if (!$this.validationErrors()) {
                        $this.canSubmit = true;

                        $this.editorMain.editor.edit(
                            $this.editorMain.datatable.rows({ selected: true }).indexes(), false
                        ).submit(function () {
                            $this.canSubmit = false;
                            location.href = ROOT_URL + "HardwareCommercialCode/?ajaxSuccess=true";
                        }, function () {
                            $this.canSubmit = false;
                            $("#import").button("reset");

                            main.error("There was an error whill trying to import");
                        });
                    } else {
                        $("#import").button("reset");

                        main.error("Please address all rows with validation errors before importing.");
                    }
                } else {
                    $("#import").button("reset");

                    main.error("You must select at least one record");
                }
            });

            this.editorMain.datatable.on('select', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.validationErrors();
                }
            });

            this.editorMain.datatable.on('deselect', function (e, dt, type, indexes) {
                if (type === 'row') {
                    $this.clearValidation();
                }
            });
        };

        this.initEditor = function () {

            this.editorMain.initEditor({
                formOptions: {},
                ajax: {
                    url: ROOT_URL + "api/ImportApi/HardwareCommercialCodeEditor",
                    dataSrc: ""
                },
                idSrc: 'HardwareCommercialCodeId',
                fields: [
                    {
                        name: "HardwareCommercialCodeId"
                    }, {
                        name: "PartNumber"
                    }, {
                        name: "Description"
                    }, {
                        name: "CommodityCode"
                    }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetHardwareCommercialCodesFromImport",
                    dataSrc: "",
                    data: function () {
                        return {
                            FilePath: $("#FilePath").val()
                        };
                    }
                },
                rowId: 'HardwareCommercialCodeId',
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                },
                columnDefs: [
                    {
                        data: "HardwareCommercialCodeId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>";
                        }
                    },
                    { data: "PartNumber", targets: 1 },
                    { data: "Description", targets: 2  },
                    { data: "CommodityCode", targets: 3  }
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
        };

        this.validationErrors = function () {
            var $this = this;

            var errors = false;

            this.editorMain.datatable.rows({ selected: true }).every(function (rowIdx, tableLoop, rowLoop) {
                var error = false;
                var data = this.data();

                var partNumber = data.PartNumber;
                var description = data.Description;
                var commodityCode = data.CommodityCode;


                if (!partNumber) {
                    error = true;
                    $this.addError(rowIdx, 1);
                } else {
                    $this.removeError(rowIdx, 1);
                }

                if (!description) {
                    error = true;
                    $this.addError(rowIdx, 2);
                } else {
                    $this.removeError(rowIdx, 2);
                }

                if (!commodityCode) {
                    error = true;
                    $this.addError(rowIdx, 3);
                } else {
                    $this.removeError(rowIdx, 3);
                }


                if (error) {
                    $(this.node()).addClass("danger");
                    errors = true;
                } else {
                    $(this.node()).removeClass("danger");
                }
            });

            return errors;
        };


        this.clearValidation = function () {
            var $this = this;

            this.editorMain.datatable.rows({ selected: false }).every(function (rowIdx, tableLoop, rowLoop) {

                $this.removeError(rowIdx, 1);
                $(this.node()).removeClass("danger");
            });
        };

        this.addError = function (row, column) {
            $(this.editorMain.datatable.cell(row, column).node()).addClass("Red");
        };

        this.removeError = function (row, column) {
            $(this.editorMain.datatable.cell(row, column).node()).removeClass("Red");
        };


        this.initStyles();
        this.initEvents();
    };
});