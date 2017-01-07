$(function () {

    var Table = function () {

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
        }


        this.initStyles();
        this.initEvents();
    }

    new Table();

});