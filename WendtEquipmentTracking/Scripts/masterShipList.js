$(function () {

    var MasterShipList = function () {

        this.editsMade = false;
        this.createURL = "/Equipment/Create";
        this.editURL = "/Equipment/Edit";

        this.initStyles = function () {

        }

        this.initEvents = function () {
            var $this = this;

            $('.table').on('change', "> tbody > tr input, > tbody > tr select", function () {
                $this.editsMade = true;
            });

            $('.table tbody').on('blur', "tr", function () {

                if ($this.editsMade) {

                    $this.editsMade = false;

                    var $row = $(this);
                    var url = $this.editURL + "/" + $row.find("input[name='EquipmentId']").val();
                    var $form = $("<form/>");
                    
                    $row.find("input[name], select[name]").each(function (i, e) {
                        var $element = $(e).clone();

                        $form.append($element);
                    });

                    $.ajax({
                        type: "POST",
                        url: url,
                        data: $form.serialize(),
                        success: function (data) {

                            $row.replaceWith(data);
                            //table.DataTable().draw();
                        }
                    });
                }

                
            });

            $('.table tfoot').on('click', ".createSubmit", function () {


                var $row = $(this).closest("tfoot");
                var url = $this.createURL;
                var $form = $("<form/>");

                $row.find("input[name], select[name]").each(function (i, e) {
                    var $element = $(e).clone();

                    $form.append($element);
                });

                $.ajax({
                    type: "POST",
                    url: url,
                    data: $form.serialize(),
                    success: function (data) {
                        table.DataTable().row.add($(data)[0]).draw();

                        $row.find("input, select").val("");
                    }
                });
            });
        }

        this.initStyles();
        this.initEvents();
    }

    new MasterShipList();

});