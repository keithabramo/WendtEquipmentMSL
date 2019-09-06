$(function () {

    var CreateProject = function () {

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

        this.initEvents();
    };

    createProject = new CreateProject();

});