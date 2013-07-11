/// <reference path="$customNamespace$.A.Intellisense.js" />

jQuery.widget("ui.menuSite", jQuery.ui.widgetBase,
        {
            options: {
                autoCloseChilds: true
            },
            _create: function () {
                jQuery.ui.widgetBase.prototype._create.call(this)
            },
            _init: function () {

                var self = this;

                jQuery(this.element)
                    .addClass('ui-widget-content ui-corner-all ui-state-default')
                    .find('li')
                        .hover(
                            function () {
                                jQuery(this).addClass('ui-state-hover');
                            },
                            function () {
                                jQuery(this).removeClass('ui-state-hover');
                            }
                        )
                        .click(function () {

                            if (self.options.autoCloseChilds) {
                                jQuery(this).find("ul:first").toggle(1, function () {
                                    if (jQuery(this).is(':visible')) {
                                        jQuery(document).one("click", function (e) {
                                            jQuery(self.element).find('ul:first').find('ul').hide();
                                        });
                                    }
                                });
                            }

                            var url = jQuery(this).attr('data-action');
                            if (url != undefined) { window.location.href = url; }

                            var action = jQuery(this).attr('data-clientAction');
                            setTimeout("try{ " + action + "}catch(e){}", 0);

                        })
                        .find('ul')
                            .addClass('ui-widget-content ui-corner-all')
                            .find('li:first')
                                .addClass('ui-corner-top')
                                .end()
                            .find('li:last')
                                .addClass('ui-corner-bottom')
                                .end()
                        .end()
                    .end()
                    .find('li:first')
                        .addClass('ui-corner-all')
                        .siblings()
                            .each(function () {
                                jQuery(this).addClass('ui-corner-all');
                            })
                        .end()
                    .end();

                jQuery.ui.widgetBase.prototype._init.call(this);
            }
            ,
            destroy: function () {
                jQuery.ui.widgetBase.prototype.destroy.call(this);
            }
        });
