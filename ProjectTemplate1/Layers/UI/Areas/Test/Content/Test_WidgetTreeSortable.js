
(function ($) {
    jQuery(document).ready(function () {
        jQuery("#widgetTreeListSortable").find('button').button();
        initTreesSortable();
    });

    function initTreesSortable() {
        var d = new Date();
        jQuery("#widgetTreeListSortable").find('#treeListSortable').treeListSortable();
        var dd = new Date();
        jQuery("#widgetTreeListSortable").find('#renderResult').html('Found ' + jQuery("#widgetTreeListSortable").find('li').length.toString() + ' nodes. ' + (dd - d).toString() + ' ms to render the list');
    }
})(jQuery);
