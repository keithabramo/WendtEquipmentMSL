$(function () {

    var Import = function () {

        this.initStyles = function () {
            
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.table thead .selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    $(".table tbody tr td:first-child [type='checkbox']").prop("checked", true);
                }
            });


            $(document).on('change', ".table input[name$='IsHardware']", function () {
                var isHardware = $(this).is(":checked");
                var $equipmentNameInput = $(this).closest("tr").find("input[name$='EquipmentName']");

                if (isHardware) {
                    $equipmentNameInput.val("HARDWARE").attr("readonly", "readonly");
                } else {
                    $equipmentNameInput.removeAttr("readonly");
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
    form.initStyles();
    form.initEvents();
}