/*
* jQuery widget to create themable google maps utilities
* 
* 
*
*/

(function($) {


    var uigMapDirectionsTxt = {

        uiTitleBox: "Get Directions"
        , uiFrom: "From"
        , uiTo: "To"
        , uiByCar: "By Car"
        , uiByWalk: "Walking"
        , uiBicycling: "Bicycling"
        , uiUnitImperial: "Imperial"
        , uiUnitMetric: "Metric"
        , uiPrint: "Print"
        , uiClear: "Clear"
        , uiGo: "Go"
        , uiExplain: 'Use this service for computing directions between two places by specifying location of origin, destination, preferred unit system and a type of routing.'
    };

    jQuery.widget("jv.gMapDirections", jQuery.ui.widgetBase, {
        options: {
            uiTemplate: function() {
                return '<div class="ui-gMapDirections-dragHandler ui-widget-header ui-corner-all">' + uigMapDirectionsTxt.uiTitleBox +
                            '<div class="ui-gMapDirections-closeHandler ui-state-default ui-corner-all">' +
                                '<span class="ui-icon ui-icon-circle-close"></span>' +
                            '</div>' + '</div>' +
                            '<div class="ui-gMapDirections-inputs ui-widget-content ui-corner-all ui-state-default">' +
                              '<ul>' +
                                '<li class="ui-gMapDirections-label">' + uigMapDirectionsTxt.uiFrom + ':</li>' +
                                '<li> ' +
                                    '<input class="ui-gMapDirections-from" type="text" value="" ></input>' +
                                    '<select class="ui-gMapDirections-travelMode">' +
                                          '<option value="driving" selected="selected">' + uigMapDirectionsTxt.uiByCar + '</option>' +
                                          '<option value="walking">' + uigMapDirectionsTxt.uiByWalk + '</option>' +
                                          '<option value="bicyling">' + uigMapDirectionsTxt.uiBicycling + '</option>' +
                                    '</select>' +
                                '</li>' +
                                '<li class="ui-gMapDirections-label">' + uigMapDirectionsTxt.uiTo + ':</li>' +
                                '<li>' +
                                    '<input class="ui-gMapDirections-to" type="text" value=""/>' +
                                    '<select class="ui-gMapDirections-unit">' +
                                      '<option value="imperial">' + uigMapDirectionsTxt.uiUnitImperial + '</option>' +
                                      '<option value="metric" selected="selected">' + uigMapDirectionsTxt.uiUnitMetric + '</option>' +
                                    '</select>' +
                                '</li>' +
                              '</ul>' +
                            '</div>' +
                            '<div class="ui-gMapDirections-buttons">' +
                                '<button type="button" class="ui-widget-print">' + uigMapDirectionsTxt.uiPrint + '</button>' +
                                '<button type="button" class="ui-widget-clear">' + uigMapDirectionsTxt.uiClear + '</button>' +
                                '<button type="button" class="ui-widget-dirGo">' + uigMapDirectionsTxt.uiGo + '</button>' +
                            '</div>' +
                            '<div class="ui-gMapDirections-info ui-widget-content ui-corner-all ui-state-default" >' + uigMapDirectionsTxt.uiExplain + '</div>';
            }
            , map: null
            , draggable: true
            , resizable: true
            , opacity: 0.8
            //, provideTripAlternatives: true
            , width: 300
            , height: 250
        }
        , _create: function() {

            var w = this;
            var $el = jQuery(this.element);
            var o = w.options;

            o.dirService = new google.maps.DirectionsService();
            o.dirRenderer = new google.maps.DirectionsRenderer();

            $el
                .append(o.uiTemplate())
                .css('width', o.width)
                .css('height', o.height)
                .addClass('ui-gMapDirections ui-widget-content ui-corner-all')
                .fadeTo(1, o.opacity)
                    .find('button.ui-widget-dirGo')
                        .button({
                            icons: {
                                primary: 'ui-icon-transfer-e-w'
                            }
                        })
                        .click(function(e) {
                            w._getRoutes($el, w, o);
                        })
                    .end()
                    .find('button.ui-widget-clear')
                        .button()
                        .hide()
                        .click(function(e) {
                            $el
                                .find('input[type=text]')
                                    .val('')
                                .end()
                                .find('div.ui-gMapDirections-info')
                                    .html('');

                            o.dirRenderer.setMap(null);

                            jQuery(this).hide().siblings('button.ui-widget-print').hide();
                        })
                    .end()
                    .find('button.ui-widget-print')
                        .button({
                            icons: {
                                primary: 'ui-icon-print'
                            }
                        })
                        .hide()
                        .click(function(e) {
                            try {
                                if ($el.find('iframe.ui-gMap-iPrint').length == 0) {
                                    $el.find('div.ui-gMapDirections-buttons').append('<iframe class="ui-gMap-iPrint"></iframe>');
                                }
                                var $frame = $el.find('iframe.ui-gMap-iPrint');
                                var oDoc = ($frame[0].contentWindow || $frame[0].contentDocument);
                                if (oDoc.document) oDoc = oDoc.document;
                                oDoc.write("<html><head><title>" + window.location.href + "</title></head><body onload='this.focus(); this.print();'>" + $el.find('div.ui-gMapDirections-info').html() + "</body></html>");
                                oDoc.close();
                            }
                            catch (e) {
                                // just do nothing
                            }
                        })
                    .end()
                    .find('div.ui-gMapDirections-closeHandler')
                        .click(function(e) {
                            $el.hide();
                            w._trigger('close');
                        })
                        .hover(function() { jQuery(this).addClass('ui-state-hover'); },
                                    function() { jQuery(this).removeClass('ui-state-hover'); });


            w._draggable(w, $el, o);
            w._resizable(w, $el, o);
            w._calcSize($el, o.width, o.height);

        },
        _setOption: function(key, value) {

            jQuery.Widget.prototype._setOption.apply(this, arguments);

            var $el = jQuery(this.element);
            var o = this.options;
            switch (key) {
                case "opacity":
                    $el.fadeTo(1, o.opacity);
                    break;
                case "resizable":
                    this._resizable(this, $el, o);
                    break;
                case "draggable":
                    this._draggable(this, $el, o);
                    break;
                case "height":
                    $el.css("height", o.height + "px");
                    this._calcSize($el, o.width, o.height);
                    break;
                case "width":
                    $el.css("width", o.width + "px");
                    this._calcSize($el, o.width, o.height);
                    break;
            }
        }
        ,
        _draggable: function(w, $el, o) {
            if (o.draggable == true) {
                $el
                    .draggable({
                        handle: 'div.ui-gMapDirections-dragHandler'
                    })
                    .find('div.ui-gMapDirections-dragHandler')
                    .css('cursor', 'move');
            }
            else {
                $el.draggable('destroy').find('div.ui-gMapDirections-dragHandler').css('cursor', 'default');
            }
        }
        ,
        _resizable: function(w, $el, o) {
            if (o.resizable == true) {
                $el.resizable({
                    minHeight: o.height
                        , minWidth: o.width
                        , resize: function(e, ui) {
                            w._calcSize($el, ui.size.width, ui.size.height);
                        }
                        , stop: function(e, ui) {
                            w._calcSize($el, ui.size.width, ui.size.height);
                        }
                });
            }
            else {
                $el.resizable('destroy');
            }
        }
         ,
        _calcSize: function($el, width, height) {
            $el.find('div.ui-gMapDirections-info:first').css('height', height - 160)
                    .end()
                    .find('div.ui-gMapDirections-inputs input[type=text], div.ui-gMapDirections-inputs select').css('width', (width / 2) - 50);
        },
        _getRoutes: function($el, w, o) {

            var dirRequest = {
                origin: $el.find('input.ui-gMapDirections-from:first').val(),
                destination: $el.find('input.ui-gMapDirections-to:first').val(),
                travelMode: w._getTravelMode($el),
                unitSystem: w._getUnit($el)
                // provideTripAlternatives: o.provideTripAlternatives
            };

            o.dirService.route(dirRequest, function(dirResult, dirStatus) {
                $el.find('div.ui-gMapDirections-info').html('');
                o.dirRenderer.setMap(null);
                if (dirStatus != google.maps.DirectionsStatus.OK) {
                    $el
                        .find('div.ui-gMapDirections-info')
                            .append('<div class="errMsg">Failed:' + dirStatus + '</div>');
                    return;
                }
                o.dirRenderer.setPanel($el.find('div.ui-gMapDirections-info')[0]);
                o.dirRenderer.setMap(o.map);
                o.dirRenderer.setDirections(dirResult);
                $el.find('button.ui-widget-clear, button.ui-widget-print').show();
            });

        },
        _getTravelMode: function($el) {
            switch ($el.find('select.ui-gMapDirections-travelMode:first').val()) {
                case 'driving':
                    return google.maps.DirectionsTravelMode.DRIVING;
                    break;
                case 'walking':
                    return google.maps.DirectionsTravelMode.WALKING;
                    break;
                case 'bicyling':
                    return google.maps.DirectionsTravelMode.BICYLING;
                    break;
                default:
                    return google.maps.DirectionsTravelMode.DRIVING;
                    break;
            }
        },
        _getUnit: function($el) {
            return $el.find('select.ui-gMapDirections-unit:first').val() == 'metric' ?
                google.maps.DirectionsUnitSystem.METRIC :
                google.maps.DirectionsUnitSystem.IMPERIAL;
        },
        destroy: function() {

            jQuery(this.element)
                .removeClass('ui-gMapDirections ui-widget-content ui-corner-all')
                .draggable('destroy')
                .resizable('destroy')
                .find('*')
                    .remove();

            jQuery.Widget.prototype.destroy.call(this);
        }
    });


})(jQuery);