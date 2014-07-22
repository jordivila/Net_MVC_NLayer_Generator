jQuery.widget("ui.menuThemes", jQuery.ui.menuSite,
{
    options: {
        defaultTheme: ''
    }
    , _create: function () {

        jQuery.ui.menuSite.prototype._create.call(this);

    }
    , _init: function () {

        jQuery.ui.menuSite.prototype._init.call(this);

        var self = this;

        jQuery(this.element)
            .find('li')
                .click(function () {
                    var value = jQuery(this).attr('data-widget-value');
                    self.setTheme(value);
                });
    },
    destroy: function () {
        jQuery.ui.menuSite.prototype.destroy.call(this);
    }
    , setTheme: function (value) {

        window.location.href = "/Home/ThemeSet/" + value;
    }
});
