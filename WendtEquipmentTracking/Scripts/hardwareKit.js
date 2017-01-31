$(function () {

    var HardwareKit = function () {

        this.currentPercent;

        this.initStyles = function () {
            this.currentPercent = parseInt($("input[name='ExtraQuantityPercentage']").val(), 10);
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

                var deltaPercent = newPercent - $this.currentPercent;

                $(".table .text-box").each(function () {
                    var originalValue = parseInt($(this).val(), 10);

                    //change in value based on the change in percent
                    var changeInValue = (deltaPercent * originalValue) / 100;

                    var newValue = originalValue + changeInValue;

                    $(this).val(Math.ceil(newValue));
                });

                $this.currentPercent = newPercent;
            });

        }

        this.initStyles();
        this.initEvents();
    }

    new HardwareKit();

});