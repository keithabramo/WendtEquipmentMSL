$(function () {

    var Import = function () {

        this.initStyles = function () {


            Dropzone.options.mydropzone = {
                addRemoveLinks: true,
                autoProcessQueue: false, // this is important as you dont want form to be submitted unless you have clicked the submit button
                autoDiscover: false,
                previewsContainer: '#dropzonePreview',
                clickable: false, 
                accept: function (file, done) {
                    console.log("uploaded");
                    done();
                },
                error: function (file, msg) {
                    alert(msg);
                }
            };
        }

        this.initEvents = function () {
            var $this = this;

            $(document).on('change', '.table thead .selectAll', function () {
                var checked = $(this).is(":checked");

                if (checked) {
                    $(".table tbody tr td:first-child [type='checkbox']").prop("checked", true);
                }
            });


            
        }

        this.initStyles();
        this.initEvents();
    }

    new Import();

});

function OnComplete() {
    $("[type='submit']").button("reset");
    $.validator.unobtrusive.parse('#EquipmentRows')
    $.validator.unobtrusive.parse('#equipmentConfiguration')
    form.initStyles();
    form.initEvents();
}