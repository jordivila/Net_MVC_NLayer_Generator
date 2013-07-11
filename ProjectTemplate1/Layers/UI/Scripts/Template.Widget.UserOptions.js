/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.userOptions", jQuery.ui.widgetBase,
{
    options: {

    },
    _create: function () {
        jQuery.ui.widgetBase.prototype._create.call(this);
    },
    _init: function () {

        jQuery.ui.widgetBase.prototype._init.call(this);

        var self = this;

        $customNamespace$.Ajax.UserBar(
                            function (data, textStatus, jqXHR) {
                                jQuery(self.element).html(data);
                                $customNamespace$.Widgets.jQueryzer(self.element);
                                self._initTheme();
                                self._initMenuCultures();
                                self._initUserMenu();
                                self._preparePreferences();
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
    },
    destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , _initMenuCultures: function () {
        jQuery(this.element)
            .find('div[data-widget="menuCultures"]:first')
                .each(function (index, ui) {
                    jQuery(this).menuCultures({ cultureSelected: jQuery(this).attr('data-widget-cultureSelected') });
                });
    }
    , _initTheme: function () {
        jQuery(this.element)
            .find('div[data-widget="menuThemes"]:first')
            .each(function (index, ui) {
                jQuery(ui).menuThemes({
                    defaultTheme: jQuery(ui).attr('data-widget-defaultTheme'),
                    cdnBaseUrl: jQuery(ui).attr('data-widget-cdnBaseUrl')
                });
            });
    }
    , _initUserMenu: function () {
        jQuery(this.element)
            .find('div[data-widget="menuSite"]:first')
                .menuSite({
                    autoCloseChilds: false
                });
    }
    , _preparePreferences: function () {

        var $cultures = jQuery(this.element).find('div[data-widget="menuCultures"]');
        var $themes = jQuery(this.element).find('div[data-widget="menuThemes"]');
        var $menuUser = jQuery(this.element).find('div[data-widget="menuSite"]');
        var hideAll = function () {
            $cultures.hide();
            $themes.hide();
            $menuUser = hide();
        };

        var showMenu = function ($menuElement) {
            jQuery($menuElement)
                .show('fast'
                    , function () {
                        jQuery(document).one('click', null, null, function () {
                            $menuElement.hide();
                        });
                    });
        };

        jQuery(this.element)
                .find('div.ui-login-UserInfo:first')
                    .find('button')
                        .click(function () {
                            showMenu($menuUser);
                        })
                    .end()
                .end()
                .find('div.ui-sitePreferences-Info')
                    .find('button#btnLanguageSwitcher')
                        .click(function () {
                            showMenu($cultures);
                        })
                    .end()
                    .find('button#btnThemeSwitcher')
                        .click(function () {
                            showMenu($themes);
                        });
    }
});
