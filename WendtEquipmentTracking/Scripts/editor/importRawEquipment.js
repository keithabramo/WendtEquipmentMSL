Dropzone.autoDiscover = false;

$(function () {

    var ImportRawEquipment = function () {

        this.rawEquipmentTable;
        this.dropzone;

        this.initStyles = function () {
            var $this = this;

            Dropzone.options.myDropzone = {
                maxFiles: 1,
                acceptedFiles: ".xls, .xlsx, .xlsm",
                addRemoveLinks: true,
                error: function (file) {
                    if (!file.accepted) {
                        this.removeFile(file);
                    }
                },
                success: function (file, response) {
                    if (!response.Error) {
                        $("#FilePath").val(response.FilePath);

                        if ($this.rawEquipmentTable) {
                            $this.rawEquipmentTable.editorMain.datatable.ajax.reload(function () {
                                $this.rawEquipmentTable.editorMain.datatable.rows().select();
                                $(".table thead th.select-checkbox").closest("tr").addClass("selected");
                            });
                        } else {

                            $this.rawEquipmentTable = new ImportRawEquipmentTable();

                            $("#rawEquipmentRows").show();
                        }

                    }
                    else {
                        this.removeFile(file);
                        main.error(response.Error);
                    }
                },
                maxfilesexceeded: function (file) {
                    this.removeAllFiles();
                    this.addFile(file);
                }
            };

            this.dropzone = new Dropzone("#myDropzone");
        }

        this.initEvents = function () {
            var $this = this;

        }

        this.initStyles();
        this.initEvents();
    }

    importRawEquipment = new ImportRawEquipment();

});