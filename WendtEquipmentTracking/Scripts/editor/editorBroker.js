﻿$(function () {

    var EditorBroker = function () {

        this.editorMain = new Editor();

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

        };

        this.initEvents = function () {
            var $this = this;

            this.editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var name = this.field('Name');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!name.isMultiValue()) {
                        if (!name.val()) {
                            name.error('A broker name is required');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }
            });

            this.editorMain.editor.on('postEdit', function (e, json, data) {

                var row = $this.editorMain.datatable.row("#" + data.BrokerId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = $this.editorMain.editor.create(false);
                form.set('Name', $row.find("input[name='Name']").val());
                form.set('Address', $row.find("input[name='Address']").val());
                form.set('Contact1', $row.find("input[name='Contact1']").val());
                form.set('PhoneFax', $row.find("input[name='PhoneFax']").val());
                form.set('Email', $row.find("input[name='Email']").val());
                
                form.submit();

                $this.editorMain.datatable.draw();
            });

            this.editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
            });
        };

        this.initEditor = function () {

            this.editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/BrokerApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'BrokerId',
                fields: [
                    {
                        label: "Broker Id",
                        name: "BrokerId"
                    }, {
                        label: "Broker",
                        name: "Name"
                    }, {
                        label: "Address",
                        name: "Address"
                    }, {
                        label: "Contact 1",
                        name: "Contact1"
                    }, {
                        label: "Phone/Fax",
                        name: "PhoneFax"
                    }, {
                        label: "Email",
                        name: "Email"
                    }
                ]
            });
        };

        this.initDatatable = function () {
            var $this = this;

            this.editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BrokerApi/Table",
                    dataSrc: ""
                },
                rowId: 'BrokerId',
                columns: [
                    { data: "Name" },
                    { data: "Address" },
                    { data: "Contact1" },
                    { data: "PhoneFax" },
                    { data: "Email" }
                ],
                columnDefs: [
                    {
                        "targets": 5,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        }
                    }
                ],
                createdRow: function (row, data, index) {
                    if (data.IsDuplicate) {
                        $(row).addClass('warning');
                    }
                }
            });
        };

        this.initStyles();
        this.initEvents();
    };

    editorBroker = new EditorBroker();

});