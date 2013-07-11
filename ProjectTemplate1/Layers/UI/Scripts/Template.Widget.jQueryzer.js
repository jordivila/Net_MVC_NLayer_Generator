/// <reference path="$customNamespace$.A.Intellisense.js" />


/*******************************************************************************
                            HELPER PUBLIC METHODS
********************************************************************************/


$customNamespace$.Widgets.jQueryzer = function (selector) {
    jQuery(selector).widgetJqueryzer();
}


/*******************************************************************************
                            WIDGET DEFINITION
********************************************************************************/

jQuery.widget('ui.widgetJqueryzer', jQuery.ui.widgetBase,
{
    options: {

    },
    _init: function () {
        jQuery.ui.widgetBase.prototype._init.call(this);
    },
    _create: function () {

        jQuery.ui.widgetBase.prototype._create.call(this);

        var $list = jQuery(this.element).find(
                                    'div[data-widget],' +
                                    'table[data-widget],' +
                                    'input[data-widget],' +
                                    'ul[data-widget],' +
                                    'button[data-widget]');

        for (var i = 0; i < $list.length; i++) {

            var $listItem = jQuery($list[i]);
            var widgetName = $listItem.attr('data-widget');

            this.item($listItem, widgetName);

        }
    }
    , destroy: function () {
        jQuery.ui.widgetBase.prototype.destroy.call(this);
    }
    , item: function ($listItem, widgetName) {
        switch (widgetName) {
            case 'widgetBase':
                $listItem.widgetBase({
                    allowCollapse: parseBoolean($listItem.attr('data-widget-allowCollapse')),
                    allowClose: parseBoolean($listItem.attr('data-widget-allowClose')),
                    isCollapsed: parseBoolean($listItem.attr('data-widget-isCollapsed')),
                    onCollapsed: function (e, isVisible) {
                        try {
                            eval($listItem.attr('data-widget-jsOnCollapse'));
                        }
                        catch (e) {

                        }
                    }
                });
                break;
            case 'widgetModelItem':
                $listItem.widgetFormItem({
                    changed: function (e, id) {
                        $listItem.parents('form:first').find('div.ui-widgetForm-ValidationSummary').widgetFormSummary('deleteByKey', id);
                    }
                });
                break;
            case 'widgetGrid':
                $listItem.widgetGrid();
                break;
            case 'ui-widgetBoolean':
                $listItem.widgetBoolean();
                break;
            case 'dateSelector':
                $listItem.dateSelector({
                    text: $customNamespace$.Resources.clickToPickDate
                            , value: Globalize.parseDate($listItem.attr('data-value'))
                });
                break;
            case 'widgetMsg':
                $customNamespace$.Widgets.DialogInline.Create($listItem);
                break;
            case 'widgetButton':
                $listItem.widgetButton();
                break;
            case 'widgetFormSummary':
                $listItem.widgetFormSummary();
                break;
            default:
                //console.log($listItem);
                break;
        }
    }
});

