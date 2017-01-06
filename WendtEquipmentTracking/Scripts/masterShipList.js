$(function () {

    var MasterShipList = function () {

        this.editsMade = false;

        this.initStyles = function () {
        }

        this.initEvents = function () {
            var $this = this;

            $('.table').on('click', ".expand", function () {
                var $icon = $(this).find(".glyphicon");

                if ($icon.hasClass("glyphicon-plus")) {
                    $icon.removeClass("glyphicon-plus").addClass("glyphicon-minus");
                } else {
                    $icon.removeClass("glyphicon-minus").addClass("glyphicon-plus");
                }
            });

            $('.table').on('change', "tr:not(.create) input, tr:not(.create) select", function () {
                $this.editsMade = true;
            });

            $('.table').on('blur', "tr:not(.create)", function () {
                var $form = $(this).prevAll("form").eq(0);
                
                if ($this.editsMade) {
                    $this.editsMade = false;

                    if ($form.length) {

                        $form.submit();
                    }
                }
            });

        }


        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});