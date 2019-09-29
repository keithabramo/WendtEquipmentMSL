$(function () {

    var CreateProject = function () {

        this.initStyles = function () {

            $.ajax({
                url: ROOT_URL + "api/AutocompleteApi/Broker/",
                success: function (results) {

                    $(".broker-autocomplete").autocomplete({
                        minLength: 0,
                        source: results,
                        select: function (event, ui) {
                            $.ajax({
                                url: ROOT_URL + "api/BrokerApi/FindByName",
                                data: { name: ui.item.value },
                                success: function (results) {

                                    $("#ShipToBrokerPhoneFax").val(results.PhoneFax);
                                    $("#ShipToBrokerEmail").val(results.Email);

                                }
                            });
                        }
                    }).focus(function () {
                        $(this).data("uiAutocomplete").search($(this).val());
                    });

                }
            });
        };

        this.initEvents = function () {
            var $this = this;

            $(".copy-from").on("change", function () {

                var projectId = $(this).val();

                if (projectId) {
                    $.ajax({
                        url: ROOT_URL + "api/ProjectApi/Get/" + projectId,
                        success: function (result) {
                            if (result) {
                                $.each(result, function (property, value) {

                                    //We don't want to set the is completed checkbox when populating copied values
                                    if (property !== "IsCompleted") {

                                        var $input = $('[name="' + property + '"]');

                                        if ($input.is(":checkbox")) {
                                            if (value) {
                                                $input.attr("checked", "checked").prop("checked", true);
                                            } else {
                                                $input.removeAttr("checked").prop("checked", false);
                                            }
                                        } else {
                                            $('[name="' + property + '"]').val(value);
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            });
        };

        this.initStyles();
        this.initEvents();
    };

    createProject = new CreateProject();

});