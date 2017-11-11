$(function () {

    var Main = function () {

        this.error = function (message) {
            $(".global-message").html('<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>ERROR! </strong>' + message + '</div>');

            window.scrollTo(0, 0);
        }
    }

    main = new Main();

});