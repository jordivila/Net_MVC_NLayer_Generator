(function ($) {

    jQuery.widget("jv.treeListNest", jQuery.jv.treeList, {
        options: {},
        _init: function () { },
        _create: function () {

            var w = this;
            jQuery.jv.treeList.prototype._create.call(this);

            jQuery(this.element).find('li')
                .draggable({
                    helper: 'original'
                    , addClasses: false
                    , appendTo: 'body' // By default, the helper is appended to the same container as the draggable
                    , delay: 100 // Time in milliseconds after mousedown until dragging should start
                    , revert: 'invalid'
                    , zIndex: 9999
                    , cursor: 'move'
                    , cursorAt: { top: 5, left: 5 }
                })
                .droppable({
                    accept: 'li'
                    , hoverClass: 'ui-state-highlight'
                    , addClasses: false
                    , greedy: true  // If true, will prevent event propagation on nested droppables.
                    , drop: function (e, ui) {

                        if (!w._draggableIsParent(jQuery(this).parents("li"), ui.draggable)) {
                            if (ui.draggable.parents('li:first').children('ul:first').children('li').length == 1) {
                                w.closeNode(ui.draggable.parents('li:first'));
                            }
                            else {
                                w.openNode(ui.draggable.parents('li:first'));
                            }

                            w._checkToggle(ui.draggable.parents('li:first'), false);

                            if (jQuery(this).children('ul').length == 0) {
                                jQuery(this).append("<ul></ul>");
                                w._initChildList(jQuery(this).children('ul'));
                            }

                            jQuery(this).children('ul').append(ui.draggable.css('top', '0px').css('left', '0px'));

                            w._checkToggle(jQuery(this), true);

                        }
                    }
                });
        },
        destroy: function () {
            jQuery(this.element).find('li')
                .removeClass('ui-state-active')
                .droppable('destroy')
                .draggable('destroy');

            jQuery.jv.treeList.prototype.destroy.call(this);
        }
        , _draggableIsParent: function (compareThis, compareTo) {
            for (var i = 0; i < compareThis.length; i++) {
                for (var j = 0; j < compareTo.length; j++) {
                    if (this._contains(compareTo[j], compareThis[i])) {
                        return true;
                    }
                }
            }
            return false;
        }
        , _contains: function (a, b) {
            // http://ejohn.org/blog/comparing-document-position/
            return a.contains ? a != b && a.contains(b) : !!(a.compareDocumentPosition(b) & 16);
        }
        , _checkToggle: function ($li, bIsDropping) {
            var emptyListNitems = bIsDropping ? 0 : 1;
            var $toggle = $li.children('div.ui-treeList-toggle');
            if ($li.children('ul').children('li').length == emptyListNitems) {
                $toggle.hide();
            }
            else {
                $toggle.toggleClass('ui-icon-plus', !$toggle.siblings('ul').is(':visible'))
                            .toggleClass('ui-icon-minus', $toggle.siblings('ul').is(':visible'))
                            .show();
            }
        }
    });

})(jQuery);
