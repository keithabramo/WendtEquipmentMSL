$(function () {

    var MasterShipList = function () {

        this.editsMade = false;
        this.editURL = $("#editFormMock form").attr("action");

        this.initStyles = function () {
        }

        this.initEvents = function () {
            var $this = this;

            $('.table').on('change', "> tbody > tr input, > tbody > tr select", function () {
                $this.editsMade = true;
            });

            $('.table tbody').on('blur', "tr", function () {

                if ($this.editsMade) {
                    var $mockForm = $("#editFormMock form");

                    $mockForm.attr("action", $this.editURL + "/" + $(this).find("input[name='EquipmentId']").val())

                    $(this).find("input[name], select[name]").each(function (i, e) {
                        var $element = $(e);

                        $mockForm.find("[name='" + $element.attr("name") + "']").val($element.val());
                    });

                    $mockForm.submit();
                }
                //var $form = $(this).prevAll("form").eq(0);
                
                
                //    $this.editsMade = false;

                //    if ($form.length) {

                //        $form.submit();
                //    }
                //}
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});