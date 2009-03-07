// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
var DropthingsUI = {
    _LastMaximizedWidget: null,
    Attributes: {
        INSTANCE_ID: "_InstanceId",
        RESIZED: "_Resized",
        EXPANDED: "_Expanded",
        MAXIMIZED: "_Maximized",
        WIDTH: "_Width",
        HEIGHT: "_Height",
        ZONE_ID: "_zoneid",
        WIDGET_ZONE_DUMMY_LINK: "widget_holder_panel_post_link"
    },
    getWidgetDivId: function(instanceId) {
        return "#WidgetPage_WidgetPanelsLayout_WidgetContainer" + instanceId + "_Widget";
    },
    getWidgetZoneDivId: function(zoneId) {
        return "#WidgetPage_WidgetZone" + zoneId + "_WidgetHolderPanel";
    },
    init: function() {
        DropthingsUI.initTab();
    },
    initTab: function() {
        $('#tab_container').scrollable();
    },
    setActionOnWidget: function(widgetId) {
        var nohref = "javascript:void(0);";
        var widget = $('#' + widgetId)
        var widgetInstanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
        //var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);

        //Widget Title
        var widgetTitle = widget.find('.widget_title_label');
        var widgetInput = widget.find('.widget_title_input');
        var widgetSubmit = widget.find('.widget_title_submit');

        widgetTitle.show();
        widgetInput.hide();
        widgetSubmit.hide();

        widgetTitle
            .unbind('click')
            .bind('click', function() {
                widgetTitle.hide();
                widgetInput
                    .show()
                    .attr('value', widgetTitle.text())
                    .unbind('keypress')
                    .bind('keypress', function(e) {
                        if (e.which == 13) {
                            widgetSubmit.click();
                            return false;
                        }
                    });

                widgetSubmit
                    .show()
                    .unbind('click')
                    .bind('click', function() {
                        var newTitle = widgetInput.attr('value');
                        widgetSubmit.hide()
                        widgetTitle.text(newTitle).show();
                        widgetInput.hide();

                        Dropthings.Web.Framework.WidgetService.ChangeWidgetTitle(widgetInstanceId, newTitle);
                        return false;
                    });

                return false;
            });

        //Expand/Collaspe widget

        var widgetCollaspe = widget.find('.widget_min');
        var widgetExpand = widget.find('.widget_expand');
        var widgetResizeFrame = widget.find('.widget_resize_frame');
        var widgetCloseButton = widget.find('.widget_close');

        if (Boolean.parse(widget.attr(DropthingsUI.Attributes.EXPANDED))) {
            widgetCollaspe.show();
            widgetExpand.hide();
        }
        else {
            widgetCollaspe.hide();
            widgetExpand.show();
        }

        widgetCloseButton
            .unbind('click')
            .bind('click', function() {
                widget.hide('slow');
                eval(widgetCloseButton.attr("href"));
                return false;
            });

        widgetExpand
	        .unbind('click')
            .bind('click', function() {
                widgetResizeFrame.show();
                widgetExpand.hide();
                widgetCollaspe.show();
                eval(widgetExpand.attr("href"));
                // Asynchronously notify server that widget expanded
                //DropthingsUI.Actions.expandWidget(widgetId, $(this).attr('href'));
                return false;
            });
        //.attr('href', nohref);


        widgetCollaspe
		    .unbind('click')
            .bind('click', function() {
                widgetResizeFrame.hide();
                widgetExpand.show();
                widgetCollaspe.hide();
                eval(widgetCollaspe.attr("href"));
                // Asynchronously notify server that widget collapsed
                //DropthingsUI.Actions.collaspeWidget(widgetId, $(this).attr('href'));
                return false;
            });

        //Maximize/Minimize widget
        var widgetRestore = widget.find('.widget_restore');
        var widgetMaximize = widget.find('.widget_max');

        if (Boolean.parse(widget.attr(DropthingsUI.Attributes.MAXIMIZED))) {
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
            .bind('click', function() {
                widgetMaximize.hide();
                widgetRestore.show();
                eval(widgetMaximize.attr("href"));
                // Asynchronously notify server that widget maximized
                //DropthingsUI.Actions.maximizeWidget(widgetId);
                return false;
            });

        widgetRestore
		    .unbind('click')
            .bind('click', function() {
                widgetMaximize.show();
                widgetRestore.hide();
                eval(widgetRestore.attr("href"));
                // Asynchronously notify server that widget restored
                //DropthingsUI.Actions.restoreWidget(widgetId);
                return false;
            });

    },
    initWidgetActions: function(zoneDivId, widgetClass) {

        //var widgets = $('.widget', '#' + zoneDivId);
        var widgets = $('.' + widgetClass, '#' + zoneDivId);

        widgets.each(function() {
            var widget = $(this);
            var widgetId = widget.attr('id');
            DropthingsUI.setActionOnWidget(widgetId);
        })
    },
    //    refreshSortable: function(zoneId) {
    //        var zone = $('#' + zoneId);
    //        zone.sortable('refresh');
    //    },
    initDragDrop: function(zoneId, widgetClass, newWidgetClass, handleClass, zoneClass, zonePostbackTrigger) {
        // Get all widget zones on the page and allow widget to be dropped on any of them
        var allZones = $('.' + zoneClass);

        var zone = $('#' + zoneId);
        zone.each(function() {
            var plugin = $(this).data('sortable');
            if (plugin) plugin.destroy();
        });

        zone.sortable({
            //items: '> .widget:not(.nodrag)',
            items: '.' + widgetClass + ':not(.nodrag)',
            //handle: '.widget_header',
            handle: '.' + handleClass,
            cursor: 'move',
            appendTo: 'body',
            connectWith: allZones,
            placeholder: 'placeholder',
            start: function(e, ui) {
                ui.helper.css("width", ui.item.parent().outerWidth());
                ui.placeholder.height(ui.item.height());

                DropthingsUI.suspendPendingWidgetZoneUpdate();
            },
            change: function(e, ui) {
                if (ui.element) {
                    var w = ui.element.width();
                    ui.placeholder.width(w);
                    ui.helper.css("width", w);

                    if (ui.item != undefined) {
                        ui.placeholder.height(ui.item.height());
                    }
                    else {
                        //this is a new item from galarry
                        ui.placeholder.height(200);
                    }
                }
            },
            stop: function(e, ui) {
                var position = ui.item.parent()
                                    .children()
                                    .index(ui.item);

                var widgetZone = ui.item.parents('.' + zoneClass + ':first');
                var containerId = parseInt(widgetZone.attr(DropthingsUI.Attributes.ZONE_ID));

                if (ui.item.hasClass(newWidgetClass)) {
                    //new item has been dropped into the sortable list
                    var widgetId = ui.item.attr('id').match(/\d+/);

                    // OMAR: Create a summy widget placeholder while the real widget loads
                    var templateData = { title: $(ui.item).text() };
                    var widgetTemplateNode = $("#new_widget_template").clone();
                    widgetTemplateNode.drink(templateData);
                    widgetTemplateNode.insertBefore(ui.item);

                    DropthingsUI.Actions.onWidgetAdd(widgetId[0], containerId, position,
                        function() {
                            DropthingsUI.updateWidgetZone(widgetZone);
                        });
                }
                else {
                    ui.item.css({ 'width': 'auto' });
                    var instanceId = parseInt(ui.item.attr(DropthingsUI.Attributes.INSTANCE_ID));
                    DropthingsUI.Actions.onDrop(containerId, instanceId, position, function() {
                        DropthingsUI.updateWidgetZone(widgetZone);
                    });
                }
            }
        })
        .bind("sortreceive", function(e, ui) {
            var widget = $(ui.item);
            if (widget.hasClass(newWidgetClass)) {
                widget.remove();
            }
        });
    },
    initResizer: function(divId) {
        $('#' + divId)
            .resizable(
            {
                handles: 's',
                resize: function(e, ui) {
                    if (!ui.options.handles['w'] && !ui.options.handles['e']) {
                        var widget = ui.element.parent().parent();
                        if (Boolean.parse(widget.attr(DropthingsUI.Attributes.MAXIMIZED))) {
                            //$('#widgetMaxBackground').css({'height':$('#widgetMaxBackground').height() + (ui.element.height() - ui.originalSize.height) });
                        }
                        else {
                            ui.element.css({ 'width': 'auto' });
                        }
                    }
                },
                stop: function(e, ui) {
                    if (!Boolean.parse(ui.element.parent().parent().attr(DropthingsUI.Attributes.MAXIMIZED))) {
                        DropthingsUI.Actions.resizeWidget(ui.element.parent().parent(), ui.element.height());
                    }
                }
            });
    },
    initAddStuff: function(zoneClass, newWidgetClass) {
        $(document).ready(function() {
            var allZones = $('.' + zoneClass);
            $('.' + newWidgetClass).draggable("destroy");
            $('.' + newWidgetClass).draggable(
            {
                connectToSortable: allZones,
                helper: 'clone',
                start: function() {
                    $(this).click(function() { return false; });
                },
                stop: function() {
                    $(this).click(function() { return true; });
                }
            });
        });
    },
    updateWidgetZone: function(widgetZone) {
        // OMAR: update the widget zone after three seconds because user might drag & drop another widget
        // in the meantime.
        if ((DropthingsUI.__updateWidgetZoneTimerId || 0) == 0) {
            var zoneList = DropthingsUI.__widgetZonesToUpdate || new Array;
            zoneList.push(widgetZone);
            DropthingsUI.__widgetZonesToUpdate = zoneList;
            widgetZone.attr("__pendingUpdate", "1");
            DropthingsUI.__updateWidgetZoneTimerId = window.setTimeout(function() {
                $(DropthingsUI.__widgetZonesToUpdate).each(function(index, zone) {
                    if (zone.attr("__pendingUpdate") == "1") {
                        zone.attr("__pendingUpdate", "0");
                        var f = function() { return Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack(); }
                        Utility.untilFalse(f, function() {
                            DropthingsUI.asyncPostbackWidgetZone(zone);
                        });
                    }
                });
                DropthingsUI.__updateWidgetZoneTimerId = 0;
            }, 3000);
        }
        else {
            // Restart the timer when another update is queued
            DropthingsUI.suspendPendingWidgetZoneUpdate();
            DropthingsUI.updateWidgetZone(widgetZone);
        }
    },
    suspendPendingWidgetZoneUpdate: function() {
        if (DropthingsUI.__updateWidgetZoneTimerId > 0) {
            window.clearTimeout(DropthingsUI.__updateWidgetZoneTimerId);
            DropthingsUI.__updateWidgetZoneTimerId = 0;
        }
    },
    asyncPostbackWidgetZone: function(widgetZone) {
        var postBackLink = widgetZone.parent().find("." + DropthingsUI.Attributes.WIDGET_ZONE_DUMMY_LINK);
        eval(postBackLink.attr('href'));
    },
    showWidgetGallery: function() {
        $('#Widget_Gallery').show("slow");
    },
    hideWidgetGallery: function() {
        $('#Widget_Gallery').hide("slow");
    },
    Actions: {
        deleteWidget: function(instanceId) {
            Dropthings.Web.Framework.WidgetService.DeleteWidgetInstance(instanceId);
            $(DropthingsUI.getWidgetDivId(instanceId)).remove();
        },

        maximizeWidget: function(widgetId) {
            //var widgetId = DropthingsUI.getWidgetDivId(instanceId);
            var widget = $('#' + widgetId);
            widget.attr(DropthingsUI.Attributes.MAXIMIZED, 'true');

            //if collaspe then expand it
            if (!Boolean.parse(widget.attr(DropthingsUI.Attributes.EXPANDED)))
                widget.find('.widget_expand').click();

            if (null != DropthingsUI._LastMaximizedWidget) {
                DropthingsUI._LastMaximizedWidget.restorePreviouslyMaximizedWidget();
                DropthingsUI._LastMaximizedWidget.dispose();
            }

            DropthingsUI._LastMaximizedWidget = new WidgetMaximizeBehavior(widgetId);
            DropthingsUI._LastMaximizedWidget.maximize();

            Dropthings.Web.Framework.WidgetService.MaximizeWidgetInstance(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
        },

        restoreWidget: function(widgetId) {
            var widget = $('#' + widgetId);

            if (null != DropthingsUI._LastMaximizedWidget) DropthingsUI._LastMaximizedWidget.dispose();
            DropthingsUI._LastMaximizedWidget = new WidgetMaximizeBehavior(widgetId);
            DropthingsUI._LastMaximizedWidget.restore();

            //DropthingsUI.initDragDrop();
            Dropthings.Web.Framework.WidgetService.RestoreWidgetInstance(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            return false;
        },

        collaspeWidget: function(widgetId, postbackUrl) {
            var widget = $('#' + widgetId);
            widget.attr(DropthingsUI.Attributes.EXPANDED, 'false');

            var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
            Dropthings.Web.Framework.WidgetService.CollaspeWidgetInstance(instanceId, postbackUrl, DropthingsUI.Actions._onCollaspeWidgetComplete);

            var maximized = Boolean.parse(widget.attr(DropthingsUI.Attributes.MAXIMIZED));

            if (maximized) {
                if (null != DropthingsUI._LastMaximizedWidget) {
                    $('#widgetMaxBackground').css('height', DropthingsUI._LastMaximizedWidget.visibleHeightIfWidgetCollasped);
                }
            }
        },

        _onCollaspeWidgetComplete: function(postbackUrl) {
            eval(postbackUrl);
        },

        expandWidget: function(widgetId, postbackUrl) {
            var widget = $('#' + widgetId);
            widget.attr(DropthingsUI.Attributes.EXPANDED, 'true');

            var instanceId = widget.attr(DropthingsUI.Attributes.INSTANCE_ID);
            Dropthings.Web.Framework.WidgetService.ExpandWidgetInstance(instanceId, postbackUrl, DropthingsUI.Actions._onExpandWidgetComplete);

            var maximized = Boolean.parse(widget.attr(DropthingsUI.Attributes.MAXIMIZED));
            if (maximized) {
                if (null != DropthingsUI._LastMaximizedWidget) {
                    $('#widgetMaxBackground').css('height', DropthingsUI._LastMaximizedWidget.visibleHeightIfWidgetExpanded);
                }
            }
        },

        _onExpandWidgetComplete: function(postbackUrl) {
            eval(postbackUrl);
        },

        resizeWidget: function(widget, resizeHeight) {
            var maximized = Boolean.parse(widget.attr(DropthingsUI.Attributes.MAXIMIZED));
            var instanceId = parseInt(widget.attr(DropthingsUI.Attributes.INSTANCE_ID));
            if (!maximized) {
                Dropthings.Web.Framework.WidgetService.ResizeWidgetInstance(instanceId, 0, resizeHeight);
                widget.attr(DropthingsUI.Attributes.RESIZED, "true");
                widget.attr(DropthingsUI.Attributes.HEIGHT, resizeHeight);
            }

            return false;
        },

        deletePage: function(pageId) {
            Dropthings.Web.Framework.PageService.DeletePage(pageId, DropthingsUI.Actions._onDeletePageComplete);
            $('#Tab' + pageId).remove();
        },

        _onDeletePageComplete: function(currentPageName) {
            document.location.href = '?' + encodeURI(currentPageName);            
        },

        changePageLayout: function(newLayout) {
            Dropthings.Web.Framework.PageService.ChangePageLayout(newLayout, DropthingsUI.Actions._onChangePageLayoutComplete);
        },

        _onChangePageLayoutComplete: function(arg) {
            document.location.reload();
        },

        newPage: function(newLayout) {
            Dropthings.Web.Framework.PageService.NewPage(newLayout, DropthingsUI.Actions._onNewPageComplete);
        },

        _onNewPageComplete: function(newPageName) {
            document.location.href = '?' + encodeURI(newPageName);
            //__doPostBack('UpdatePanelTabAndLayout','');
        },

        renamePage: function(newLabel) {
            var newPageName = document.getElementById(newLabel).value;
            Dropthings.Web.Framework.PageService.RenamePage(newPageName, DropthingsUI.Actions._onRenamePageComplete);
        },

        _onRenamePageComplete: function() {
            __doPostBack('TabUpdatePanel', '');
        },

        changePage: function(pageId, pageName) {
            //Dropthings.Web.Framework.PageService.ChangeCurrentPage(pageId, OnChangePageComplete);
            document.location.href = '?' + encodeURI(pageName);
        },

        _onChangePageComplete: function(arg) {
            __doPostBack('UpdatePanelTabAndLayout', '');
        },

        onDrop: function(columnNo, instanceId, row, callback) {
            Dropthings.Web.Framework.WidgetService.MoveWidgetInstance(instanceId, columnNo, row, callback);
        },

        onWidgetAdd: function(widgetId, columnNo, row, callback) {
            Dropthings.Web.Framework.WidgetService.AddWidgetInstance(widgetId, columnNo, row, function() { callback(); });
        },
        hide: function(id) {
            document.getElementById(id).style.display = "none";
        },

        showHelp: function() {
            var request = new Sys.Net.WebRequest();
            request.set_httpVerb("GET");
            request.set_url('help.aspx');
            request.add_completed(function(executor) {
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
        if (typeof e == "object") e.style.display = "none"; else if ($get(e) != null) $get(e).style.display = "none";
    },
    // change to display:block
    display: function(e, inline) {
        if (typeof e == "object") e.style.display = (inline ? "inline" : "block"); else if ($get(e) != null) $get(e).style.display = (inline ? "inline" : "block");
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
        while (element != null) {
            position += element["offset" + positionType];
            element = element.offsetParent;
        }

        return position;
    },


    blockUI: function() {
        Utility.display('blockUI');
        var blockUI = $get('blockUI');

        if (blockUI != null) // it will be null if called from CompactFramework
            blockUI.style.height = Math.max(Utility.getContentHeight(), 1000) + "px";
    },

    unblockUI: function() {
        Utility.nodisplay('blockUI');
    },

    untilTrue: function(test, callback) {
        if (test() === true)
            callback();
        else
            window.setTimeout(function() { Utility.untilTrue(test, callback); }, 200);
    },
    
    untilFalse: function(test, callback) {
        Utility.untilTrue(function(){ return test() === false }, callback);
    }
};

var DeleteWarning =
{
    yesCallback : null,
    noCallback : null,
    _initialized : false,
    init : function()
    {
        if( DeleteWarning._initialized ) return;
        
        var hiddenHtmlTextArea = $get('DeleteConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        DeleteWarning._initialized = true;
    },
    show : function( yesCallback, noCallback )
    {
        DeleteWarning.init();
        
        Utility.blockUI();
        
        var popup = $get('DeleteConfirmPopup');
        Utility.display(popup);
        
        DeleteWarning.yesCallback = yesCallback;
        DeleteWarning.noCallback = noCallback;
        
        $addHandler( $get("DeleteConfirmPopup_Yes"), 'click', DeleteWarning._yesHandler );
        $addHandler( $get("DeleteConfirmPopup_No"), 'click', DeleteWarning._noHandler );
    },
    hide : function()
    {
        DeleteWarning.init();
        
        var popup = $get('DeleteConfirmPopup');
        Utility.nodisplay(popup);
        
        $clearHandlers( $get('DeleteConfirmPopup_Yes') );
        
        Utility.unblockUI();
        
    },
    _yesHandler : function()
    {
        DeleteWarning.hide();
        DeleteWarning.yesCallback();    
    },
    _noHandler : function()
    {
        DeleteWarning.hide();
        DeleteWarning.noCallback();
    }
};

var DeletePageWarning =
{
    yesCallback : null,
    noCallback : null,
    _initialized : false,
    init : function()
    {
        if( DeletePageWarning._initialized ) return;
        
        var hiddenHtmlTextArea = $get('DeletePageConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        DeletePageWarning._initialized = true;
    },
    show : function( yesCallback, noCallback )
    {
        DeletePageWarning.init();
        
        Utility.blockUI();
        
        var popup = $get('DeletePageConfirmPopup');
        Utility.display(popup);
        
        DeletePageWarning.yesCallback = yesCallback;
        DeletePageWarning.noCallback = noCallback;
        
        $addHandler( $get("DeletePageConfirmPopup_Yes"), 'click', DeletePageWarning._yesHandler );
        $addHandler( $get("DeletePageConfirmPopup_No"), 'click', DeletePageWarning._noHandler );
    },
    hide : function()
    {
        DeletePageWarning.init();
        
        var popup = $get('DeletePageConfirmPopup');
        Utility.nodisplay(popup);
        
        $clearHandlers( $get('DeletePageConfirmPopup_Yes') );
        
        Utility.unblockUI();
        
    },
    _yesHandler : function()
    {
        DeletePageWarning.hide();
        DeletePageWarning.yesCallback();    
    },
    _noHandler : function()
    {
        DeletePageWarning.hide();
        DeletePageWarning.noCallback();
    }
};

function winopen(url, w, h) 
{
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

  window.open(url, "_blank", "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  
  return;
}

function winopen_withlocationbar(url) 
{
 var w = screen.width / 2;
 var h = screen.height /2;
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

  window.open(url, "_blank");
  
  return;
}

function winopen2(url,target, w, h) 
{
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

 if(popupWin_2[target] != null)
	if(!popupWin_2[target].closed)
		popupWin_2[target].focus();
	else
		popupWin_2[target] = window.open(url, target, "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  else
	popupWin_2[target] = window.open(url, target, "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  
  return;
}


var LayoutPicker =
{
    yesCallback : null,
    noCallback : null,
    type1Callback:null,
    type2Callback:null,
    type3Callback:null,
    type4Callback:null,
    _initialized : false,
    clientID :null,
    init : function()
    {
        if( LayoutPicker._initialized ) return;

        var hiddenHtmlTextArea = $get('LayoutPickerPopupPlaceholder');
        
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        LayoutPicker._initialized = true;
    },
    show : function( Type1Callback,Type2Callback,Type3Callback,Type4Callback, noCallback, clientID )
    {   
        LayoutPicker.init();
        
        Utility.blockUI();
        
        var popup = $get('LayoutPickerPopup');
        Utility.display(popup);
        
        LayoutPicker.type1Callback= Type1Callback;
        LayoutPicker.type2Callback= Type2Callback;
        LayoutPicker.type3Callback= Type3Callback;
        LayoutPicker.type4Callback= Type4Callback;
        LayoutPicker.clientID = clientID;
        LayoutPicker.noCallback = noCallback;
        
        $addHandler( $get("SelectLayoutPopup_Cancel"), 'click', LayoutPicker._noHandler );
        $addHandler( $get("SelectLayoutPopup_Type1"), 'click', LayoutPicker._type1Handler );
        $addHandler( $get("SelectLayoutPopup_Type2"), 'click', LayoutPicker._type2Handler );
        $addHandler( $get("SelectLayoutPopup_Type3"), 'click', LayoutPicker._type3Handler );
        $addHandler( $get("SelectLayoutPopup_Type4"), 'click', LayoutPicker._type4Handler );
        
    },
    hide : function()
    {
        LayoutPicker.init();

        
        var popup = $get('LayoutPickerPopup');
        Utility.nodisplay(popup);
        //is there a cleaner way to clear the handlers?
        $clearHandlers( $get('SelectLayoutPopup_Type1') );
        $clearHandlers( $get('SelectLayoutPopup_Type2') );
        $clearHandlers( $get('SelectLayoutPopup_Type3') );
        $clearHandlers( $get('SelectLayoutPopup_Type4') );
        
        Utility.unblockUI();
        
    },

    _type1Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type1Callback();  
    },
    _type2Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type2Callback();    
    },
    _type3Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type3Callback();
    },
    _type4Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type4Callback();
    },

    _noHandler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.noCallback();
    }
};

/*
function CreateResizeExtender(instanceId)
{
     var _resizeHandler = Function.createDelegate(this, CreateResizeExtender1(instanceId));
        $addHandler(window, 'load', this._resizeHandler);
}
function CreateResizeExtender1(instanceId)
{        
    var widgetContainerId = "WidgetPanelsLayout_WidgetContainer" + instanceId ;
    var widgetPanel = $get(widgetContainerId + '_Widget'); 
}
*/

//var ResizeWidget =
//{
//    _resizableItemClassValue: 'widget',
//    init: function() {
//        ResizeWidget._initializeResizableItems();
//        //window.setTimeout(ResizeWidget._initializeResizableItems, 2000);

//        //var resizeHandler = Function.createDelegate(this, this._initializeResizableItems);
//        //$addHandler(window, 'resize', resizeHandler);

//    },
//    _initializeResizableItems: function() {
//        var widgetPanels = new Array($get('WidgetPanelsLayout_LeftPanel'), $get('WidgetPanelsLayout_MiddlePanel'), $get('WidgetPanelsLayout_RightPanel'));

//        for (var i = 0; i < widgetPanels.length; i++) {
//            var el = widgetPanels[i];

//            if (el == null)
//                return null;

//            var child = el.firstChild;
//            while (child != null) {
//                if (ResizeWidget._isItemResizable(child, ResizeWidget._resizableItemClassValue)) {
//                    ResizeWidget.attachResizer(child);
//                }
//                child = child.nextSibling;
//            }
//        }
//        var el = $get('WidgetContainer');

//    },
//    _isItemResizable: function(item, className) {
//        var regEx = new RegExp('(?:^|\\s+)' + className + '(?:\\s+|$)');
//        return regEx.test(item.className);
//    },
//    attachResizer: function(item) {
//        return;
//        
//        var rceId = item.id + '_ResizableControlExtender';
//        var rce = $find(rceId);

//        if (rce) {
//            rce.dispose();
//            Sys.Application.removeComponent(rce);
//        }

//        var isResized = Boolean.parse(item.getAttribute(DropthingsUI.Attributes.RESIZED));
//        var isMaximized = Boolean.parse(item.getAttribute(DropthingsUI.Attributes.MAXIMIZED));
//        var isExpanded = Boolean.parse(item.getAttribute(DropthingsUI.Attributes.EXPANDED));

//        rce = $create(Dropthings.ResizingWidgetBehavior, { "IsResized": isResized, "ClientStateFieldID": rceId + "_ClientState", "HandleCssClass": "widget_resize_handler", "MinimumHeight": 39, "MaximumWidth": item.offsetWidth - 2, "MinimumWidth": item.offsetWidth - 2, "id": rceId }, null, null, item);
//        
//        /*    
//        if (isMaximized && isExpanded) {
//            rce.set_Size({ width: -1, height: item.offsetHeight });
//        }
//        */
//        if (!isMaximized && isResized && isExpanded) {
//            rce.set_Size({ width: -1, height: item.getAttribute(DropthingsUI.Attributes.HEIGHT) });
//        }
//        /*
//        else if (!isMaximized && isExpanded) {
//            rce.set_Size({ width: -1, height: item.offsetHeight });
//        }*/

//        if (rce) {
//            rce.set_resize("DropthingsUI.Actions.resizeWidget");
//        }
//    },
//    removeResizer: function(item) {
//        var rceId = item.id + '_ResizableControlExtender';
//        var rce = $find(rceId);

//        if (rce) {
//            rce.dispose();
//            Sys.Application.removeComponent(rce);
//        }
//    }
//};

var WidgetMaximizeBehavior = function(widgetId) {
    this.visibleHeightIfWidgetExpanded = 0
    this.visibleHeightIfWidgetCollasped = 0
    this.init = function(widgetId) {
        if (this.initialized) return;
        
        this.widget =  $('#' + widgetId);
        //this.widgetSelector = DropthingsUI.getWidgetDivId(instanceId);
        this.instanceId = this.widget.attr(DropthingsUI.Attributes.INSTANCE_ID);;


        //_widgetMaxBackground = $('widgetMaxBackground');
        //_maximizeButton = $(widgetContainerId + '_MaximizeWidget');
        //_restoreButton = $(widgetContainerId + '_RestoreWidget');

        this.initialized = true;
    }

    this.init(widgetId);

    this.dispose = function() {
        this.initialized = false;
        this.instanceId = 0;
        this.widget = null;
    }

    this.maximize = function() {
        this.removeFloatingBehavior();
        this.fitToViewPort();
        window.scrollTo(0, 0);
    }

    this.restorePreviouslyMaximizedWidget = function() {
        if (this.initialized) {
            this.widget.find('.widget_restore').click();
        }
    }

    this.restore = function() {
        this.addFloatingBehavior();
        $('.widget_max_holder').css({ display: 'none', height: 'auto' });

        var height = Boolean.parse(this.widget.attr( DropthingsUI.Attributes.RESIZED )) ? this.widget.attr( DropthingsUI.Attributes.HEIGHT ) + 'px' : 'auto';
        this.widget.css({ left: 'auto', top: 'auto', position: 'static' })
                    .removeClass('widget_max_content')
                    .attr(DropthingsUI.Attributes.MAXIMIZED, 'false')

        this.widget.find('.widget_resize_frame').each(function() {
            $(this)
                        .css({ width: 'auto' })
                        .css({height: height});

        });

        this.restoredInstanceId = this.instanceId;
        this.dispose();
    }

    this.fitToViewPort = function() {
        var maxBackground = $('.widget_max_holder');
        var visibleHeight = (Utility.getViewPortHeight() - Utility.getAbsolutePosition(maxBackground[0], 'Top'));

        if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
            visibleHeight -= 20;
        }
        maxBackground.css({ display: 'block', height: visibleHeight });

        this.widget.css({ zIndex: '10000',
            position: 'absolute',
            left: maxBackground.offset().left,
            top: maxBackground.offset().top
        }).addClass('widget_max_content');

        var isExpanded = Boolean.parse(this.widget.attr(DropthingsUI.Attributes.EXPANDED));
        var WidgetResizeFrame = this.widget.find('.widget_resize_frame');

        this.visibleHeightIfWidgetExpanded = visibleHeight;
        this.visibleHeightIfWidgetCollasped = 40;

         
        if (isExpanded) {
           WidgetResizeFrame.height((this.visibleHeightIfWidgetExpanded - 40) + 'px');
        }
        else {
            WidgetResizeFrame.height(this.visibleHeightIfWidgetCollasped + 'px');//this is a mim hieght
        }
    }

    this.removeFloatingBehavior = function() {
        this.widget.addClass('nodrag')
                    .attr('onmouseover', "this.className='widget nodrag widget_hover'")
                    .attr('onmouseout', "this.className='widget nodrag'");
    }

    this.addFloatingBehavior = function() {
        this.widget.removeClass('nodrag')
                    .attr("onmouseover", "this.className='widget widget_hover'")
                    .attr("onmouseout", "this.className='widget'");
    }
};
var WidgetPermission =
{
    Save: function() {
        WidgetPermission.showProgress(false);
        $('#Message').html('');
        var widgets = $('.widgetItem');
        var nameValuePair = '';

        widgets.each(function() {
            var widget = $(this);
            var widgetId = widget.attr('id').match(/\d+/);
            var roles = $('.role', '#' + widget.attr('id'));
            nameValuePair += widgetId[0] + ":";
            var roleStr = '';
            roles.each(function() {
                chkRole = $(this);
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
                                                                    $('#Message').html("Permission assigned successfully.");
                                                                });

    },
    showProgress: function(hide) {
        if (hide)
            $('#progress').hide();
        else
            $('#progress').show();
    }
};
function pageUnload() {

}