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

        }

        this.initStyles();
        this.initEvents();
    }

    new HardwareKit();

});