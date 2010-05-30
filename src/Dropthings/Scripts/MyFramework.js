// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />

var is = {
    types: ["Array", "RegExp", "Date", "Number", "String", "Object", "HTMLDocument"]
};

for (var i = 0, c; c = is.types[i++]; ) {
    is[c] = (function(type) {
        return function(obj) {
            return Object.prototype.toString.call(obj) == "[object " + type + "]";
        }
    })(c);
}

/*  
Client side class which contains all the necessary operations of client
side activities like Drag & Drop, Add/Edit/Delete widget, switching page
etc.
*/
var DropthingsUI = {
    _LastMaximizedWidget: null,
    // These attributes are injected from server side inside <div> to provide
    // necessary information for widgets like whar's the widgetInstanceId,
    // what's the zoneId
    Attributes: {
        INSTANCE_ID: "_InstanceId",
        ZONE_ID: "_zoneid",
        WIDGET_ZONE_DUMMY_LINK: "widget_holder_panel_post_link"
    },
    // Contains a definition collection of all widgets on the page. Server side
    // widget instance object is serialized and stored in this collection.
    WidgetDefs: [],
    // Store a widget's definition in the WidgetDefs array
    //    setWidgetDef: function(id, expanded, maximized, resized, width, height, zoneId) {
    //        DropthingsUI.WidgetDefs["" + id] = { id: id, expanded: expanded, maximized: maximized, resized: resized, width: width, height: height, zoneId: zoneId };
    //    },
    setWidgetDef: function (id, widgetInstance) {
        DropthingsUI.WidgetDefs["" + id] = widgetInstance;
    },
    // Get a widget's definition from the WidgetDefs array
    getWidgetDef: function (id) {
        return DropthingsUI.WidgetDefs["" + id];
    },
    // Get a widget DivID (server side generated DIV ClientID) from widget instanceId
    getWidgetDivId: function (instanceId) {
        return "WidgetPage_WidgetPanelsLayout_WidgetContainer" + instanceId + "_Widget";
    },
    // Get widget zone DivID (server side generated DIV ClientID) from zoneId
    getWidgetZoneDivId: function (zoneId) {
        return "WidgetPage_WidgetZone" + zoneId + "_WidgetHolderPanel";
    },
    // Initialize client side stuffs. Called during page load.
    init: function (isTemplateUser) {
        var enableSorting = Boolean.parse(isTemplateUser);
        DropthingsUI.initTab(enableSorting);
    },
    // Initializes a tab. Called during tab load.
    initTab: function (enableSorting) {
        jQuery('#tab_container').scrollable();
        if (enableSorting) {
            DropthingsUI.initTabSorting();
        }
    },
    // Use the jQuery sortable plugin to allow tabs to be sorted.
    initTabSorting: function () {
        var plugin = jQuery('#tab_container').find(".tabs").data('sortable');
        if (plugin) plugin.destroy();

        jQuery('#tab_container')
            .find(".tabs")
                .sortable({
                    axis: 'x',
                    items: '.tab' + ':not(.nodrag)',
                    opacity: 0.8,
                    revert: true,
                    stop: function (e, ui) {
                        var position = ui.item.parent().children(':not(.nodrag)').index(ui.item);
                        var pageId = ui.item.attr('id').match(/\d+/);

                        DropthingsUI.Actions.onTabItemDrop(pageId[0], position, function () {
                            //do work after saving the tab order
                        });
                    }
                });
    },
    // Hook jQuery stuffs on a widget to allow widget title change by cliking
    // on widget title, allow expand, collapse, maximize, minimize, close
    setActionOnWidget: function (widgetId) {
        var nohref = "javascript:void(0);";
        var widget = jQuery('#' + widgetId);
        var widgetInstanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
        var widgetDef = DropthingsUI.getWidgetDef(widgetInstanceId);
        //var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);

        //Widget Title
        var widgetTitle = widget.find('.widget_title_label');
        var widgetInput = widget.find('.widget_title_input');
        var widgetSubmit = widget.find('.widget_title_submit');

        var widgetCollaspe = widget.find('.widget_min');
        var widgetExpand = widget.find('.widget_expand');
        var widgetResizeFrame = widget.find('.widget_resize_frame');
        var widgetCloseButton = widget.find('.widget_close');

        widgetTitle.show();
        widgetInput.hide();
        widgetSubmit.hide();

        if (widgetDef.Widget.IsLocked) {
            
        }
        else {
            widgetTitle
            .unbind('click')
            .bind('click', function () {
                widgetTitle.hide();
                widgetInput
                    .show()
                    .attr('value', widgetTitle.text())
                    .unbind('keypress')
                    .bind('keypress', function (e) {
                        if (e.which == 13) {
                            widgetSubmit.click();
                            return false;
                        }
                    });

                widgetSubmit
                    .show()
                    .unbind('click')
                    .bind('click', function () {
                        var newTitle = widgetInput.attr('value');
                        widgetSubmit.hide();
                        widgetTitle.text(newTitle).show();
                        widgetInput.hide();

                        Dropthings.Web.Framework.WidgetService.ChangeWidgetTitle(widgetInstanceId, newTitle);
                        return false;
                    });

                return false;
            });

            //Expand/Collaspe widget
            if (widgetDef.Expanded) {
                widgetCollaspe.show();
                widgetExpand.hide();
            }
            else {
                widgetCollaspe.hide();
                widgetExpand.show();
            }

            widgetCloseButton
            .unbind('click')
            .bind('click', function () {
                widget.hide('slow');
                eval(widgetCloseButton.attr("href"));
                return false;
            });

            widgetExpand
	        .unbind('click')
            .bind('click', function () {
                widgetResizeFrame.show();
                widgetExpand.hide();
                widgetCollaspe.show();
                eval(widgetExpand.attr("href"));
                return false;
            });


            widgetCollaspe
		    .unbind('click')
            .bind('click', function () {
                widgetResizeFrame.hide();
                widgetExpand.show();
                widgetCollaspe.hide();
                eval(widgetCollaspe.attr("href"));
                return false;
            });

            //Maximize/Minimize widget
            var widgetRestore = widget.find('.widget_restore');
            var widgetMaximize = widget.find('.widget_max');

            if (widgetDef.Maximized) {
                widgetRestore.show();
                widgetMaximize.hide();

                DropthingsUI._LastMaximizedWidget = new WidgetMaximizeBehavior(widgetId);
                DropthingsUI._LastMaximizedWidget.maximize();
            }
            else {
                widgetRestore.hide();
                widgetMaximize.show();
            }

            widgetMaximize
	        .unbind('click')
            .bind('click', function () {
                //widgetDef.maximized = true;
                widgetMaximize.hide();
                widgetRestore.css({ display: 'block' });
                // Asynchronously notify server that widget maximized
                DropthingsUI.Actions.maximizeWidget(widgetId);
                return false;
            });

            widgetRestore
		    .unbind('click')
            .bind('click', function () {
                //widgetDef.maximized = true;
                widgetMaximize.show();
                widgetRestore.hide();
                // Asynchronously notify server that widget restored
                DropthingsUI.Actions.restoreWidget(widgetId);
                return false;
            });
        }
    },
    // Hook jQuery sortable plugin with the widget zones so that widgets
    // can be dragged & dropped between widegt zones.
    initDragDrop: function (zoneId, widgetClass, newWidgetClass, handleClass, zoneClass, zonePostbackTrigger) {
        DropthingsUI.initAddStuff(zoneClass, newWidgetClass);

        // Get all widget zones on the page and allow widget to be dropped on any of them
        var allZones = jQuery('.' + zoneClass);

        //var zone = jQuery('#' + zoneId);
        allZones.each(function () {
            var plugin = jQuery(this).data('sortable');
            if (plugin) plugin.destroy();
        });

        allZones.sortable({
            items: '.' + widgetClass + ':not(.nodrag)',
            handle: '.' + handleClass,
            cancel: 'a, input',
            cursor: 'move',
            appendTo: 'body',
            connectWith: allZones,
            opacity: 0.8,
            revert: true,
            tolerance: 'pointer',
            placeholder: 'placeholder',
            start: function (e, ui) {
                ui.helper.css("width", ui.item.parent().outerWidth());
                ui.placeholder.height(ui.item.height());

                DropthingsUI.suspendPendingWidgetZoneUpdate();
            },
            change: function (e, ui) {
                if (ui.element) {
                    var w = ui.element.width();
                    ui.placeholder.width(w);
                    ui.helper.css("width", w);

                    if (ui.item !== undefined) {
                        ui.placeholder.height(ui.item.height());
                    }
                    else {
                        //this is a new item from galarry
                        ui.placeholder.height(200);
                    }
                }
            },
            stop: function (e, ui) {
                var position = ui.item.parent()
                                    .children()
                                    .index(ui.item);

                var widgetZone = ui.item.parents('.' + zoneClass + ':first');
                var containerId = parseInt(widgetZone.attr(DropthingsUI.Attributes.ZONE_ID));

                if (ui.item.hasClass(newWidgetClass)) {
                    //new item has been dropped into the sortable list
                    var widgetId = ui.item.attr('id').match(/\d+/);

                    // OMAR: Create a dummy widget placeholder while the real widget loads
                    var templateData = { title: jQuery(ui.item).text() };
                    var widgetTemplateNode = jQuery("#new_widget_template").clone();
                    widgetTemplateNode.drink(templateData);
                    widgetTemplateNode.insertAfter(ui.item);

                    ui.item.remove();

                    DropthingsUI.Actions.onWidgetAdd(widgetId[0], containerId, position,
                        function () {
                            DropthingsUI.updateWidgetZone(widgetZone);
                        });
                }
                else {
                    ui.item.css({ 'width': 'auto' });
                    var instanceId = parseInt(ui.item.attr(DropthingsUI.Attributes.INSTANCE_ID));
                    DropthingsUI.Actions.onDrop(containerId, instanceId, position, function () {
                        //DropthingsUI.updateWidgetZone(widgetZone);
                    });
                }
            }
        })
        .bind("sortreceive", function (e, ui) {
            var widget = jQuery(ui.item);
            if (widget.hasClass(newWidgetClass)) {
                //widget.remove();
            }
        });
    },
    // Cause the widget to refresh itself by simularing a dummy 
    // postback on the widget's updatepanel
    updateWidget: function (widgetId) {
        var widget = jQuery('#' + widgetId);
        var postbackLink = widget.find('.dummy_postback');
        eval(postbackLink.attr('href'));
    },
    // Hook jquery stuffs to make a widget vertically resizable.
    initResizer: function (divId) {
        // Disabling this feature since it's buggy and hardly anyone ever uses it.
        return false;

        jQuery('#' + divId)
            .resizable(
            {
                handles: 's',
                resize: function (e, ui) {
                    if (!ui.options.handles['w'] && !ui.options.handles['e']) {
                        var widget = ui.element.parent().parent();
                        var widgetDef = DropthingsUI.getWidgetDef(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
                        widgetDef.Expanded = false;
                        if (!widgetDef.Maximized) {
                            
                        }
                        else {
                            ui.element.css({ 'width': 'auto' });
                        }
                    }
                },
                stop: function (e, ui) {
                    var widget = ui.element.parent().parent();
                    var widgetDef = DropthingsUI.getWidgetDef(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
                    widgetDef.Expanded = false;
                    if (!widgetDef.Maximized) {
                        DropthingsUI.Actions.resizeWidget(widget, ui.element.height());
                    }
                }
            });
    },
    // Hook jquery stuffs on the widget list so that new widgets
    // can be added by dragging & dropping a widget from the widget
    // list.
    initAddStuff: function (zoneClass, newWidgetClass) {
        var allZones = jQuery('.' + zoneClass);

        var widgetLinks = jQuery('.' + newWidgetClass);
        widgetLinks.draggable("destroy");
        widgetLinks.each(function () {
            var plugin = jQuery(this).data('sortable');
            if (plugin) plugin.destroy();
        });

        jQuery('.' + newWidgetClass).draggable(
        {
            connectToSortable: allZones,
            helper: 'clone',
            start: function () {
                
            },
            stop: function () {
                
            }
        });
    },
    // Queue update for a widget zone.
    updateWidgetZone: function (widgetZone) {
        // OMAR: update the widget zone after three seconds because user might drag & drop another widget
        // in the meantime.
        if ((DropthingsUI.__updateWidgetZoneTimerId || 0) === 0) {
            var zoneList = DropthingsUI.__widgetZonesToUpdate || [];
            zoneList.push(widgetZone);
            DropthingsUI.__widgetZonesToUpdate = zoneList;
            widgetZone.attr("__pendingUpdate", "1");
            DropthingsUI.__updateWidgetZoneTimerId = window.setTimeout(function () {
                jQuery(DropthingsUI.__widgetZonesToUpdate).each(function (index, zone) {
                    if (zone.attr("__pendingUpdate") == "1") {
                        zone.attr("__pendingUpdate", "0");
                        var f = function () { return Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack(); };
                        Utility.untilFalse(f, function () {
                            DropthingsUI.asyncPostbackWidgetZone(zone);
                        });
                    }
                });
                DropthingsUI.__updateWidgetZoneTimerId = 0;
            }, 1000);
        }
        else {
            // Restart the timer when another update is queued
            DropthingsUI.suspendPendingWidgetZoneUpdate();
            DropthingsUI.updateWidgetZone(widgetZone);
        }
    },
    suspendPendingWidgetZoneUpdate: function () {
        if (DropthingsUI.__updateWidgetZoneTimerId > 0) {
            window.clearTimeout(DropthingsUI.__updateWidgetZoneTimerId);
            DropthingsUI.__updateWidgetZoneTimerId = 0;
        }
    },
    asyncPostbackWidgetZone: function (widgetZone) {
        var postBackLink = widgetZone.parent().find("." + DropthingsUI.Attributes.WIDGET_ZONE_DUMMY_LINK);
        eval(postBackLink.attr('href'));
    },
    showWidgetGallery: function () {
        jQuery('#Widget_Gallery').show();
    },
    hideWidgetGallery: function () {
        jQuery('#Widget_Gallery').hide("slow");
    },
    Actions: {
        deleteWidget: function (instanceId) {
            Dropthings.Web.Framework.WidgetService.DeleteWidgetInstance(instanceId);
            jQuery(DropthingsUI.getWidgetDivId('#' + instanceId)).remove();
        },

        maximizeWidget: function (widgetId) {
            var widget = jQuery('#' + widgetId);
            
            //if collaspe then expand it
            var widgetDef = DropthingsUI.getWidgetDef(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            widgetDef.Maximized = true;
            if (!widgetDef.Expanded) {
                widget.find('.widget_expand').click();
            }

            if (null !== DropthingsUI._LastMaximizedWidget) {
                DropthingsUI._LastMaximizedWidget.restorePreviouslyMaximizedWidget();
                DropthingsUI._LastMaximizedWidget.dispose();
            }

            DropthingsUI._LastMaximizedWidget = new WidgetMaximizeBehavior(widgetId);
            DropthingsUI._LastMaximizedWidget.maximize();

            Dropthings.Web.Framework.WidgetService.MaximizeWidgetInstance(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
        },

        restoreWidget: function (widgetId) {
            var widget = jQuery('#' + widgetId);
            var widgetDef = DropthingsUI.getWidgetDef(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            widgetDef.Maximized = false;
            DropthingsUI._LastMaximizedWidget.restore();

            if (null !== DropthingsUI._LastMaximizedWidget) DropthingsUI._LastMaximizedWidget.dispose();

            Dropthings.Web.Framework.WidgetService.RestoreWidgetInstance(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            return false;
        },

        collaspeWidget: function (widgetId, postbackUrl) {
            var widget = jQuery('#' + widgetId);
            
            var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
            var widgetDef = DropthingsUI.getWidgetDef(instanceId);
            widgetDef.Expanded = false;
            Dropthings.Web.Framework.WidgetService.CollaspeWidgetInstance(instanceId, postbackUrl, DropthingsUI.Actions._onCollaspeWidgetComplete);

            if (widgetDef.Maximized) {
                if (null !== DropthingsUI._LastMaximizedWidget) {
                    jQuery('#widgetMaxBackground').css('height', DropthingsUI._LastMaximizedWidget.visibleHeightIfWidgetCollasped);
                }
            }
        },

        _onCollaspeWidgetComplete: function (postbackUrl) {
            eval(postbackUrl);
        },

        expandWidget: function (widgetId, postbackUrl) {
            var widget = jQuery('#' + widgetId);

            var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
            var widgetDef = DropthingsUI.getWidgetDef(instanceId);
            widgetDef.Expanded = true;
            Dropthings.Web.Framework.WidgetService.ExpandWidgetInstance(instanceId, postbackUrl, DropthingsUI.Actions._onExpandWidgetComplete);

            if (widgetDef.Maximized) {
                if (null !== DropthingsUI._LastMaximizedWidget) {
                    jQuery('#widgetMaxBackground').css('height', DropthingsUI._LastMaximizedWidget.visibleHeightIfWidgetExpanded);
                }
            }
        },

        _onExpandWidgetComplete: function (postbackUrl) {
            eval(postbackUrl);
        },

        resizeWidget: function (widget, resizeHeight) {
            var instanceId = parseInt(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            var widgetDef = DropthingsUI.getWidgetDef(instanceId);

            if (!widgetDef.Maximized) {
                Dropthings.Web.Framework.WidgetService.ResizeWidgetInstance(instanceId, 0, resizeHeight);
                widgetDef.Resized = true;
                widgetDef.Height = resizeHeight;
            }

            return false;
        },

        deletePage: function (pageId) {
            Dropthings.Web.Framework.PageService.DeletePage(pageId, DropthingsUI.Actions._onDeletePageComplete);
            jQuery('#Tab' + pageId).remove();
        },

        _onDeletePageComplete: function (currentPageName) {
            document.location.href = '?' + encodeURI(currentPageName);
        },

        changePageLayout: function (newLayout) {
            Dropthings.Web.Framework.PageService.ChangePageLayout(newLayout, DropthingsUI.Actions._onChangePageLayoutComplete);
        },

        _onChangePageLayoutComplete: function (arg) {
            document.location = document.location.href;
        },

        newPage: function (newLayout) {
            Dropthings.Web.Framework.PageService.NewPage(newLayout, DropthingsUI.Actions._onNewPageComplete);
        },

        _onNewPageComplete: function (newPageName) {
            document.location.href = '?' + encodeURI(newPageName);
        },

        renamePage: function (newLabel) {
            var newPageName = document.getElementById(newLabel).value;
            Dropthings.Web.Framework.PageService.RenamePage(newPageName, DropthingsUI.Actions._onRenamePageComplete);
        },

        _onRenamePageComplete: function () {
            __doPostBack('TabUpdatePanel', '');
        },

        changePage: function (pageId, pageName) {
            document.location.href = '?' + encodeURI(pageName);
        },

        _onChangePageComplete: function (arg) {
            __doPostBack('UpdatePanelTabAndLayout', '');
        },

        onDrop: function (columnNo, instanceId, row, callback) {
            Dropthings.Web.Framework.WidgetService.MoveWidgetInstance(instanceId, columnNo, row, callback);
        },

        onWidgetAdd: function (widgetId, columnNo, row, callback) {
            Dropthings.Web.Framework.WidgetService.AddWidgetInstance(widgetId, columnNo, row, function () { callback(); });
        },
        hide: function (id) {
            document.getElementById(id).style.display = "none";
        },
        onTabItemDrop: function (tabId, orderNo, callback) {
            Dropthings.Web.Framework.PageService.MoveTab(tabId, orderNo, callback);
        },

        showHelp: function () {
            var request = new Sys.Net.WebRequest();
            request.set_httpVerb("GET");
            request.set_url('help.aspx');
            request.add_completed(function (executor) {
                if (executor.get_responseAvailable()) {
                    var helpDiv = $get('HelpDiv');
                    var helpLink = $get('HelpLink');

                    var helpLinkBounds = Sys.UI.DomElement.getBounds(helpLink);

                    helpDiv.style.top = (helpLinkBounds.y + helpLinkBounds.height) + "px";

                    var content = executor.get_responseData();
                    helpDiv.innerHTML = content;
                    helpDiv.style.display = "block";
                }
            });

            var executor = new Sys.Net.XMLHttpExecutor();
            request.set_executor(executor);
            executor.executeRequest();
        }
    }
};

var Utility =
{
    // change to display:none
    nodisplay: function(e) {
        if (typeof e == "object") e.style.display = "none"; else if ($get(e) !== null) $get(e).style.display = "none";
    },
    // change to display:block
    display: function(e, inline) {
        if (typeof e == "object") e.style.display = (inline ? "inline" : "block"); else if ($get(e) !== null) $get(e).style.display = (inline ? "inline" : "block");
    },

    getContentHeight: function() {
        if ((document.body) && (document.body.offsetHeight)) {
            return document.body.offsetHeight;
        }

        return 0;
    },

    getViewPortWidth: function() {
        var width = 0;

        if ((document.documentElement) && (document.documentElement.clientWidth)) {
            width = document.documentElement.clientWidth;
        }
        else if ((document.body) && (document.body.clientWidth)) {
            width = document.body.clientWidth;
        }
        else if (window.innerWidth) {
            width = window.innerWidth;
        }

        return width;
    },

    getViewPortHeight: function() {
        var height = 0;

        if (window.innerHeight) {
            height = window.innerHeight - 18;
        }
        else if ((document.documentElement) && (document.documentElement.clientHeight)) {
            height = document.documentElement.clientHeight;
        }

        return height;
    },

    getViewPortScrollX: function() {
        var scrollX = 0;

        if ((document.documentElement) && (document.documentElement.scrollLeft)) {
            scrollX = document.documentElement.scrollLeft;
        }
        else if ((document.body) && (document.body.scrollLeft)) {
            scrollX = document.body.scrollLeft;
        }
        else if (window.pageXOffset) {
            scrollX = window.pageXOffset;
        }
        else if (window.scrollX) {
            scrollX = window.scrollX;
        }

        return scrollX;
    },

    getViewPortScrollY: function() {
        var scrollY = 0;

        if ((document.documentElement) && (document.documentElement.scrollTop)) {
            scrollY = document.documentElement.scrollTop;
        }
        else if ((document.body) && (document.body.scrollTop)) {
            scrollY = document.body.scrollTop;
        }
        else if (window.pageYOffset) {
            scrollY = window.pageYOffset;
        }
        else if (window.scrollY) {
            scrollY = window.scrollY;
        }

        return scrollY;
    },

    centralize: function(e) {
        var x = (($U.getViewPortWidth() - e.offsetWidth) / 2);
        var y = (($U.getViewPortHeight() - e.offsetHeight) / 2) + Utility.getViewPortScrollY();

        Sys.UI.DomElement.setLocation(e, x, y);
    },

    getAbsolutePosition: function(element, positionType) {
        var position = 0;
        while (element !== null) {
            position += element["offset" + positionType];
            element = element.offsetParent;
        }

        return position;
    },


    blockUI: function() {
        Utility.display('blockUI');
        var blockUI = $get('blockUI');

        if (blockUI !== null) {// it will be null if called from CompactFramework
            blockUI.style.height = Math.max(Utility.getContentHeight(), 1000) + "px";
        }
    },

    unblockUI: function() {
        Utility.nodisplay('blockUI');
    },

    untilTrue: function(test, callback) {
        if (test() === true) {
            callback();
        }
        else {
            window.setTimeout(function() { Utility.untilTrue(test, callback); }, 200);
        }
    },

    untilFalse: function(test, callback) {
        Utility.untilTrue(function() { return test() === false }, callback);
    },

    getXMLDocument: function(xmlString) {
        var doc;
        if ($.browser.msie) {
            doc = new ActiveXObject('Microsoft.XMLDOM');
            doc.async = 'false'
            doc.loadXML(xmlString);
        } else {
            doc = (new DOMParser()).parseFromString(xmlString, 'text/xml');
        }
        return doc;
    },

    getJSONObject: function(jsonString) {
        return (new Function("return " + jsonString))();
    }
};

var DeleteWarning =
{
    yesCallback: null,
    noCallback: null,
    _initialized: false,
    init: function() {
        if (DeleteWarning._initialized) return;

        var hiddenHtmlTextArea = $get('DeleteConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);

        DeleteWarning._initialized = true;
    },
    show: function(yesCallback, noCallback) {
        DeleteWarning.init();

        Utility.blockUI();

        var popup = $get('DeleteConfirmPopup');
        Utility.display(popup);

        DeleteWarning.yesCallback = yesCallback;
        DeleteWarning.noCallback = noCallback;

        $addHandler($get("DeleteConfirmPopup_Yes"), 'click', DeleteWarning._yesHandler);
        $addHandler($get("DeleteConfirmPopup_No"), 'click', DeleteWarning._noHandler);
    },
    hide: function() {
        DeleteWarning.init();

        var popup = $get('DeleteConfirmPopup');
        Utility.nodisplay(popup);

        $clearHandlers($get('DeleteConfirmPopup_Yes'));

        Utility.unblockUI();

    },
    _yesHandler: function() {
        DeleteWarning.hide();
        DeleteWarning.yesCallback();
    },
    _noHandler: function() {
        DeleteWarning.hide();
        DeleteWarning.noCallback();
    }
};

var DeletePageWarning =
{
    yesCallback: null,
    noCallback: null,
    _initialized: false,
    init: function() {
        if (DeletePageWarning._initialized) return;

        var hiddenHtmlTextArea = $get('DeletePageConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);

        DeletePageWarning._initialized = true;
    },
    show: function(yesCallback, noCallback) {
        DeletePageWarning.init();

        Utility.blockUI();

        var popup = $get('DeletePageConfirmPopup');
        Utility.display(popup);

        DeletePageWarning.yesCallback = yesCallback;
        DeletePageWarning.noCallback = noCallback;

        $addHandler($get("DeletePageConfirmPopup_Yes"), 'click', DeletePageWarning._yesHandler);
        $addHandler($get("DeletePageConfirmPopup_No"), 'click', DeletePageWarning._noHandler);
    },
    hide: function() {
        DeletePageWarning.init();

        var popup = $get('DeletePageConfirmPopup');
        Utility.nodisplay(popup);

        $clearHandlers($get('DeletePageConfirmPopup_Yes'));

        Utility.unblockUI();

    },
    _yesHandler: function() {
        DeletePageWarning.hide();
        DeletePageWarning.yesCallback();
    },
    _noHandler: function() {
        DeletePageWarning.hide();
        DeletePageWarning.noCallback();
    }
};

function winopen(url, w, h) {
    var left = (screen.width) ? (screen.width - w) / 2 : 0;
    var top = (screen.height) ? (screen.height - h) / 2 : 0;

    window.open(url, "_blank", "width=" + w + ",height=" + h + ",left=" + left + ",top=" + top + ",resizable=yes,scrollbars=yes");

    return;
}

function winopen_withlocationbar(url) {
    var w = screen.width / 2;
    var h = screen.height / 2;
    var left = (screen.width) ? (screen.width - w) / 2 : 0;
    var top = (screen.height) ? (screen.height - h) / 2 : 0;

    window.open(url, "_blank");

    return;
}

function winopen2(url, target, w, h) {
    var left = (screen.width) ? (screen.width - w) / 2 : 0;
    var top = (screen.height) ? (screen.height - h) / 2 : 0;

    if (popupWin_2[target] != null)
        if (!popupWin_2[target].closed)
        popupWin_2[target].focus();
    else
        popupWin_2[target] = window.open(url, target, "width=" + w + ",height=" + h + ",left=" + left + ",top=" + top + ",resizable=yes,scrollbars=yes");
    else
        popupWin_2[target] = window.open(url, target, "width=" + w + ",height=" + h + ",left=" + left + ",top=" + top + ",resizable=yes,scrollbars=yes");

    return;
}

//var LayoutPicker =
//{
//    yesCallback: null,
//    noCallback: null,
//    type1Callback: null,
//    type2Callback: null,
//    type3Callback: null,
//    type4Callback: null,
//    _initialized: false,
//    clientID: null,
//    init: function() {
//        if (LayoutPicker._initialized) return;

//        var hiddenHtmlTextArea = $get('LayoutPickerPopupPlaceholder');

//        var html = hiddenHtmlTextArea.value;
//        var div = document.createElement('div');
//        div.innerHTML = html;
//        document.body.appendChild(div);

//        LayoutPicker._initialized = true;
//    },
//    show: function(Type1Callback, Type2Callback, Type3Callback, Type4Callback, noCallback, clientID) {
//        LayoutPicker.init();

//        Utility.blockUI();

//        var popup = $get('LayoutPickerPopup');
//        Utility.display(popup);

//        LayoutPicker.type1Callback = Type1Callback;
//        LayoutPicker.type2Callback = Type2Callback;
//        LayoutPicker.type3Callback = Type3Callback;
//        LayoutPicker.type4Callback = Type4Callback;
//        LayoutPicker.clientID = clientID;
//        LayoutPicker.noCallback = noCallback;

//        $addHandler($get("SelectLayoutPopup_Cancel"), 'click', LayoutPicker._noHandler);
//        $addHandler($get("SelectLayoutPopup_Type1"), 'click', LayoutPicker._type1Handler);
//        $addHandler($get("SelectLayoutPopup_Type2"), 'click', LayoutPicker._type2Handler);
//        $addHandler($get("SelectLayoutPopup_Type3"), 'click', LayoutPicker._type3Handler);
//        $addHandler($get("SelectLayoutPopup_Type4"), 'click', LayoutPicker._type4Handler);

//    },
//    hide: function() {
//        LayoutPicker.init();


//        var popup = $get('LayoutPickerPopup');
//        Utility.nodisplay(popup);
//        //is there a cleaner way to clear the handlers?
//        $clearHandlers($get('SelectLayoutPopup_Type1'));
//        $clearHandlers($get('SelectLayoutPopup_Type2'));
//        $clearHandlers($get('SelectLayoutPopup_Type3'));
//        $clearHandlers($get('SelectLayoutPopup_Type4'));

//        Utility.unblockUI();

//    },

//    _type1Handler: function() {
//        LayoutPicker.hide();
//        LayoutPicker.type1Callback();
//    },
//    _type2Handler: function() {
//        LayoutPicker.hide();
//        LayoutPicker.type2Callback();
//    },
//    _type3Handler: function() {
//        LayoutPicker.hide();
//        LayoutPicker.type3Callback();
//    },
//    _type4Handler: function() {
//        LayoutPicker.hide();
//        LayoutPicker.type4Callback();
//    },

//    _noHandler: function() {
//        LayoutPicker.hide();
//        LayoutPicker.noCallback();
//    }
//};

var WidgetMaximizeBehavior = function(widgetId) {
    this.visibleHeightIfWidgetExpanded = 0;
    this.visibleHeightIfWidgetCollasped = 0;
    this.LastDomLocation = null;

    this.init = function(widgetId) {
        if (this.initialized) return;

        this.widget = jQuery('#' + widgetId);

        // Remember where the widget was last time
        this.LastDomLocation = { container: this.widget.parent(), position: this.widget.parent().children().index(this.widget) };
        this.instanceId = this.widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
    }

    this.init(widgetId);

    this.dispose = function() {
        this.initialized = false;
        this.instanceId = 0;
        this.widget = null;
    };

    this.maximize = function() {
        this.fitToViewPort();
        window.scrollTo(0, 0);
    };

    this.restorePreviouslyMaximizedWidget = function() {
        if (this.widget != null)
            this.widget.find('.widget_restore').click();
    };

    this.restore = function() {
        var widgetDef = DropthingsUI.getWidgetDef(this.instanceId);
        var height = widgetDef.Resized ? widgetDef.Height + 'px' : 'auto';
        this.widget.css({ marginRight: '0px', left: 'auto', top: 'auto', width: 'auto', height: 'auto', position: 'static' });

        widgetDef.Maximized = false;

        var resizeFrame = this.widget.find('.widget_resize_frame');
        resizeFrame.css({ width: 'auto', height: height });
        //get the current item of the postion

        var currentWidget = jQuery(this.LastDomLocation.container).children()[this.LastDomLocation.position];

        if (currentWidget != null) {
            this.widget.insertBefore(currentWidget);
        }
        else {
            //
            var length = jQuery(this.LastDomLocation.container).children().length;

            if (this.LastDomLocation.position >= length) {
                //add as last item
                jQuery(this.LastDomLocation.container).append(this.widget);
            }
            else {
                this.widget.prependTo(this.LastDomLocation.container);
            }
        }

        //this.widget.prependTo(this.LastDomLocation.container);
        this.restoredInstanceId = this.instanceId;
        this.dispose();
    };

    this.fitToViewPort = function() {
        var maxBackground = jQuery('#widget_area_wrapper');
        //var visibleHeight = (Utility.getViewPortHeight() - Utility.getAbsolutePosition(maxBackground[0], 'Top'));
        var visibleHeight = (Utility.getViewPortHeight() - maxBackground.offset().top);

        if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
            visibleHeight -= 20;
        }

        this.widget.prependTo(maxBackground);

        this.widget.css({ zIndex: '10000', display: 'block', clear: 'both', marginRight: '15px' });

        var widgetDef = DropthingsUI.getWidgetDef(this.widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
        widgetDef.Maximized = true;

        //        var WidgetResizeFrame = this.widget.find('.widget_resize_frame');

        //        this.visibleHeightIfWidgetExpanded = visibleHeight;
        //        this.visibleHeightIfWidgetCollasped = 40;

        //        if (widgetDef.expanded) {
        //            WidgetResizeFrame.height((this.visibleHeightIfWidgetExpanded - 40) + 'px');
        //        }
        //        else {
        //            WidgetResizeFrame.height(this.visibleHeightIfWidgetCollasped + 'px'); //this is a min height
        //        }
    };
};
var WidgetPermission =
{
    Save: function() {

        WidgetPermission.showProgress(false);
        jQuery('#Message').html('');
        var widgets = jQuery('.widgetItem');
        var nameValuePair = '';

        widgets.each(function() {
            var widget = jQuery(this);
            var widgetId = widget.attr('id').match(/\d+/);
            var roles = jQuery('.role', '#' + widget.attr('id'));
            nameValuePair += widgetId[0] + ":";
            var roleStr = '';
            roles.each(function() {
                chkRole = jQuery(this);
                if (chkRole[0] != undefined) {
                    if (chkRole[0].checked) {
                        roleStr = roleStr + chkRole.attr('id').slice(3) + ',';
                    }
                }
            });

            if (roleStr != null && roleStr.length > 0) {
                nameValuePair += roleStr.substring(0, roleStr.length - 1);
            }
            nameValuePair += ';';
        });

        Dropthings.Web.Framework.WidgetService.AssignPermission(nameValuePair,
            function(result) {
                WidgetPermission.showProgress(true);
                jQuery('#Message').html("Permission assigned successfully.");
            });

    },
    showProgress: function(hide) {
        if (hide)
            jQuery('#progress').hide();
        else
            jQuery('#progress').show();
    }
};

function pageUnload() {

}