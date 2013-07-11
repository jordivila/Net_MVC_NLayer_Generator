/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.widgetBoolean", jQuery.ui.widgetBase,
{
    options: {
        isNullable: false
        , icons: []
        , values: []
    },
    _create: function () {

        jQuery.ui.widgetBase.prototype._create.call(this);

        if (jQuery(this.element).attr('data-widget-nullable') != undefined) {
            this.options.isNullable = jQuery(this.element).attr('data-widget-nullable').toString().toLowerCase() == "true";
        }

        this.options.icons = ['ui-icon-check', 'ui-icon-closethick'];
        this.options.values = [true, false];

        if (this.options.isNullable) {
            this.options.icons.push('ui-icon-help');
            this.options.values.push(null);
        }

    },
    _init: function () {

        jQuery.ui.widgetBase.prototype._init.call(this);

        var self = this;
        var $el = jQuery(this.element);
        var icons = null;
        var values = null;

        jQuery(this.element)
                            .find('button')
                                .click(function () {

                                    var nextIndex = self._getNextIndex();
                                    var nextClassName = self.options.icons[nextIndex];

                                    console.log(nextClassName);

                                    jQuery(this)
                                        .find('span')
                                        .removeClass(self.options.icons.join(" "))
                                        .addClass(nextClassName);

                                    switch (nextClassName) {
                                        case 'ui-icon-check':
                                            $el.find(':checkbox').attr('checked', 'checked');
                                            if (self.options.isNullable) { $el.find('input[type="hidden"]').val(''); }
                                            break;
                                        case 'ui-icon-closethick':
                                            $el.find(':checkbox').removeAttr('checked');
                                            if (self.options.isNullable) { $el.find('input[type="hidden"]').val('false'); }
                                            break;
                                        case 'ui-icon-help':
                                            $el.find(':checkbox').removeAttr('checked');
                                            if (self.options.isNullable) { $el.find('input[type="hidden"]').val(''); }
                                            break;
                                    }
                                });

    }
    , destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , _getCurrentIndex: function () {

        //var $el = jQuery(this.element);
        var currentValue = null;
        var result = 0;

        if (this.options.isNullable) {
            if (jQuery(this.element).find('input[type="checkbox"]').attr('checked') == 'checked') {
                currentValue = true;
            }
            else {
                if (jQuery(this.element).find('input[type="hidden"]').val() == "false") {
                    currentValue = false;
                }
            }
        }
        else {
            currentValue = jQuery(this.element).find('input[type="checkbox"]').attr('checked') == 'checked';
        }

        for (var i = 0; i < this.options.values.length; i++) {
            if (currentValue == this.options.values[i]) {
                result = i;
                break;
            }
        }


        return result;
    }
    , _getNextIndex: function () {
        var i = this._getCurrentIndex();
        var result = (i + 1) >= (this.options.values.length) ? 0 : (i + 1);
        return result;
    }

});