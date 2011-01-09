// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

var FastFlickrWidget = function(url, container, previousId, nextId, cachedJson)
{
    this.url = url;
    this.container = container;
    this.pageIndex = 0;
    this.previousId = previousId;
    this.nextId = nextId;
    this.xml = null;
    this.cachedJson = cachedJson;
}

FastFlickrWidget.FLICKR_SERVER_URL="http://static.flickr.com/";
FastFlickrWidget.FLICKR_PHOTO_URL="http://www.flickr.com/photos/";

FastFlickrWidget.prototype = {

    load: function() {
        if (null == this.cachedJson) {
            this.pageIndex = 0;

            var div = $get(this.container);
            div.innerHTML = "Loading...";

            Proxy.getUrl(this.url, 10, Function.createDelegate(this, this.onContentLoad));
        }
        else {
            var xml = new Sys.Net.XMLDOM(this.cachedJson);
            this.onContentLoad(xml);
        }
    },

    onContentLoad: function(xml) {
        this.xml = xml;
        this.showPhotos();
    },

    getPhotoUrl: function(photoNode, small) {
        return FastFlickrWidget.FLICKR_SERVER_URL + photoNode.getAttribute("server") + '/' + photoNode.getAttribute("id") + '_' + photoNode.getAttribute("secret") + (small ? "_s.jpg" : "_m.jpg");
    },

    getPhotoTitle: function(photoNode) {
        return photoNode.getAttribute("title");
    },

    getPhotoPageUrl: function(photoNode) {
        return FastFlickrWidget.FLICKR_PHOTO_URL + photoNode.getAttribute('owner') + '/' + photoNode.getAttribute('id');
    },

    showPhotos: function() {
        var div = jQuery('#' + this.container);
        div.text("");

        if (null == this.xml)
            return (div.text("Error occured while loading Flickr feed"));

        var photos = this.xml.documentElement.getElementsByTagName("photo");

        var row = 0, col = 0, count = 0;

        var table = jQuery('<table style="position:relative" border="0" align="center"></table>');
        var tableBody = jQuery('<tbody></tbody>');
        table.append(tableBody);
        div.append(table);

        var tr = null;

        for (var i = 0; i < 9; i++) {
            var photo = photos[i + (this.pageIndex * 9)];

            if (photo == null) {
                Utility.nodisplay(this.nextId);
                break;
            }

            if (col == 0) {
                tr = jQuery('<tr></tr>');
                tableBody.append(tr);
            }

            var a = jQuery('<a style="position:relative; width: 75px; height:75px;"></a>')
                            .attr({
                                href: this.getPhotoPageUrl(photo),
                                target: "_blank",
                                title: this.getPhotoTitle(photo)
                            })
                            .append(jQuery('<img border="0" style="position:absolute; left: 0; top: 0; width: 75px; height: 75px; background: #f0f0f0; padding: 5px; border: 1px solid #ddd; -ms-interpolation-mode: bicubic; " />')
                                        .attr({ src: this.getPhotoUrl(photo, true) })
                            );

            tr.append(jQuery('<td style="width: 75px; height: 75px;"></td>').append(a));

            jQuery(a).hover(function() {
                jQuery(this).css({ 'z-index': '60000' }); /*Add a higher z-index value so this image stays on top*/
                jQuery(this).find('img')
                    .addClass("hover")
                    .stop() /* Add class of "hover", then stop animation queue buildup*/
                    .animate({
                        marginTop: '-50px', /* The next 4 lines will vertically align this image */
                        marginLeft: '-50px',
                        top: '50%',
                        left: '50%',
                        width: '240px', /* Set new width */
                        height: '174px', /* Set new height */
                        padding: '5px'
                    }, 200, function() {
                        var src = "" + jQuery(this).attr("src");
                        jQuery(this).attr("src", src.replace("_s", "_m"));
                    }); /* this value of "200" is the speed of how fast/slow this hover animates */

            }, function() {
                jQuery(this).css({ 'z-index': '0' }); /* Set z-index back to 0 */
                jQuery(this).find('img').removeClass("hover").stop()  /* Remove the "hover" class , then stop animation queue buildup*/
		                .animate({
		                    marginTop: '0', /* Set alignment back to default */
		                    marginLeft: '0',
		                    top: '0',
		                    left: '0',
		                    width: '75px', /* Set width back to default */
		                    height: '75px', /* Set height back to default */
		                    padding: '5px'
		                }, 400);
            });

            if (++col == 3) { col = 0; row++ }

        }

        if (this.pageIndex == 0) Utility.nodisplay(this.previousId);
    },

    previous: function() {
        this.pageIndex--;
        this.showPhotos();

        Utility.display(this.nextId, true);
        if (this.pageIndex == 0)
            Utility.nodisplay(this.previousId);

    },

    next: function() {
        this.pageIndex++;
        this.showPhotos();
        Utility.display(this.previousId, true);
    }
};


