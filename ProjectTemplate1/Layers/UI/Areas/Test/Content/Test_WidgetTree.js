
(function ($) {
    jQuery(document).ready(function () {

        var $treeList = jQuery('#treeList');

        jQuery("#widgetTreeList")
            .find("input[type='button']")
                .button()
            .end()
            .find("#treeList_destroy")
                .click(function () {
                    $treeList.treeList('destroy');
                })
            .end()
            .find("#treeList_create")
                .click(function () {
                    $treeList.treeList();
                })
            .end()
            .find("#treeList_selectFirst")
                .click(function () {
                    $treeList.treeList('selected', $treeList.find('li:nth-child(1):first'));
                })
            .end()
            .find("#treeList_selected")
                .click(function () {
                    alert($treeList.treeList('selected'));
                })
            .end()
            .find("#treeList_Expand")
                .click(function () {
                    $treeList.treeList('openNode', $treeList.find('li'));
                })
            .end()
            .find("#treeList_Collapse")
                .click(function () {
                    $treeList.treeList('closeNode', $treeList.find('li'));
                })
            .end();

        initTrees();
    });

    function initTrees() {
        var d = new Date();
        jQuery("#widgetTreeList").find('#treeList').treeList();
        var dd = new Date();
        jQuery("#widgetTreeList").find('#renderResult').html('Found ' + jQuery("#widgetTreeList").find('li').length.toString() + ' nodes. ' + (dd - d).toString() + ' ms to render the list');
        jQuery("#widgetTreeList").find('#treeList').treeList('closeNode', jQuery("#widgetTreeList").find('#treeList').find('li'));
    }
})(jQuery);

