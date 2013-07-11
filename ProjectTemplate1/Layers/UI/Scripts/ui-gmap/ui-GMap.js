(function($) {
    jv = {}
    jv.wGMap = {
        wGMapCue: [] //widgets waiting to load Google API
         , gMapApiIsLoading: false
        , mapHtml: '<div class="map-container"></div>'
        , mapBarHtml: function() { return jQuery('#mapBarTemplate').html(); }
        , mapApiLoaded: function() {
            for (var i = 0; i < jv.wGMap.wGMapCue.length; i++) {
                jv.wGMap.wGMapCue[i].initMap();
            }
        }
    }

    jQuery.widget("jv.GMapIt", {
        options: {}
        , _create: function() {

            if ((typeof (window["google"]) == 'undefined') || typeof (window["google"]['maps']) == 'undefined') {
                jv.wGMap.wGMapCue.push(this);
                if (!jv.wGMap.gMapApiIsLoading) {
                    jv.wGMap.gMapApiIsLoading = true;
                    jQuery.getScript('http://maps.google.com/maps/api/js?language=en&callback=jv.wGMap.mapApiLoaded&sensor=false&key=' + this.options.GApiKey);
                }
            }
            else {
                this.initMap();
            }
        },
        initMap: function() {

            var w = this;
            var $el = jQuery(this.element);

            //$el.append(jv.wGMap.mapHtml).append(jv.wGMap.mapBarHtml());
            $el.append(jv.wGMap.mapHtml);






            this.options.mapGeoCoder = new google.maps.Geocoder();
            this.options.map = new google.maps.Map(jQuery(this.element).find('div.map-container:first')[0], {
                zoom: 13,
                center: new google.maps.LatLng(this.options.latitude, this.options.longitude),
                mapTypeId: google.maps.MapTypeId.ROADMAP
                , mapTypeControl: false // The initial enabled/disabled state of the Map type control.
            });


            jQuery(this.element).parent().append('<div class="mapDirBox"></div>');

            jQuery(this.element).parent().find('div.mapDirBox:first')
                        .gMapDirections({
                            map: w.options.map
                            , close: function() {
                                $el.find('div.map-bar #directions').attr('checked', false);
                            }
                        });


            $el   //create directions search box
                    .addClass('wGMapContainer ui-widget ui-widget-content')
                    .find('div.map-container')
                        .hover(
                                    function() {
                                        jQuery(this).siblings('div.map-bar').show('blind');
                                    },
                                    function() {
                                        jQuery(this).siblings('div.map-bar').hide();
                                    })
                    .end()
                    .find('div.map-bar')
                        .css('width', jQuery(this.element).css('width'))
                        .fadeTo(1, 0.4)
                        .hover(
                                    function() {
                                        jQuery(this).fadeTo(1, 0.8);
                                    },
                                    function() {
                                        jQuery(this).fadeTo(1, 0.4)
                                    })
                        .hide()
                        .find('#directions')
                            .click(function(e) {
                                if (jQuery(this).is(':checked')) {
                                    // stupid IE does not display control unless we reposition as absolute
                                    // even it has been positioned at css
                                    jQuery(document).find('div.mapDirBox:first')
                                            .show()
                                            .css('position', 'absolute');
                                }
                                else {
                                    jQuery(document).find('div.mapDirBox:first').hide();
                                }
                            })
                        .end()
                    .end()
                    .find('select.mapTypeControl')
                        .change(function(e) {
                            w.options.map.setMapTypeId(jQuery(this).val());
                        })
                    .end()
                    .find('button.mapGeoCodeBtn')
                        .button({
                            icons: { primary: 'ui-icon-search' }
                        })
                        .click(function(e) {

                            var address = jQuery(this).siblings('input.mapGeoCodeTxt').val();
                            if (w.options.mapGeoCoder) {
                                w.options.mapGeoCoder.geocode({ 'address': address }, function(results, status) {
                                    if (status == google.maps.GeocoderStatus.OK) {
                                        w.options.map.setCenter(results[0].geometry.location);
                                        var marker = new google.maps.Marker({
                                            map: w.options.map,
                                            position: results[0].geometry.location,
                                            title: address

                                        });

                                        /*
                                        var infowindow = new google.maps.InfoWindow({
                                        content: '<div id="content">' +
                                        '<div id="siteNotice"></div>' +
                                        '<h1 id="firstHeading" class="firstHeading">' + address + '</h1>' +
                                        '<div id="bodyContent"></div>' +
                                        '</div>'
                                        });

                                        google.maps.event.addListener(marker, 'click', function() {
                                        infowindow.open(w.options.map, marker);
                                        });
                                        */

                                    } else {
                                        alert("Geocode was not successful for the following reason: " + status);
                                    }
                                });
                            }


                            return false;
                        });

        },
        destroy: function() {

            jQuery(this.element).find('div.map-bar').unbind('click');

            jQuery.Widget.prototype.destroy.call(this);
        }
    });

})(jQuery);
