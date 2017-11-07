$(function () {

    var MasterShipList = function () {
       
        this.initStyles = function () {
            
            
        }

        this.initEvents = function () {
            var $this = this;

          
        }


        this.resetValidation = function ($row) {

            $row.removeClass("danger");

            $row.find("input[type='checkbox']").removeAttr("data-val-required");
            
            //Removes validation from input-fields
            $row.find('.input-validation-error')
                .addClass('input-validation-valid')
                .removeClass('input-validation-error');
            //Removes validation message after input-fields
            $row.find('.field-validation-error')
                .addClass('field-validation-valid')
                .removeClass('field-validation-error')
                .removeClass('text-danger')
                .text("");

        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});