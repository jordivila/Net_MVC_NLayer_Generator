/// <reference path="$customNamespace$.A.Intellisense.js" />


jQuery.widget("ui.menuCultures", jQuery.ui.menuSite,
{
    options: {
        cultureSelected: null
    }
    , _init: function () {

        var self = this;



        jQuery.ui.menuSite.prototype._init.call(this);

        jQuery(this.element)
        //.removeClass('ui-corner-all')
        //.addClass("ui-corner-bottom")
            .find('li')
                .click(function () {

                    var culture = jQuery(this).find('div.ui-flag').attr('data-widget-value');

                    $customNamespace$.Ajax.CultureSet(culture,
                                            function () {
                                                window.location.reload();
                                            },
                                            function () {
                                                $customNamespace$.Widgets.Dialogs.createErrorMessage($customNamespace$.Resources.unExpectedError, function () { });
                                            });
                });

        jQuery(this.element)
            .find('div[data-widget-value="' + this.options.cultureSelected + '"]')
        //.find("span.value:contains('" + this.options.cultureSelected + "')")
            .parents('li:first')
                            .addClass('ui-state-active')
                            .removeClass('ui-state-default');
    }
    , _create: function () {
        jQuery.ui.menuSite.prototype._create.call(this);
    },
    destroy: function () {
        jQuery.ui.menuSite.prototype.destroy.call(this);
    }
});

