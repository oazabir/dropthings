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

    load: function () {
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

    onContentLoad: function (xml) {
        this.xml = xml;
        this.showPhotos();
    },

    getPhotoUrl: function (photoNode, small) {
        return FastFlickrWidget.FLICKR_SERVER_URL + photoNode.getAttribute("server") + '/' + photoNode.getAttribute("id") + '_' + photoNode.getAttribute("secret") + (small ? "_s.jpg" : "_m.jpg");
    },

    getPhotoTitle: function (photoNode) {
        return photoNode.getAttribute("title");
    },

    getPhotoPageUrl: function (photoNode) {
        return FastFlickrWidget.FLICKR_PHOTO_URL + photoNode.getAttribute('owner') + '/' + photoNode.getAttribute('id');
    },

    showPhotos: function () {
        var div = $get(this.container);
        div.innerHTML = "";
        
        if (null == this.xml)
            return (div.innerHTML = "Error occured while loading Flickr feed");

        var photos = this.xml.documentElement.getElementsByTagName("photo");

        var row = 0, col = 0, count = 0;

        var table = document.createElement("table");
        table.align = "center";
        var tableBody = document.createElement("TBODY");
        table.appendChild(tableBody);
        var tr;

        for (var i = 0; i < 9; i++) {
            var photo = photos[i + (this.pageIndex * 9)];

            if (photo == null) {
                Utility.nodisplay(this.nextId);
                break;
            }

            if (col == 0) {
                tr = document.createElement("TR");
                tableBody.appendChild(tr);
            }

            var td = document.createElement("TD");

            var img = document.createElement("IMG");
            img.src = this.getPhotoUrl(photo, true);
            img.style.width = img.style.height = "75px";
            img.style.border = "none";

            var a = document.createElement("A");
            a.href = this.getPhotoPageUrl(photo);
            a.target = "_blank";
            a.title = this.getPhotoTitle(photo);

            a.appendChild(img);
            td.appendChild(a);
            tr.appendChild(td);

            if (++col == 3) { col = 0; row++ }

        }

        div.appendChild(table);

        if (this.pageIndex == 0) Utility.nodisplay(this.previousId);
    },

    previous: function () {
        this.pageIndex--;
        this.showPhotos();

        Utility.display(this.nextId, true);
        if (this.pageIndex == 0)
            Utility.nodisplay(this.previousId);

    },

    next: function () {
        this.pageIndex++;
        this.showPhotos();
        Utility.display(this.previousId, true);
    }
};


