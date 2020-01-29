$(function () {

    var Main = function () {

        this.fontStyle;

        this.initStyle = function () {
            this.fontStyle = $("<style/>");
            this.fontStyle.appendTo('head');
        };

        this.initEvents = function () {
            var $this = this;

            $(".fontSize").on("change", function () {
                var fontSize = $(this).val();

                if (fontSize && !isNaN(fontSize)) {
                    $this.fontStyle.text(".table-styled tbody td, .table-styled thead th, .table-styled thead th input.form-control {font-size: " + fontSize + "px !important;}");

                    $(this).trigger("fontadjusted");

                }
            });

        };

        this.error = function (message) {
            $(".global-message").html('<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Error! </strong>' + message + '</div>');

            window.scrollTo(0, 0);
        };

        this.success = function (message) {
            $(".global-message").html('<div class="alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Success! </strong>' + message + '</div>');

            window.scrollTo(0, 0);
        };

        this.warning = function (message) {
            $(".global-message").html('<div class="alert alert-warning alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Warning! </strong>' + message + '</div>');

            window.scrollTo(0, 0);
        };

        this.pad = function (str, max) {
            str = str.toString();
            return str.length < max ? this.pad("0" + str, max) : str;
        };

        this.initStyle();
        this.initEvents();
    };

    main = new Main();

});