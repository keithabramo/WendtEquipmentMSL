Dropzone.autoDiscover = false;

$(function () {

    var Import = function () {

        this.initStyles = function () {
            

            Dropzone.options.myDropzone = {
                maxFiles: 1,
                acceptedFiles: ".xls, .xlsx, .xlsm",
                error: function(file){
                    if (!file.accepted) {
                        this.removeFile(file);
                    }
                },
                success: function (file, response) {
                    $("#equipmentConfiguration").html(response);
                    $("#equipmentRows").html("");
                },
                maxfilesexceeded: function(file) {
                    this.removeAllFiles();
                    this.addFile(file);
               }
            };

            $('#myDropzone').dropzone();
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.table thead .selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    $(".table tbody tr td:first-child [type='checkbox']").prop("checked", true);
                }
            });

            $(document).on("keypress keydown keyup", 'form', function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            
        }

        this.initStyles();
        this.initEvents();
    }

    new Import();

});

function OnComplete() {
    $("[type='submit']").button("reset");
    $.validator.unobtrusive.parse('#EquipmentRows')
    $.validator.unobtrusive.parse('#equipmentConfiguration')
    form.initStyles();
    form.initEvents();
}