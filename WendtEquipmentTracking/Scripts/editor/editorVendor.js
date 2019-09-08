$(function () {

    var EditorVendor = function () {

        this.initStyles = function () {
            var $this = this;

            this.initEditor();
            this.initDatatable();

            $("div.createButtonContainer").append('<input type="button" value="Create" class="btn btn-sm btn-primary createSubmit" />');

        };

        this.initEvents = function () {
            var $this = this;

            editorMain.editor.on('preSubmit', function (e, data, action) {
                if (action !== 'remove') {
                    var name = this.field('Name');

                    // Only validate user input values - different values indicate that
                    // the end user has not entered a value
                    if (!name.isMultiValue()) {
                        if (!name.val()) {
                            name.error('A vendor name is required');
                        }
                    }


                    // If any error was reported, cancel the submission so it can be corrected
                    if (this.inError()) {
                        return false;
                    }
                }
            });

            editorMain.editor.on('postEdit', function (e, json, data) {

                var row = editorMain.datatable.row("#" + data.VendorId);

                if (data.IsDuplicate) {
                    $(row.node()).attr("class", 'warning');
                } else {
                    $(row.node()).removeClass('warning');
                }

            });

            $(".createSubmit").on("click", function () {

                var $row = $(".table tfoot tr");

                var form = editorMain.editor.create(false);
                form.set('Name', $row.find("input[name='Name']").val());
                form.set('Address', $row.find("input[name='Address']").val());
                form.set('Contact1', $row.find("input[name='Contact1']").val());
                form.set('PhoneFax', $row.find("input[name='PhoneFax']").val());
                form.set('Email', $row.find("input[name='Email']").val());
                
                form.submit();

                editorMain.datatable.draw();
            });

            editorMain.editor.on('postCreate', function (e, json, data) {

                var $createRow = $("tfoot tr");

                $createRow.find(":input").val("");
            });
        };

        this.initEditor = function () {

            editorMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/VendorApi/Editor",
                    dataSrc: ""
                },
                idSrc: 'VendorId',
                fields: [
                    {
                        label: "Vendor Id",
                        name: "VendorId"
                    }, {
                        label: "Project Id",
                        name: "ProjectId"
                    }, {
                        label: "Vendor",
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
        }

        this.initDatatable = function () {
            var $this = this;

            editorMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/VendorApi/Table",
                    dataSrc: ""
                },
                rowId: 'VendorId',
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

    editorVendor = new EditorVendor();

});