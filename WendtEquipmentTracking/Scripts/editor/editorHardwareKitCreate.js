﻿$(function () {

    var EditorHardwareKit = function () {

        this.canSubmit = false;
        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            $.validator.unobtrusive.parse('#HardwareKitForm');


            this.initEditor();
            this.initDatatable();
        };

        this.initEvents = function () {
            var $this = this;

            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var quantityToShip = this.field('QuantityToShip');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!quantityToShip.isMultiValue()) {
                        if (!quantityToShip.val()) {
                            quantityToShip.error('A quantity is required');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }

                data.doSubmit = $this.canSubmit;
                if ($this.canSubmit) {


                    data.HardwareKitNumber = $("#HardwareKitNumber").val();
                    data.ExtraQuantityPercentage = $("#ExtraQuantityPercentage").val();
                    data.HardwareKitId = $("#HardwareKitId").val() || "";
                }

            });

            $("input[name='ExtraQuantityPercentage']").on("change", function () {
                var newPercent = parseInt($(this).val(), 10);

                $this.editorMain.datatable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    var data = this.data();

                    var originalQuantity = data.Quantity;
                    var newValue = originalQuantity + (newPercent * originalQuantity) / 100;

                    $this.editorMain.datatable.cell(rowIdx, 4).data(Math.ceil(newValue));

                });
            });

            $("#Save").on("click", function () {

                if ($("#HardwareKitForm").valid()) {
                    $this.canSubmit = true;

                    var selectedRows = $this.editorMain.datatable.rows({ selected: true }).indexes();
                    if (selectedRows.length) {
                        $this.editorMain.editor.edit(
                            selectedRows, false
                        ).submit(function () {
                            $this.canSubmit = false;
                            $("#Save").button("reset");
                            location.href = ROOT_URL + "HardwareKit/?ajaxSuccess=true";
                        }, function () {
                            $this.canSubmit = false;
                            $("#Save").button("reset");

                            main.error("There was an error whill trying to save this hardware kit");
                        });
                    } else {
                        $("#Save").button("reset");

                        main.error("You must select at least one record");
                    }
                } else {
                    $("#Save").button("reset");

                    main.error("Please address the validation errors before saving");
                }
            });

        };

        this.initEditor = function () {

            this.editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/HardwareKitApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'HardwareKitGroupId',
                fields: [
                    { name: "HardwareKitGroupId" },
                    { name: "ShippingTagNumber" },
                    { name: "Description" },
                    { name: "Quantity" },
                    { name: "OriginalQuantity" },
                    { name: "QuantityToShip" }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/HardwareKitApi/CreateTable",
                    dataSrc: "",
                    data: {
                        hardwareKitId: $("#HardwareKitId").val()
                    }
                },
                rowId: 'HardwareKitGroupId',
                order: [[1, 'asc']],
                autoWidth: false,
                columnDefs: [
                    {
                        data: "HardwareKitGroupId",
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0,
                        render: function () {
                            return "<span></span>"
                        }
                    },
                    {
                        data: "ShippingTagNumber", "targets": 1,
                        className: "active"
                    },
                    {
                        data: "Description", "targets": 2,
                        className: "active"
                    },
                    {
                        data: "Quantity", "targets": 3,
                        className: "active"
                    },
                    {
                        data: "QuantityToShip", "targets": 4
                    }
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                keys: {
                    columns: [4]
                },
                autoFill: {
                    editor: null,
                    columns: [4]
                }
            });
        };

        this.initStyles();
        this.initEvents();
    };

    editorHardwareKit = new EditorHardwareKit();

});