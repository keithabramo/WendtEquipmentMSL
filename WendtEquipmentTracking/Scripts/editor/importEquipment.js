Dropzone.autoDiscover = false;

$(function () {

    var ImportEquipment = function () {

        this.equipmentTable;
        this.dropzone;

        this.initStyles = function () {
            var $this = this;

            Dropzone.options.myDropzone = {
                acceptedFiles: ".xls, .xlsx, .xlsm",
                addRemoveLinks: true,
                error: function (file) {
                    if (!file.accepted) {
                        this.removeFile(file);
                        main.error("File type not supported. Accepted file types includ; .xls, .xlsx, .xlsm");
                    } 
                },
                success: function (file, response) {
                    if (!response.Error) {
                        $("#equipmentConfiguration")
                            .append($("<input>")
                                        .attr("data-fileid", file.name)
                                        .attr("name", "FilePath")
                                        .attr("type", "hidden")
                                        .val(response.DrawingNumber + "+" + response.FilePath));

                    }
                    else {
                        this.removeFile(file);
                        main.error(response.Error);
                    }
                }
            };

            this.dropzone = new Dropzone("#myDropzone");
        }

        this.initEvents = function () {
            var $this = this;



            this.dropzone.on("removedfile", function (file) {
                $("#equipmentConfiguration").find("input[data-fileid='" + file.name + "']").remove();
                if ($this.editorMain.datatable.rows().any()) {
                    $("#equipmentConfigurationForm").submit();
                }
            });

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