$(function () {

    var EditorPriority = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();
        }

        this.initEvents = function () {
            var $this = this;

            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var priorityNumber = this.field('PriorityNumber');
                    var priorityId = this.field('PriorityId');
                    var dueDate = this.field('DueDate');
                    var endDate = this.field('EndDate');
                    var contractualShipDate = this.field('ContractualShipDate');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!priorityNumber.isMultiValue()) {
                        if (!priorityNumber.val()) {
                            priorityNumber.error('A priority number is required');
                        } else if (isNaN(priorityNumber.val())) {
                            priorityNumber.error('The priority is not a valid number');
                        } else {
                            $.ajax({
                                url: ROOT_URL + "Validate/ValidPriorityNumber",
                                data: { PriorityNumber: priorityNumber.val(), PriorityId: priorityId.val() },
                                async: false,
                                success: function (result) {
                                    if (result == false) {
                                        priorityNumber.error('This priority number already exists');
                                    }
                                }
                            });
                        }
                    }


                    if (!dueDate.isMultiValue()) {
                        if (!dueDate.val()) {
                            dueDate.error('A start date is required');
                        } else if (!moment(dueDate.val(), 'M/D/YY', true).isValid()) {
                            dueDate.error('This date must be in the format m/d/yy');
                        }
                    }

                    if (!endDate.isMultiValue() && endDate.val()) {
                        if (!moment(endDate.val(), 'M/D/YY', true).isValid()) {
                            endDate.error('This date must be in the format m/d/yy');
                        }
                    }

                    if (!contractualShipDate.isMultiValue() && contractualShipDate.val()) {
                        if (!moment(contractualShipDate.val(), 'M/D/YY', true).isValid()) {
                            contractualShipDate.error('This date must be in the format m/d/yy');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }
            });

            editorMain.datatable.on('preAutoFill', function (e, datatable, cells) {
                datatable.cell.blur();
            });
        }

        this.initEditor = function () {

            editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/PriorityApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'PriorityId',
                fields: [
                    { name: "PriorityNumber" },
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
                    { name: "EquipmentName" },
                    { name: "PriorityId" }
                ]
            });
        }

        this.initDatatable = function () {
            var $this = this;

            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/PriorityApi/Table",
                    dataSrc: ""
                },
                rowId: 'PriorityId',
                columns: [
                    { data: "PriorityNumber" },
                    { data: "DueDate" },
                    { data: "EndDate" },
                    { data: "ContractualShipDate" },
                    { data: "EquipmentName" },
                    { data: "PriorityId" }
                ],
                keys: {
                    columns: [0, 1, 2, 3, 4]
                },
                autoFill: {
                    columns: [1, 2, 3, 4]
                },
                columnDefs: [
                    {
                        "targets": 0, className: "priorityWidth"
                    },
                    {
                        "targets": 1, className: "dateWidth", type: "date"
                    },
                    {
                        "targets": 2, className: "dateWidth", type: "date"
                    },
                    {
                        "targets": 3, className: "dateWidth", type: "date"
                    },
                    {
                        "targets": 5,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return canReadWrite === "true" ? '<a href="javascript:void(0);" class="delete">Delete</a>' : '';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        }

        this.initStyles();
        this.initEvents();
    };

    editorPriority = new EditorPriority();

});