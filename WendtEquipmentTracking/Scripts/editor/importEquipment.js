Dropzone.autoDiscover = false;

$(function () {

    var ImportEquipment = function () {

        this.equipmentTable;

        this.initStyles = function () {
            var $this = this;

            Dropzone.options.myDropzone = {
                maxFiles: 1,
                acceptedFiles: ".xls, .xlsx, .xlsm",
                error: function (file) {
                    if (!file.accepted) {
                        this.removeFile(file);
                    }
                },
                success: function (file, response) {
                    $("#equipmentConfiguration").html(response);
                    $.validator.unobtrusive.parse('#equipmentConfiguration')
                    $("[type='submit']").button("reset");

                    form.initStyles();
                    form.initEvents();
                },
                maxfilesexceeded: function (file) {
                    this.removeAllFiles();
                    this.addFile(file);
                }
            };

            $('#myDropzone').dropzone();
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on("submit", "#equipmentConfigurationForm", function (e) {
                e.preventDefault();

                if ($(this).valid()) {

                    if ($this.equipmentTable) {
                        editorMain.datatable.ajax.reload();
                    } else {

                        $this.equipmentTable = new ImportEquipmentTable();

                        $("#equipmentRows").show();
                    }
                }

                $("[type='submit']").button("reset");

            });
        }

        this.initStyles();
        this.initEvents();
    }

    new ImportEquipment();

});

function OnComplete() {


    $("[type='submit']").button("reset");
    $.validator.unobtrusive.parse('#equipmentConfiguration')
    form.initStyles();
    form.initEvents();
}