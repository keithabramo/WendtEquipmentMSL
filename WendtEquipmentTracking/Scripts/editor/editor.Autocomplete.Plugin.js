(function ($, DataTable) {


    if (!DataTable.ext.editorFields) {
        DataTable.ext.editorFields = {};
    }

    var _fieldTypes = DataTable.Editor ?
        DataTable.Editor.fieldTypes :
        DataTable.ext.editorFields;

    _fieldTypes.autoComplete = {
        create: function (conf) {
            conf._input = $('<input type="text" id="' + conf.id + '">')
                .autocomplete(conf.opts || {});

            return conf._input[0];
        },

        get: function (conf) {
            return conf._input.val();
        },

        set: function (conf, val) {
            conf._input.val(val);
        },

        enable: function (conf) {
            conf._input.autocomplete('enable');
        },

        disable: function (conf) {
            conf._input.autocomplete('disable');
        },

        canReturnSubmit: function (conf, node) {
            if ($('ul.ui-autocomplete').is(':visible')) {
                return false;
            }
            return true;
        },

        owns: function (conf, node) {
            if ($(node).closest('ul.ui.autocomplete').length) {
                return true;
            }
            return false;
        },

        // Non-standard Editor method - custom to this plug-in
        node: function (conf) {
            return conf._input;
        },

        update: function (conf, options) {
            conf._input.autocomplete('option', 'source', options);
        }
    };


})(jQuery, jQuery.fn.dataTable);