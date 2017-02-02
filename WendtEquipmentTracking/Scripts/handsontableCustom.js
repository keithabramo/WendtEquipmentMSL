
$(function () {

    var HandsonTable = function () {

        this.table;

        this.Table = function () {
            return this.table;
        };

        this.initStyles = function () {
            
            $.ajax({
                type: "GET",
                url: ROOT_URL + "api/EquipmentApi/Table",
                success: function (data) {


                    this.table = new Handsontable($("#msl-table")[0], {
                        data: data,
                        colHeaders: true,
                        rowHeaders: true,
                        filters: true,
                        dropdownMenu: ['filter_by_condition', 'filter_by_value', 'filter_action_bar'],
                        columnSorting: true,
                        sortIndicator: true,
                        autoRowSize: false,
                        autoColumnSize: { useHeaders: true },
                        viewportRowRenderingOffset: 10,
                        viewportColumnRenderingOffset: 30,
                        manualColumnResize: true,
                        colHeaders: [
                              "EquipmentId",
                                    "IsHardware",
                                    "EquipmentName"
                                ,
                                    "Priority"
                                ,
                                    "ReleaseDate"
                                ,
                                    "DrawingNumber"
                                ,
                                    "WorkOrderNumber"
                                ,
                                    "Quantity"
                                ,
                                    "ShippingTagNumber"
                                ,
                                    "Description"
                                ,
                                    "UnitWeight"
                                ,
                                    "TotalWeight"
                                ,
                                    "TotalWeightShipped"
                                ,
                                    "ReadyToShip"
                                ,
                                    "ShippedQuantity"
                                ,
                                    "LeftToShip"
                                ,
                                    "FullyShipped"
                                ,
                                    "CustomsValue"
                                ,
                                    "SalePrice"
                                ,
                                    "Notes"
                                ,
                                    "AutoShipFile"
                                ,
                                    "SalesOrderNumber"
                        ],
                        columns: [
                          {
                              "data": "EquipmentId"
                          },
                                {
                                    "data": "IsHardware"
                                },
                                {
                                    "data": "EquipmentName"
                                },
                                {
                                    "data": "Priority",
                                    type: 'dropdown',
                                    source: ["01", "P02", "P03", "P04", "P05", "P06", "P07", "P08", "P09", "P10", "A1"]
                                },
                                {
                                    "data": "ReleaseDate",
                                    type: 'date',
                                    dateFormat: 'MM/DD/YYYY',
                                    correctFormat: true
                                },
                                {
                                    "data": "DrawingNumber"
                                },
                                {
                                    "data": "WorkOrderNumber"
                                },
                                {
                                    "data": "Quantity",
                                    type: 'numeric'
                                },
                                {
                                    "data": "ShippingTagNumber"
                                },
                                {
                                    "data": "Description"
                                },
                                {
                                    "data": "UnitWeight",
                                    type: 'numeric',
                                    format: '0.00'
                                },
                                {
                                    "data": "TotalWeight",
                                    type: 'numeric',
                                    format: '0.00'
                                },
                                {
                                    "data": "TotalWeightShipped",
                                    type: 'numeric',
                                    format: '0.00'
                                },
                                {
                                    "data": "ReadyToShip",
                                    type: 'numeric'
                                },
                                {
                                    "data": "ShippedQuantity",
                                    type: 'numeric'
                                },
                                {
                                    "data": "LeftToShip",
                                    type: 'numeric'
                                },
                                {
                                    "data": "FullyShipped"
                                },
                                {
                                    "data": "CustomsValue",
                                    type: 'numeric',
                                    format: '$ 0,0.00'
                                },
                                {
                                    "data": "SalePrice",
                                    type: 'numeric',
                                    format: '$ 0,0.00'
                                },
                                {
                                    "data": "Notes"
                                },
                                {
                                    "data": "AutoShipFile"
                                },
                                {
                                    "data": "SalesOrderNumber"
                                },
                                {
                                    "data": "HasBillOfLading"
                                },
                                { "data": "IsHardwareKit" },
                                { "data": "IsAssociatedToHardwareKit"},
                                { "data": "AssociatedHardwareKitNumber" },
                                { "data": "Indicators" }
                        ],
                        minSpareRows: 1
                    });


                    
                }
            });













        }

        this.initEvents = function () {
            var $this = this;



        }


        this.initStyles();
        this.initEvents();
    }

    handsonTable = new HandsonTable();

});

function DeleteSuccess() {
    $(this).closest("tr").remove();
};