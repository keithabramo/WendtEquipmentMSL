
$(function () {

    var colorClasses = "White Red Yellow Green Purple Pink Fuchsia";

    var MSLRender = function () {

        this.EquipmentNameRender = function ($cell, rowData) {

            $cell.find("input").val(data).prop("readOnly", rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit);



            if (rowData.IsAssociatedToHardwareKit) {
                $cell.append("<br> Associated to Hardware Kit: " + rowData.AssociatedHardwareKitNumber);
            }

        }

        this.ReleaseDateRender = function ($cell, rowData) {

            //needed to add some hours so chrome timezone interpretation doesn't kick it back a day (added 10 hours)
            value = $.datepicker.formatDate('mm/dd/yy', new Date(data.replace("T00:00:00", "T10:00:00")));

        }

        this.QuantityRender = function ($cell, rowData) {
            $cell.find("input").val(data).prop("readOnly", rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit);
        }

        this.UnitWeightRender = function ($cell, rowData) {
            var value = (data ? data.toFixed(2) : (0).toFixed(2));
        }
        this.TotalWeightRender = function ($cell, rowData) {
            var value = (data ? data.toFixed(2) : (0).toFixed(2));
        }
        this.TotalWeightShippedRender = function ($cell, rowData) {
            var value = (data ? data.toFixed(2) : (0).toFixed(2));
        }

        this.CustomsValueRender = function ($cell, rowData) {
            var value = "$" + (data ? data.toFixed(2) : 0);
        }
        this.SalePriceRender = function ($cell, rowData) {
            var value = "$" + (data ? data.toFixed(2) : 0);
        }

    }

    mslRender = new MSLRender();

});