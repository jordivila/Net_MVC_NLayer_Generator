/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.menuNav", jQuery.ui.menuSite,
    {
        options: {
            controllerSelected: ''
            , actionSelected: ''
            , allowCollapse: true
        },
        _init: function () {
            jQuery.ui.menuSite.prototype._init.call(this);
        }
        , _create: function () {

            jQuery.ui.menuSite.prototype._create.call(this)

            this.options.controllerSelected = jQuery(this.element).attr('data-widget-controllerSelected');
            this.options.actionSelected = jQuery(this.element).attr('data-widget-controllerAction');

            if (this.options.controllerSelected != "") {
                jQuery(this.element)
                .find("li[data-action*='" + this.options.controllerSelected + "'][data-action*='" + this.options.actionSelected + "']")
                            .each(function () {
                                if (jQuery(this).parents('li:last').length > 0) {
                                    jQuery(this).parents('li:last').addClass('ui-corner-all ui-state-active');
                                }
                                else {
                                    jQuery(this).addClass('ui-corner-all ui-state-active');
                                }
                            });
            }
        },
        destroy: function () {
            jQuery.ui.menuSite.prototype.destroy.call(this);
        }
    });
