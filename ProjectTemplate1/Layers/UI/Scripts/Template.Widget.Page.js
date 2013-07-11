/// <reference path="$customNamespace$.A.Intellisense.js" />


/*******************************************************************************
                                HELPER PUBLIC METHODS
********************************************************************************/

$customNamespace$.Widgets.Page = {
    selector: null,
    allowCssClasses: false,
    cultureSelected: null,
    cultureGlobalization: null,
    cultureDatePicker: null,
    defaultTheme: null,
    InitCallback: function () { },
    Init: function () {
        jQuery(this.selector).page({
            allowCssClasses: this.allowCssClasses
            , cultureSelected: this.cultureSelected
            , cultureGlobalization: this.cultureGlobalization
            , cultureDatePicker: this.cultureDatePicker
            , defaultTheme: this.defaultTheme
            , initComplete: function () {
                $customNamespace$.Widgets.Page.InitCallback();
            }
        });
    }
};


/*******************************************************************************
                                WIDGET DEFINITION
********************************************************************************/

jQuery.widget("ui.page", jQuery.ui.widgetBase,
{
    options: {
        cultureGlobalization: null
        , cultureDatePicker: null
        , controllerSelected: null
        , defaultTheme: null
    }
    , _init: function () {

        jQuery.ui.widgetBase.prototype._init.call(this);

        this.initAjaxProgress();
        this.initResizeControl();
        this.initGlobalization();
        this.initValidate();
        this.initJQueryzer();
        this.initMenuNav();
        this.initUserOptions();
    }
    , _create: function () {
        jQuery.ui.widgetBase.prototype._create.call(this);
    }
    , destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , initMenuNav: function () {
        jQuery(this.element).find('div[data-widget="menuNav"]:first').menuNav({ allowCollapse: true, isCollapsed: true, allowClose: false });
    }
    , initUserOptions: function () {

        var self = this;

        jQuery(this.element).find('div[data-widget="userOptions"]:first').userOptions({
            complete: function () {
                self._trigger('initComplete', null, null);
            }
        });
    }
    , initAjaxProgress: function () {
        $customNamespace$.Widgets.AjaxProgress().Create();
    }
    , initFooter: function () {
        jQuery('div.ui-siteFooter').widgetBase();
    }
    , initGlobalization: function () {
        /* Globalization Initializaer */
        Globalize.culture(this.options.cultureGlobalization);
        //jQuery('div.sample').append('<span>' + Globalize.format(3899.888, "c") + '</span><br/>');
        //jQuery('div.sample').append('<span>' + Globalize.format(new Date(2011, 12, 25), "D") + '</span><br/>');
        //jQuery('div.sample').append('<span>' + Globalize.format(45678, "n0") + '</span><br/>');
        jQuery.datepicker.setDefaults(jQuery.datepicker.regional[this.options.cultureDatePicker]);
    }
    , initValidate: function () {

        jQuery.validator.setDefaults({
            debug: false
            , errorClass: "ui-state-error"
        });

        jQuery.validator.methods.number = function (value, element) {
            if (Globalize.parseFloat(value)) {
                return true;
            }
            return false;
        }
    }
    , initResizeControl: function () {
        var resizing = function () {

            var availableHeight = jQuery(window).height() -
                                                (jQuery('div.ui-site-header:first').height() +
                                                    jQuery('div.ui-siteFooter:first').height() +
                                                    jQuery('div.ui-siteMenu-TopNav:first').height() +
                                                    jQuery('div.ui-cultureSwitcher:first').height() +
                                                    jQuery('div.ui-themeSwitcher:first').height()
                                                );

            jQuery('div.ui-siteContent:first').animate({ height: availableHeight - 50 }, "slow");
        }
        //        jQuery(window).resize(function () {
        //            resizing();
        //        });
        //        resizing();
    }
    , initJQueryzer: function () {

        $customNamespace$.Widgets.jQueryzer(this.element);

        jQuery(this.element)
                    .find(':input:not([type=hidden]):not([type=checkbox]):not([type=radio]), textarea')
                        .addClass('ui-widget-content ui-corner-all')
                        .focus(function () { jQuery(this).addClass('ui-state-focus'); })
                        .blur(function () { jQuery(this).removeClass('ui-state-focus'); })
                    .end()
                    .find('fieldset')
                        .addClass('ui-widget-content  ui-corner-all')
                    .end()
                    .find('hr')
                        .addClass('ui-widget-content');
    }
});
