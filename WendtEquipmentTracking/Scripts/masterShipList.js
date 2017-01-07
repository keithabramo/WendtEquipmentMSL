$(function () {

    var MasterShipList = function () {

        this.editsMade = false;

        this.initStyles = function () {
        }

        this.initEvents = function () {
            var $this = this;

            $('.table').on('change', "> tbody > tr:not(.create, .collapse) input, > tbody > tr:not(.create, .collapse) select", function () {
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