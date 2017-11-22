$(function () {

    var Main = function () {

        this.fontStyle;

        this.initStyle = function () {
            this.fontStyle = $("<style/>");
            this.fontStyle.appendTo('head');
        }

        this.initEvents = function () {
            var $this = this;

            $(".fontSize").on("change", function () {
                var fontSize = $(this).val();

                if (fontSize && !isNaN(fontSize)) {
                    $this.fontStyle.text(".table-styled tbody td {font-size: " + fontSize + "px !important;}");
                }
            });
            
        }

        this.error = function (message) {
            $(".global-message").html('<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Error! </strong>' + message + '</div>');

            window.scrollTo(0, 0);
        }

        this.initStyle();
        this.initEvents();
    }

    main = new Main();

});