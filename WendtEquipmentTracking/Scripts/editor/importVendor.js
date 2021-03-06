﻿Dropzone.autoDiscover = false;

$(function () {

    var ImportVendor = function () {

        this.vendorTable;
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

                        if ($this.vendorTable) {
                            $this.vendorTable.editorMain.datatable.ajax.reload();
                        } else {

                            $this.vendorTable = new ImportVendorTable();

                            $("#vendorRows").show();
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
        };

        this.initEvents = function () {
            var $this = this;

        };

        this.initStyles();
        this.initEvents();
    };

    importVendor = new ImportVendor();

});