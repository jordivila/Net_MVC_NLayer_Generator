/// <reference path="$customNamespace$.A.Intellisense.js" />

$customNamespace$.Widgets.Dialogs =
{
    _html: '<div class="ui-template-dialog"><div class="ui-template-dialogIcon"><span class="ui-icon "></span></div><div class="ui-template-dialogMsg"></div></div>'
    , createCustomInfo: function (message, onClose) {

        var dButtons = {};
        dButtons[$customNamespace$.Resources.close] = function () { jQuery(this).dialog('close'); };

        jQuery($customNamespace$.Widgets.Dialogs._html)
            .find('div.ui-template-dialogIcon span')
                .addClass('ui-icon-info')
                .end()
            .find('div.ui-template-dialogMsg')
                .html(message)
            .end()
            .dialog({
                resizable: false,
                modal: true,
                title: $customNamespace$.Resources.siteTitle,
                buttons: dButtons,
                close: function () {
                    if (onClose) {
                        onClose();
                    }
                    jQuery(this).dialog('destroy');
                }
            });
    }
    , createCustomSuccess: function (message, onOk, onClose) {

        var dButtons = {};
        dButtons[$customNamespace$.Resources.accept] = function () { jQuery(this).dialog('close'); onOk(); };

        jQuery($customNamespace$.Widgets.Dialogs._html)
            .find('div.ui-template-dialogIcon span')
                .addClass('ui-icon-check')
                .end()
            .find('div.ui-template-dialogMsg')
                .html(message)
            .end()
            .dialog({
                resizable: false,
                modal: true,
                title: $customNamespace$.Resources.siteTitle,
                buttons: dButtons,
                close: function () {
                    if (onClose) {
                        onClose();
                    }
                    jQuery(this).dialog('destroy');
                }
            });
    }
    , createOkCancelMessage: function (message, onOk, onKO) {

        var dButtons = {};
        dButtons[$customNamespace$.Resources.accept] = function () { jQuery(this).dialog('close'); onOk(); };
        dButtons[$customNamespace$.Resources.cancel] = function () { jQuery(this).dialog('close'); onKO(); };

        jQuery($customNamespace$.Widgets.Dialogs._html)
            .find('div.ui-template-dialogIcon span')
                .addClass('ui-icon-help')
                .end()
            .find('div.ui-template-dialogMsg')
                .html(message)
            .end()
            .dialog({
                resizable: false,
                modal: true,
                title: $customNamespace$.Resources.siteTitle,
                buttons: dButtons,
                close: function () {
                    jQuery(this).dialog('destroy');
                }
            });
    }
    , createErrorMessage: function (message, onClose) {

        var dButtons = {};
        dButtons[$customNamespace$.Resources.close] = function () { jQuery(this).dialog('close'); };

        jQuery($customNamespace$.Widgets.Dialogs._html)
            .find('div.ui-template-dialogIcon span')
                .addClass('ui-icon-alert')
                .end()
            .find('div.ui-template-dialogMsg')
                .html(message);

        jQuery($customNamespace$.Widgets.Dialogs._html)
            .dialog({
                resizable: false,
                modal: true,
                title: $customNamespace$.Resources.siteTitle,
                buttons: dButtons,
                close: function () {
                    jQuery(this).dialog('destroy');
                    if (onClose) {
                        onClose();
                    }
                }
            });
    }
}
