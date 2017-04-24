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
                    OnComplete();
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

            $(document).on("change", "table tbody input[type='checkbox']", function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    $(this).closest("tr").find(":input").not("[type='checkbox']").attr("data-val-required", "true");
                } else {
                    $(this).closest("tr").find(":input").not("[type='checkbox']").removeAttr("data-val-required");
                }
            })
            
        }

        this.initStyles();
        this.initEvents();
    }

    new Import();

});

function OnComplete() {
    $("[type='submit']").button("reset");
    $.validator.unobtrusive.parse('#equipmentRows')
    $.validator.unobtrusive.parse('#equipmentConfiguration')
    form.initStyles();
    form.initEvents();
}