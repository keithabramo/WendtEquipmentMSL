Dropzone.autoDiscover = false;

$(function () {

    var ImportEquipmentRevision = function () {

        this.equipmentRevisionTable;
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
                        $("#equipmentRevisionConfiguration")
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
                $("#equipmentRevisionConfiguration").find("input[data-fileid='" + file.name + "']").remove();
                if ($this.equipmentRevisionTable) {
                    $this.equipmentRevisionTable.editorMain.datatable.ajax.reload();
                }
            });

            $(document).on("submit", "#equipmentRevisionConfigurationForm", function (e) {
                e.preventDefault();

                if ($(this).valid()) {

                    if ($this.equipmentRevisionTable) {
                        $this.equipmentRevisionTable..editorMain.datatable.ajax.reload(function () {
                            $this.equipmentRevisionTable..editorMain.datatable.rows().select();
                            $(".table thead th.select-checkbox").closest("tr").addClass("selected");
                        });
                    } else {

                        $this.equipmentRevisionTable = new ImportEquipmentRevisionTable();

                        $("#equipmentRevisionRows").show();
                    }
                }

                $("[type='submit']").button("reset");

            });
        };

        this.initStyles();
        this.initEvents();
    };

    new ImportEquipmentRevision();

});