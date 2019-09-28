﻿$(function () {

    var EditProject = function () {

        this.initStyles = function () {
            $.ajax({
                url: ROOT_URL + "api/BrokerApi/Search/",
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

        this.initStyles();
    };

    editProject = new EditProject();

});