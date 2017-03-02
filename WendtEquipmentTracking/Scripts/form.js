﻿$(function () {

    var Form = function () {

        this.initStyles = function () {
            
            //hack since create row is being copyed for the scrolly stuff we need to remove extra id so datepicker stops breaking
           
            $(".table .datePickerTable").datepicker({
                onSelect: function () {
                    // The "this" keyword refers to the input (in this case: #someinput)
                    if ($(this).closest("td").length) {
                        $(this).change();
                        $(this).closest("td").next().find("input").focus();
                        
                    }
                }
            });

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