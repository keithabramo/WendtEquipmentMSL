
$(function () {

    var MSLRender = function () {

        
        this.EquipmentNameRender = function ($cell, rowData) {
            var colIndex = 0;
            var data = rowData.EquipmentName;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data).prop("readOnly", rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit);

            } else {
                $cell.html(data);
            }

            if (rowData.IsAssociatedToHardwareKit) {
                $cell.append("<br> Associated to Hardware Kit: " + rowData.AssociatedHardwareKitNumber);
            }
        }
        this.PriorityRender = function ($cell, rowData) {
            var colIndex = 1;
            var data = rowData.Priority;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("select").val(data);
            } else {
                $cell.html(data);
            }
        }
        this.ReleaseDateRender = function ($cell, rowData) {
            var colIndex = 2;
            var data = rowData.ReleaseDate;

            var $template = $(".template");
            var value = data;
            if (data) {
                //needed to add some hours so chrome timezone interpretation doesn't kick it back a day (added 10 hours)
                value = $.datepicker.formatDate('mm/dd/yy', new Date(data.replace("T00:00:00", "T10:00:00"))); 
            }

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(value).attr("id", "releaseDate" + rowData.EquipmentId);

            } else {
                $cell.html(value);
            }
        }
        this.DrawingNumberRender = function ($cell, rowData) {
            var colIndex = 3;
            var data = rowData.DrawingNumber;

            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.WorkOrderNumberRender = function ($cell, rowData) {
            var colIndex = 4;
            var data = rowData.WorkOrderNumber;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }

            $cell.addClass(rowData.Indicators.WorkOrderNumberColor);
        }
        this.QuantityRender = function ($cell, rowData) {
            var colIndex = 5;
            var data = rowData.Quantity;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data).prop("readOnly", rowData.IsAssociatedToHardwareKit || rowData.IsHardwareKit);

            } else {
                $cell.html(data);
            }
        }
        this.ShippingTagNumberRender = function ($cell, rowData) {
            var colIndex = 6;
            var data = rowData.ShippingTagNumber;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.DescriptionRender = function ($cell, rowData) {
            var colIndex = 7;
            var data = rowData.Description;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("textarea").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.UnitWeightRender = function ($cell, rowData) {
            var colIndex = 8;
            var data = rowData.UnitWeight;
            var $template = $(".template");
            var value = (data ? data.toFixed(2) : (0).toFixed(2));

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(value);

            } else {
                $cell.html(value);
            }

            $cell.addClass(rowData.Indicators.UnitWeightColor);
        }
        this.TotalWeightRender = function ($cell, rowData) {
            var colIndex = 9;
            var data = rowData.TotalWeight;
            var $template = $(".template");
            var value = (data ? data.toFixed(2) : (0).toFixed(2));

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(value);
                $cell.append(value);

            } else {
                $cell.html(value);
            }
        }
        this.TotalWeightShippedRender = function ($cell, rowData) {
            var colIndex = 10;
            var data = rowData.TotalWeightShipped;
            var $template = $(".template");
            var value = (data ? data.toFixed(2) : (0).toFixed(2));

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);
                $cell.append(value);

            } else {
                $cell.html(value);
            }
        }
        this.ReadyToShipRender = function ($cell, rowData) {
            var colIndex = 11;
            var data = rowData.ReadyToShip;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }

            $cell.addClass(rowData.Indicators.ReadyToShipColor);
        }
        this.ShippedQuantityRender = function ($cell, rowData) {
            var colIndex = 12;
            var data = rowData.ShippedQuantity;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);
                $cell.append(data);

            } else {
                $cell.html(data);
            }

            $cell.addClass(rowData.Indicators.ShippedQtyColor);
        }
        this.LeftToShipRender = function ($cell, rowData) {
            var colIndex = 13;
            var data = rowData.LeftToShip;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);
                $cell.append(data);

            } else {
                $cell.html(data);
            }

            $cell.addClass(rowData.Indicators.LeftToShipColor);
        }
        this.FullyShippedRender = function ($cell, rowData) {
            var colIndex = 14;
            var data = rowData.FullyShipped;
            var $template = $(".template");

            var value = "N\A";
            if (data === true) {
                value = "Yes";
            } else if (data === false) {
                value = "No";
            }

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);
                $cell.append(value);
            } else {
                $cell.html(value);
            }

            $cell.addClass(rowData.Indicators.FullyShippedColor);
        }
        this.ShippedFromRender = function ($cell, rowData) {
            var colIndex = 15;
            var data = rowData.ShippedFrom;

            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.CustomsValueRender = function ($cell, rowData) {
            var colIndex = 16;
            var data = rowData.CustomsValue;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

                var value = "$" + (data ? data.toFixed(2) : 0);
                $cell.append(value);

            } else {
                var value = "$" + (data ? data.toFixed(2) : 0);
                $cell.html(value);
            }


            $cell.addClass(rowData.Indicators.CustomsValueColor);
        }
        this.SalePriceRender = function ($cell, rowData) {
            var colIndex = 17;
            var data = rowData.SalePrice;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

                var value = "$" + (data ? data.toFixed(2) : 0);
                $cell.append(value);

            } else {
                var value = "$" + (data ? data.toFixed(2) : 0);
                $cell.html(value);
            }


            $cell.addClass(rowData.Indicators.SalePriceColor);
        }
        this.HTSCodeRender = function ($cell, rowData) {
            var colIndex = 18;
            var data = rowData.HTSCode;

            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.CountryOfOriginRender = function ($cell, rowData) {
            var colIndex = 19;
            var data = rowData.CountryOfOrigin;

            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.NotesRender = function ($cell, rowData) {
            var colIndex = 20;
            var data = rowData.Notes;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("textarea").val(data);

            } else {
                $cell.html(data);
            }
        }
        this.DeleteRender = function ($cell, rowData) {
            var colIndex = 21;
            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("a").attr("href", $cell.find("a").attr("href") + "/" + rowData.EquipmentId);

            } else {
                $cell.html("");
            }
        }
        this.HasBillOfLadingRender = function ($cell, rowData) {
            var colIndex = 22;
            var data = rowData.HasBillOfLading;

            var $template = $(".template");

            if ($template.length) {
                var $cellTemplate = $template.find("div").eq(colIndex).clone();

                $cell.html($cellTemplate.html());
                $cell.find("input").val(rowData.EquipmentId);

                if (rowData.HasBillOfLading) {
                    $cell.append("<span class='glyphicon glyphicon-plus text-primary'></span>");
                }
            } else {
                $cell.html("");
            }

            $cell.attr("data-toggle", "collapse").attr("data-id", rowData.EquipmentId);

        }
    }

    mslRender = new MSLRender();

});