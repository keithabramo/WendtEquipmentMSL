Dropzone.autoDiscover = false;

$(function () {

    var TableEquipmentAttachment = function () {

        this.tableMain = new Table('.equipment-attachment-datatable');

        this.init = function (equipmentId) {
            var $this = this;

            if (this.tableMain.datatable) {
                this.tableMain.datatable.destroy();
            }

            this.initEditor();
            this.initDatatable(equipmentId);

            $("#myDropzone #equipmentId").val(equipmentId);
            $("#viewAllAttachments").attr("href", ROOT_URL + "EquipmentAttachment/ViewAll?equipmentId=" + equipmentId);

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
                    url: ROOT_URL + "api/EquipmentAttachmentApi/Delete",
                    dataSrc: ""
                },
                idSrc: 'EquipmentAttachmentId'
            });
        };

        this.initDatatable = function (equipmentId) {
            var $this = this;

            this.tableMain.initDatatable({
                ajax: {
                    url: ROOT_URL + "api/EquipmentAttachmentApi/Table/",
                    dataSrc: "",
                    data: {
                        "equipmentId": equipmentId
                    }
                },
                rowId: 'EquipmentAttachmentId',
                order: [[0, 'asc']],
                columnDefs: [
                    
                    {
                        data: "FileTitle", targets: 0,
                        render: function (data, type, row, meta) {
                            return "<a target='_blank' href='" + ROOT_URL + "Attachments/" + row.FileName + "'>" + row.FileTitle + "</a>";
                        }
                    },
                    {
                        data: "EquipmentAttachmentId", targets: 1,
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

    tableEquipmentAttachment = new TableEquipmentAttachment();

});