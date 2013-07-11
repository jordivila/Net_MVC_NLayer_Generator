/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.widgetFormItem", jQuery.ui.widgetBase,
{
    options: {

    },
    _create: function () {
        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype._create.call(this);

        var self = this;

        jQuery(this.element)
            .find(':input')
                .change(function () {
                    jQuery(self.element).removeClass('ui-state-error').find('div.ui-widgetForm-inputError').remove();
                    self._trigger('changed', null, jQuery(this).attr('id'));
                });
    },
    _init: function () {

        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype._init.call(this);

    }
    , destroy: function () {
        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
});

jQuery.widget("ui.widgetFormSummary", jQuery.ui.widgetBase,
{
    options: {

    },
    _create: function () {
        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype._create.call(this);
    },
    _init: function () {

        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype._init.call(this);

    }
    , destroy: function () {
        // TODO: check this is really needed
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , deleteByKey: function (key) {
        jQuery(this.element).find('li[modelkey="' + key + '"]').remove();
        if (jQuery(this.element).find('ul').find('li').length == 0) {
            jQuery(this.element).hide();
        }
    }
});

