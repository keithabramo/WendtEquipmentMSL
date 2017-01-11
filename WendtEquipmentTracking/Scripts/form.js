$(function () {

    var Form = function () {

        this.initStyles = function () {
            
            $(".datePicker").datepicker();

            $(".autocomplete").autocomplete({
                source: "/api/WorkOrderPriceApi/Search",
                minLength: 2
            });
        }

        this.initEvents = function () {
            var $this = this;
            var $forms = $("form");


            $forms.on("submit", function () {
                $(this).find("button[type='submit']").button("loading");

                return true;
            });
        }


        this.initStyles();
        this.initEvents();
    }

    new Form();

});