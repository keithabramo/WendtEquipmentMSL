$(function () {

    var Form = function () {

        this.initStyles = function () {
            
            $(".datePicker").datepicker();

            $(".autocomplete").autocomplete({
                source: "/api/WorkOrderPriceApi/Search"
            });
        }

        this.initEvents = function () {
            var $this = this;
            var $forms = $("form");


            $forms.on("submit", function () {
                if ($(this).valid()) {
                    $(this).find("[type='submit']").button("loading");
                    return true;
                }
            });
        }


        this.initStyles();
        this.initEvents();
    }

    form = new Form();

});