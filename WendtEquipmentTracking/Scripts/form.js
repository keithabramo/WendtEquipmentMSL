$(function () {

    var Form = function () {

        this.invalid = false;

        this.initStyles = function () {

            //hack since create row is being copyed for the scrolly stuff we need to remove extra id so datepicker stops breaking

            $(".table .datePickerTable").datepicker({
                dateFormat: 'm/d/y',
                onSelect: function () {
                    // The "this" keyword refers to the input (in this case: #someinput)
                    if ($(this).closest("td").length) {
                        $(this).change();
                        $(this).closest("td").next().find("input").focus();

                    }
                }
            }).datepicker("setDate", new Date());

            $(".datePicker").attr("autocomplete", "off").datepicker({
                dateFormat: 'm/d/y'
            });

        };

        this.initEvents = function () {
            var $this = this;
            var $forms = $("form");


            $("[type='submit'], input#Save[type='button']").on("click", function () {
                $(this).button("loading");
            });

            $forms.on('invalid-form.validate', function () {
                $("[type='submit'], input#Save[type='button']").button("reset");
                $this.invalid = true;
            });
        };


        this.initStyles();
        this.initEvents();
    };

    form = new Form();

});