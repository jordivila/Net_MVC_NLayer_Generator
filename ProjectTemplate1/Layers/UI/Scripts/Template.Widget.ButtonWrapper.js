/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.widgetButton", jQuery.ui.widgetBase,
{
    options: {

    },
    _create: function () {

        jQuery.ui.widgetBase.prototype._create.call(this);

    },
    _init: function () {

        jQuery.ui.widgetBase.prototype._init.call(this);

        var $el = jQuery(this.element);

        //if button requires confirmation. We append functionality to do so
        if ($el.attr('data-widget-confirmButton') != undefined) {
            var $btn = $el;
            var proxiedClick = null;
            if ($btn.attr('onclick') != undefined) {
                proxiedClick = $btn.attr('onclick');
                $btn.removeAttr('onclick');
            }

            var answered = null;

            var newClick = function () {

                $customNamespace$.Widgets.Dialogs.createOkCancelMessage($customNamespace$.Resources.confirmToContinue
                                                                , function () {
                                                                    answered = true;
                                                                    if (proxiedClick != null) {
                                                                        $btn.attr('onclick', proxiedClick);
                                                                        $btn.unbind('click');
                                                                        $btn.click();
                                                                        $btn.removeAttr('onclick');
                                                                        $btn.bind('click', null, newClick);
                                                                    }
                                                                    else {
                                                                        $btn.click();
                                                                    }
                                                                }
                                                                , function () {

                                                                });
                if (answered != null) {
                    answered = null;    // reinitializes flag
                }
                else {
                    return false;
                }
            }
            $btn.bind('click', null, newClick);
        };
    },
    destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
});