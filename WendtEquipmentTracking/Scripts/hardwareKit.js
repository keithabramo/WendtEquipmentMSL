$(function () {

    var HardwareKit = function () {

        this.initStyles = function () {
        }

        this.initEvents = function () {
            var $this = this;

            $('.table thead .selectAll').on('change', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    $(".table tbody tr td:first-child [type='checkbox']").prop("checked", true);
                }
            });

            $("input[name='ExtraQuantityPercentage']").on("change", function () {
                var newPercent = parseInt($(this).val(), 10);

                $(".table .text-box").each(function () {
                    var originalQuantity = parseInt($(this).closest("tr").find(".original-quantity").val(), 10);

                    var newValue = originalQuantity + (newPercent * originalQuantity) / 100;

                    $(this).val(Math.ceil(newValue));
                });
            });

        }

        this.initStyles();
        this.initEvents();
    }

    new HardwareKit();

});