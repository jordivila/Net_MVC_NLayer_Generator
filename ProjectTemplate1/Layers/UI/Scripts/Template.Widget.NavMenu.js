
(function ($) {
	jQuery.widget("ui.navMenu", {
		options: { selectable: true },
		_create: function () {

			var w = this;
			w._initItem(jQuery(this.element).find("li"));
			w._initChildList(jQuery(this.element).find("ul"));

			var $lisOpen = jQuery(this.element).find("li[class*='ui-treeList-open']");
			w.openNode($lisOpen);
			w.openNode($lisOpen.parents('li'));

			jQuery(this.element)
                .addClass('ui-treeList ui-widget-content ui-corner-all')
                .bind('click', function (e) {
                	var $t = jQuery(e.target);

                	var $node = null;

                	if ($t.hasClass('ui-treeList-toggle')) {
                		$node = $t.parents('li.ui-treeList-item:first');
                	}
                	else {
                		if ($t.hasClass('ui-treeList-item')) {
                			$node = $t;
                		}
                		else {
                			if ($t.hasClass('ui-treeList-link')) {
                				$node = $t.parents('li.ui-treeList-item:first');
                			}
                		}
                	}


                	if ($node != null)
                	{
                		if ($node.find('ul:first').length > 0)
                		{
                			var b = $node.find('ul:first').is(':visible');
                			if (b) w.closeNode($node);
                			else w.openNode($node);
                		}
                		else
                		{
                			if ($node.find('a:first').length > 0)
                			{
                				window.location.href = $node.find('a:first').attr('href');
                			}
                		}
                	}

                })
                .disableSelection();
		},
		destroy: function () {

			jQuery(this.element)
            .unbind('click')
            .removeClass('ui-treeList ui-widget-content ui-corner-all')
            .find('li')
                .unbind('mouseenter mouseleave')
                .removeClass('ui-treeList-item ui-widget-content ui-corner-all ui-state-default ui-state-active ui-state-hover')
                .children('div.ui-treeList-toggle')
                    .remove()
                .end()
                .find('ul')
                    .unbind('mouseenter mouseleave')
                    .removeClass('ui-treeList-childs');

			jQuery.Widget.prototype.destroy.call(this);
		},
		_initItem: function ($lis) {
			$lis.addClass('ui-treeList-item ui-widget-content ui-corner-all ui-state-default')
                  .hover(
                        function () { jQuery(this).addClass('ui-state-hover').parents('li').removeClass('ui-state-hover');; return false; }
                        , function () { jQuery(this).removeClass('ui-state-hover'); return false; }
                    )
				.find('a:first')
					.addClass('ui-treeList-link');
		},
		_initChildList: function ($uls) {
			$uls.addClass('ui-treeList-childs')
                    .hide()
                    .before('<div class="ui-treeList-toggle ui-icon ui-icon-triangle-1-s"></div>');
		},
		openNode: function ($lisOpen) {
			if ($lisOpen) {
				$lisOpen.children('ul')
                                .show()
                                .siblings('div.ui-treeList-toggle')
                                    .removeClass('ui-icon ui-icon-triangle-1-s')
                                    .addClass('ui-icon ui-icon-triangle-1-n')
                                    .end()
                                .end()
                                .find('ul:has(li)')
                                    .parents('li')
                                        .removeClass('ui-state-default');
			}
		}
        ,
		closeNode: function ($lisClose) {
			if ($lisClose) {
				$lisClose.addClass('ui-state-default')
                                .children('ul')
                                .hide()
                                .siblings('div.ui-treeList-toggle').removeClass('ui-icon-triangle-1-n').addClass('ui-icon ui-icon-triangle-1-s');
			}
		}
        , selected: function ($lis) {
        	if ($lis) {
        		jQuery(this.element).find('li').removeClass('ui-state-active');
        		$lis.addClass('ui-state-active');
        		this._trigger('onSelect');
        	}
        	else {
        		return jQuery(this.element).find('li.ui-state-active');
        	}
        }
	});

})(jQuery);
