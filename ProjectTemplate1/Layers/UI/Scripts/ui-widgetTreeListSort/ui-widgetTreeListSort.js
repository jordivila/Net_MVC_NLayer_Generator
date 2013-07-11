/*
* jQuery widget to create themable Tree Lists
* 
* 
*
*/

(function($) {
    jQuery.widget("jv.treeListSortable", jQuery.jv.treeList, {
        options: {}
        , _create: function() {

            var w = this;
            jQuery.jv.treeList.prototype._create.call(this);

            jQuery(this.element).sortable({
                items: 'li',
                placeholder: 'ui-sortable-placeholder',
                connectWith: 'ul',
                //grid: [5, 5],
                //tolerance: 'pointer',
                //distance: 1,
                cursor: 'move',
                revert: true,
                dropOnEmpty: true,
                activate: function(e, ui) {
                    if (ui.placeholder.siblings('li').length == 1) {
                        ui.placeholder
                        // add an empty item because "dropOnEmpty" looks like is not working at all
                        // and items cannot be added
                        .parents('ul:first').append('<li class="ui-treeList-itemEmpty"></li>')
                        .end()
                        .parents('li:first').children().remove('div.ui-treeList-toggle');
                    }
                },
                deactivate: function(e, ui) {
                    if (ui.item.siblings('li').length == 1) {
                        if (ui.item.parents('li:first').children('div.ui-treeList-toggle').length == 0) {
                            ui.item.parents('li:first')
                            .children('ul').before('<div class="ui-treeList-toggle ui-widget ui-widget-content ui-corner-all ui-icon ui-icon-minus"></div>')
                            .children().remove('li.ui-treeList-itemEmpty');
                        }
                    }
                }
            });
        },
        destroy: function() {

            jQuery(this.element).find('li')
                .removeClass('ui-state-active')
                .sortable('destroy');

            jQuery.jv.treeList.prototype.destroy.call(this);
        }
    });


})(jQuery);
