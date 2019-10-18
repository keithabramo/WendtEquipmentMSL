Dropzone.autoDiscover = false;

$(function () {

    var ImportBroker = function () {

        this.brokerTable;
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

                        if ($this.brokerTable) {
                            $this.brokerTable.editorMain.datatable.ajax.reload();
                        } else {

                            $this.brokerTable = new ImportBrokerTable();

                            $("#brokerRows").show();
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

    importBroker = new ImportBroker();

});