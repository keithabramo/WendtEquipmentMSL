$(function () {

    var Project = function () {

        this.currentPercent;

        this.initStyles = function () {
            table.DataTable().order([0, 'desc']).draw();
        }
        this.initStyles();
    }

    new Project();

});