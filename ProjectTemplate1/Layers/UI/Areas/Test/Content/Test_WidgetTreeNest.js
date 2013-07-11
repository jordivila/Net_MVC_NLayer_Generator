(function ($) {
    jQuery(document).ready(function () {
        jQuery("#widgetTreeListNestable").find("button").button();
        initTreesNestable();
    });

    function initTreesNestable() {
        var d = new Date();
        jQuery("#widgetTreeListNestable").find('#treeListNest').treeListNest();
        var dd = new Date();
        jQuery("#widgetTreeListNestable").find('#renderResult').html('Found ' + jQuery("#widgetTreeListNestable").find('li').length.toString() + ' nodes. ' + (dd - d).toString() + ' ms to render the list');
        jQuery("#widgetTreeListNestable").find('#treeListNest').treeListNest('closeNode', jQuery("#widgetTreeListNestable").find('#treeListNest').find('li'));
    }
})(jQuery);
