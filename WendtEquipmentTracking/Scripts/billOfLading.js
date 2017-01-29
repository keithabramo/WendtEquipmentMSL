$(function () {

    var BillOfLading = function () {

        this.initStyles = function () {
            
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    table.DataTable().$("input[type='checkbox']").prop("checked", true);
                }
            });

            $('form').submit(function () {
                var $inputs = table.DataTable().$('input, select');
                
                $(this).append($inputs);
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new BillOfLading();

});