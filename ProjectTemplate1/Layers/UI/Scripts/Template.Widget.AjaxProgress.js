
$customNamespace$.Widgets.AjaxProgress = function () {

    var me = {},
        $ajaxProgress = jQuery('<div class="ui-ajaxProgress-box"><div class="ui-ajaxProgress-boxChild ui-widget ui-widget-content ui-state-active">Plase wait while loading</div></div>')
                        .hide();

    me.Create = function () {
        jQuery('body').append($ajaxProgress);
        jQuery(document)
            .ajaxStart(function () {
                $ajaxProgress.fadeIn();
            })
            .ajaxStop(function () {
                $ajaxProgress.fadeOut();
            });
    };

    return me;
};