$(function () {

    var Form = function () {

        this.initStyles = function () {
            
            //$(".hasDatepicker").datepicker('remove'); //detach
            $(".table .datePicker").datepicker({
                onSelect: function () {
                    // The "this" keyword refers to the input (in this case: #someinput)
                    if ($(this).closest("td").length) {
                        $(this).change();
                        $(this).closest("td").next().find("input").focus();
                        
                    }
                }
            });

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