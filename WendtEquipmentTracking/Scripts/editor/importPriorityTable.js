$(function () {

    ImportPriorityTable = function () {

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

                var row = $this.editorMain.datatable.row("#" + data.PriorityId);

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
                            location.href = ROOT_URL + "Priority/?ajaxSuccess=true";
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
                    url: ROOT_URL + "api/ImportApi/PriorityEditor",
                    dataSrc: ""
                },
                idSrc: 'PriorityId',
                fields: [
                    {
                        name: "PriorityId"
                    },
                    {
                        name: "PriorityNumber"
                    },
                    {
                        name: "DueDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    {
                        name: "EndDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    {
                        name: "ContractualShipDate",
                        type: "datetime",
                        format: "M/D/YY",
                        opts: {
                            firstDay: 0
                        }
                    },
                    { name: "EquipmentName" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/ImportApi/GetPrioritiesFromImport",
                    dataSrc: "",
                    data: function () {
                        return {
                            FilePath: $("#FilePath").val()
                        };
                    }
                },
                order: [[1, 'asc']],
                rowId: 'PriorityId',
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                },
                columnDefs: [
                    {
                        data: "PriorityId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>";
                        }
                    },
                    { data: "PriorityNumber", targets: 1 },
                    { data: "DueDate", targets: 2 },
                    { data: "EndDate", targets: 3 },
                    { data: "ContractualShipDate", targets: 4 },
                    { data: "EquipmentName", targets: 5 }
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

                var priorityNumber = data.PriorityNumber;
                var dueDate = data.DueDate;
                var endDate = data.EndDate;
                var contractualShipDate = data.ContractualShipDate;
                var equipmentName = data.EquipmentName;


                if (!priorityNumber) {
                    error = true;
                    $this.addError(rowIdx, 1);
                } else if (!parseInt(priorityNumber, 10) || parseInt(priorityNumber, 10) > 99) {
                    error = true;
                    $this.addError(rowIdx, 1);
                } else {
                    $this.removeError(rowIdx, 1);
                }

                if (!dueDate) {
                    error = true;
                    $this.addError(rowIdx, 2);
                } else if (!moment(dueDate, 'M/D/YY', true).isValid()) {
                    error = true;
                    $this.addError(rowIdx, 2);
                } else {
                    $this.removeError(rowIdx, 2);
                }

                if (endDate && !moment(endDate, 'M/D/YY', true).isValid()) {
                    error = true;
                    $this.addError(rowIdx, 3);
                } else {
                    $this.removeError(rowIdx, 3);
                }

                if (contractualShipDate && !moment(contractualShipDate, 'M/D/YY', true).isValid()) {
                    error = true;
                    $this.addError(rowIdx, 4);
                } else {
                    $this.removeError(rowIdx, 4);
                }

                if (!equipmentName) {
                    error = true;
                    $this.addError(rowIdx, 5);
                } else {
                    $this.removeError(rowIdx, 5);
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