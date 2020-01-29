Dropzone.autoDiscover = false;

$(function () {

    var ImportEquipmentRevision = function () {

        this.equipmentRevisionTable;
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
                        $("#equipmentRevisionConfiguration")
                            .append($("<input>")
                                .attr("data-fileid", file.name)
                                .attr("name", "FilePath")
                                .attr("type", "hidden")
                                .val(response.FilePath));
                        $("#equipmentRevisionConfigurationForm input[name='DrawingNumber']").val(response.DrawingNumber);
                        $("#equipmentRevisionConfigurationForm input[name='Revision']").val(main.pad(response.Revision, 2));

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
                        $this.equipmentRevisionTable.editorMain.datatable.ajax.reload(function () {
                            $this.equipmentRevisionTable.editorMain.datatable.rows(function (idx, data, node) {
                                return data.HasChanged;
                            }).select();
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