Dropzone.autoDiscover = false;

$(function () {

    var TableBOLAttachment = function () {

        this.tableMain = new Table('.billOfLading-attachment-datatable');

        this.init = function (billOfLadingId) {
            var $this = this;

            if (this.tableMain.datatable) {
                this.tableMain.datatable.destroy();
            }

            this.initEditor();
            this.initDatatable(billOfLadingId);

            $("#myDropzone #billOfLadingId").val(billOfLadingId);
            $("#viewAllAttachments").attr("href", ROOT_URL + "BillOfLadingAttachment/ViewAll?billOfLadingId=" + billOfLadingId);

        };

        this.initStyles = function () {
            var $this = this;

            Dropzone.options.myDropzone = {
                //acceptedFiles: ".xls, .xlsx, .xlsm",
                addRemoveLinks: true,
                resizeWidth: 1000,
                error: function (file) {
                    this.removeFile(file);
                },
                success: function (file, response) {
                    this.removeFile(file);

                    if (response.Error) {
                        alert("There was an error uploading this file");
                    }

                    $this.tableMain.datatable.ajax.reload();
                    
                }
            };

            this.dropzone = new Dropzone("#myDropzone");
        };

        this.initEvents = function () {
            var $this = this;
        };

        this.initEditor = function () {

            this.tableMain.initEditor({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingAttachmentApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'BillOfLadingAttachmentId'
            });
        };

        this.initDatatable = function (billOfLadingId) {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/BillOfLadingAttachmentApi/Table/",
                    dataSrc: "",
                    data: {
                        "billOfLadingId": billOfLadingId
                    }
                },
                rowId: 'BillOfLadingAttachmentId',
                order: [[0, 'asc']],
                columnDefs: [
                    
                    {
                        data: "FileTitle", targets: 0,
                        render: function (data, type, row, meta) {
                            return "<a target='_blank' href='" + ROOT_URL + "Attachments/" + row.FileName + "'>" + row.FileTitle + "</a>";
                        }
                    },
                    {
                        data: "BillOfLadingAttachmentId", targets: 1,
                        searchable: false,
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return '<a href="javascript:void(0);" class="delete">Delete</a>';
                        },
                        className: "text-nowrap"
                    }
                ]
            });
        };

        this.initStyles();
        this.initEvents();
    };

    tableBOLAttachment = new TableBOLAttachment();

});