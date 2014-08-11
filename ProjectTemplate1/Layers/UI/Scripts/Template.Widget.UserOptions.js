/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.userOptions", jQuery.ui.widgetBase,
{
    options: {

    }
    , _create: function () {
        jQuery.ui.widgetBase.prototype._create.call(this);
    }
    , _init: function () {

        jQuery.ui.widgetBase.prototype._init.call(this);

        this.initMenuNav();
        this.updateUserLastActivity();
    }
    , destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , updateUserLastActivity: function () {
        var self = this;

        $customNamespace$.Ajax.UserUpdateLastActivity(
                            function (data, textStatus, jqXHR) {
                                jQuery(self.element).append(data);
                                $customNamespace$.Widgets.jQueryzer(self.element);
                            }
                            , function (jqXHR, textStatus, errorThrown) {

                                jQuery(self.element)
                                    .append("<div></div><div class='ui-carriageReturn'></div>")
                                    .find("div:first")
                                    .html($customNamespace$.Resources.unExpectedError);

                                $customNamespace$.Widgets.DialogInline.Create(jQuery(self.element).find("div:first"), $customNamespace$.Widgets.DialogInline.MsgTypes.Error);

                                jQuery(self.element)
                                    .find("div.ui-message div:first")
                                    .removeClass("ui-corner-all")
                                    .addClass("ui-corner-right");
                            }
                            , function () {
                                self._trigger('complete', null, null);
                            });
    }
    , initMenuNav: function () {

        //TODO: load async Menu based on user identity

        var $panelMenu = jQuery('#panelMenu');
        var $panelContent = jQuery('#panelContent');

        $panelMenu.navMenu();

        jQuery('#menuToggle').click(function () {

            $panelMenu.show('slide', function () {
                if (jQuery(this).is(':visible')) {

                    jQuery(document).bind("click", function (e) {

                        var menuClicked = jQuery(e.target).parents($panelMenu.selector).length > 0;

                        if (!menuClicked) {

                            $panelMenu.hide('slide', function () {
                                $panelMenu.navMenu('collapseAll');
                            });

                            jQuery(document).unbind("click");
                        }
                    });
                }
            });
        });
    }
});