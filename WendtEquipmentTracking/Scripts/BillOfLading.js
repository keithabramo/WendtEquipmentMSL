$(function () {

    var BillOfLading = function () {

        this.initStyles = function () {
            
            //$(".datePicker").datepicker();
        }

        this.initEvents = function () {
            var $this = this;
            var $forms = $("form");


            $forms.on("submit", function () {
                $(this).find("button[type='submit']").button("loading");

                return true;
            });

            $(".sites.table").on("click", ".create-bol, .edit-bol, .delete-bol, .view-bol", function () {

                $(this).button("loading");
                waitingDialog.show();
            });

            $(document).on('hidden.bs.modal', ".modal", function () {
                $(this).data('bs.modal', null);
                $(this).remove();
            });

            $(document).on('click', ".modal-cancel", function () {
                $(".modal").modal('hide');

            });

            $('.master-ship-list').on('click', ".expand", function () {
                var $icon = $(this).find(".glyphicon");

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });

        }


        this.initStyles();
        this.initEvents();
    }

    new BillOfLading();

});


function OnAjaxFinish() {
    waitingDialog.hide();

    $("#BOLDialogs .modal").modal({
        width: "100%",
        height: "100%"
    });
};