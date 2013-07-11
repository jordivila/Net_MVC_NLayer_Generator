/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.menuThemes", jQuery.ui.menuSite,
{
    options: {
        defaultTheme: ''
    }
    , _create: function () {

        jQuery.ui.menuSite.prototype._create.call(this);

    }
    , _init: function () {

        jQuery.ui.menuSite.prototype._init.call(this);

        var self = this;

        jQuery(this.element)
            .find('li')
                .click(function () {
                    var value = jQuery(this).attr('data-widget-value');
                    self.setTheme(value);
                });
    },
    destroy: function () {
        jQuery.ui.menuSite.prototype.destroy.call(this);
    }
    , setTheme: function (value) {

        var self = this;



        $customNamespace$.Ajax.ThemeSet(value,
                            function (result) {

                                jQuery.ui.menuSite.prototype.addCss.call(this, '<link href="' + result.Data + '" rel="stylesheet" type="text/css" />');

                                jQuery(self.element)
                                            .find("li")
                                                .removeClass('ui-state-active')
                                                .end()
                                            .find("li[data-widget-value='" + value + "']")
                                                    .addClass('ui-state-active');
                            },
                            function () {
                                $customNamespace$.Widgets.Dialogs.createErrorMessage($customNamespace$.Resources.unExpectedError, function () { });
                            });
    }
});
