


window.$wMsgGlobal =
{
    // template used to decorate message content
    htmlTemplate: '<div class="ui-widget ui-widgetMsgWrapper ui-corner-all"><p class="widgetMsgStyleContent"><span class="widgetIcon ui-icon"></span></p></div>'
    // template used to decorate Yes/No buttons in case widget's type is enumMsgType.ConfirmYesNo
    //, yesNoButtonsTmpl: '<div class="widgetYesNoButtons"><div class="ui-message-yes ui-button ui-state-default ui-corner-all"><span class="ui-icon ui-icon-check"></span>Ok</div><div class="ui-message-no ui-button ui-state-default ui-corner-all"><span class="ui-icon ui-icon-close"></span>Cancel</div></div>'
    , yesNoButtonsTmpl: '<div class="widgetYesNoButtons"><button type="button">Ok</button><button type="button">Cancel</button></div>'
}

//enumeration message types
function enumMsgType() { }
enumMsgType.Success = 0;
enumMsgType.Warning = 1;
enumMsgType.Error = 2;
enumMsgType.ConfirmYesNo = 3;
enumMsgType.GetEnumTypeByNum = function (num) {
    switch (num) {
        case 1:
            return enumMsgType.Warning;
            break;
        case 2:
            return enumMsgType.Error;
            break;
        case 3:
            return enumMsgType.ConfirmYesNo;
            break;
        default:
            return enumMsgType.Success;
            break;
    }
};


(function ($) {
    jQuery.widget('ui.widgetMsg', {
        options: {
            msgType: null
            , allowClose: null
            , autoHide: null
            , onAccept: null
            , onCancel: null
        }
        , _create: function () {

            var $el = jQuery(this.element);
            var o = this.options;

            if (o.msgType == null) {
                o.msgType = enumMsgType.GetEnumTypeByNum(parseInt($el.attr('data-widget-msgType')));
            }
            if (o.msgType == null) {
                o.msgType = enumMsgType.Success;
            }

            if (o.allowClose == null) {
                o.allowClose = $el.attr('data-widget-allowClose') == 'true';
            }
            if (o.allowClose == null) {
                o.allowClose = false;
            }

            if (o.autoHide == null) {
                o.autoHide = $el.attr('data-widget-autoHide') == 'true';
            }
            if (o.autoHide == null) {
                o.autoHide = false;
            }

            if ($el.attr('data-widget-onAccept') != '') {
                o.onAccept = function () { eval($el.attr('data-widget-onAccept')); };
            }

            if ($el.attr('data-widget-onCancel') != '') {
                o.onCancel = function () { eval($el.attr('data-widget-onCancel')); };
            }

        },
        _init: function () {

            var w = this;
            var $el = jQuery(this.element);
            var o = this.options;

            // wrap element contents into window feedback message template
            var elHtml = $el.html();
            $el.addClass('ui-message').html($wMsgGlobal.htmlTemplate);
            $el.find('.widgetMsgStyleContent').append(elHtml);

            //store jquery results for later use
            var $wDiv = $el.find('div.ui-widget:first');
            var $wIcon = $el.find('span.widgetIcon:first');

            if (!isNaN(o.msgType)) {
                o.msgType = enumMsgType.GetEnumTypeByNum(o.msgType);
            }

            switch (o.msgType) {
                case (enumMsgType.ConfirmYesNo):
                    $wDiv.addClass('ui-state-default');
                    $wIcon.addClass('ui-icon-help');
                    //append buttons to feedback message
                    jQuery($wMsgGlobal.yesNoButtonsTmpl).appendTo($el.find('div.ui-widget:first'));

                    $wDiv
                        .find('button:first').button({
                            icons: {
                                primary: 'ui-icon-check'
                            }
                        })
                        .click(function () {
                            if (o.onAccept != null) {
                                o.onAccept(this, true);
                            }
                            else {
                                w._trigger('answered', null, { confirm: true });
                            }
                        })
                        .end()
                        .find('button:last').button({
                            icons: {
                                primary: 'ui-icon-close'
                            }
                        })
                        .click(function () {
                            if (o.onCancel != null) {
                                o.onCancel(this, false);
                            }
                            else {
                                w._trigger('answered', null, { confirm: false });
                            }
                        });
                    break;
                case (enumMsgType.Warning):
                    $wDiv.addClass('ui-state-highlight');
                    $wIcon.addClass('ui-icon-info');
                    break;
                case (enumMsgType.Success):
                    $wDiv.addClass('ui-state-active');
                    $wIcon.addClass('ui-icon-check');
                    break;
                case (enumMsgType.Error):
                    $wDiv.addClass('ui-state-error');
                    $wIcon.addClass('ui-icon-alert');
                    $el.find('p.widgetMsgStyleContent:first').addClass('ui-state-error-text');
                    break;
                default:
                    break;
            }

            if (o.allowClose == true) {
                this.allowClose();
            }

            if (o.autoHide == true) {
                $el.delay(1000).fadeOut(2000);
            }

        },
        destroy: function () {
            //release events
            if (this.options.msgType == enumMsgType.ConfirmYesNo) {
                jQuery(this.element).find('div.ui-message-yes, div.ui-message-no').unbind('click').unbind('mouseenter mouseleave');
            }
            jQuery.Widget.prototype.destroy.apply(this, arguments);
        },
        allowClose: function () {
            var self = this;
            jQuery(this.element)
                .find('div.ui-widgetMsgWrapper:first')
                    .append('<div class="ui-widget-close ui-corner-all ui-icon ui-icon-close"></div>')
                .end()
                .find('div.ui-widget-close')
                    .click(function () {
                        jQuery(self.element).toggle();
                    })
                    .show();
        }
    });

    jQuery.extend(jQuery.ui.widgetMsg, {
        defaults: {}
    });

})(jQuery);