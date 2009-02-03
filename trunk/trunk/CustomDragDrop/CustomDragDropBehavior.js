// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

Type.registerNamespace('CustomDragDrop');

CustomDragDrop.CustomDragDropBehavior = function(element) {

    CustomDragDrop.CustomDragDropBehavior.initializeBase(this, [element]);
    
    this._DragItemClassValue = null;    
    this._DragItemHandleClassValue = null;
    this._DropCueIDValue = null;
    this._dropCue = null;
    this._floatingBehaviors = [];
}

CustomDragDrop.CustomDragDropBehavior.prototype = {
    
    initialize : function() {
        // Register ourselves as a drop target.
        AjaxControlToolkit.DragDropManager.registerDropTarget(this); 
        //Sys.Preview.UI.DragDropManager.registerDropTarget(this);
        
        // Initialize drag behavior after a while
        window.setTimeout( Function.createDelegate( this, this._initializeDraggableItems ), 1000 );
        
        this._dropCue = $get(this.get_DropCueID());
    },
    
    dispose : function() {
        AjaxControlToolkit.DragDropManager.unregisterDropTarget(this);
        //Sys.Preview.UI.DragDropManager.unregisterDropTarget(this);
        
        this._clearFloatingBehaviors();
        
        CustomDragDrop.CustomDragDropBehavior.callBaseMethod(this, 'dispose');
    },
    
    add_onDrop : function(handler) {
        this.get_events().addHandler("onDrop", handler);
    },
    
    remove_onDrop : function(handler) {
        this.get_events().removeHandler("onDrop", handler);
    },
    
    // onDrop property maps to onDrop event
    get_onDrop : function() {
        return this.get_events().getHandler("onDrop");
    },

    set_onDrop : function(value) {
        if (value && (0 < value.length)) {
            var func = CommonToolkitScripts.resolveFunction(value);
            if (func) { 
                this.add_onDrop(func);
            } else {
                throw Error.argumentType('value', typeof(value), 'Function', 'resize handler not a function, function name, or function text.');
            }
        }
    },
    
    _raiseEvent : function( eventName, eventArgs ) {
        var handler = this.get_events().getHandler(eventName);
        if( handler ) {
            if( !eventArgs ) eventArgs = Sys.EventArgs.Empty;
            handler(this, eventArgs);
        }
    },
    
    _clearFloatingBehaviors : function()
    {
        if( this._floatingBehaviors != null ) {
            while( this._floatingBehaviors.length > 0 )
            {
                var behavior = this._floatingBehaviors.pop();            
                behavior.dispose();
            }
        }
    },
    
    _findChildByClass : function(item, className)
    {
        // First check all immediate child items
        var child = item.firstChild;
        while( child != null )
        {
            if( child.className == className ) return child;
            child = child.nextSibling;
        }
        
        // Not found, recursively check all child items
        child = item.firstChild;
        while( child != null )
        {
            var found = this._findChildByClass( child, className );
            if( found != null ) return found;
            child = child.nextSibling;
        }
    },
    
    _isItemDraggable : function(item, className)
    {
        var regEx = new RegExp('(?:^|\\s+)' + className + '(?:\\s+|$)');
        return regEx.test(item.className);
    },
    
    // Find all items with the drag item class and make each item
    // draggable        
    _initializeDraggableItems : function() 
    {
        this._clearFloatingBehaviors();
        
        var el = this.get_element();
        if (el == null)
         return null;
        
        var child = el.firstChild;
        while( child != null )
        {
            if( this._isItemDraggable(child, this._DragItemClassValue) && child != this._dropCue)
            {
                var handle = this._findChildByClass(child, this._DragItemHandleClassValue);
                if( handle )
                {                        
                    var handleId = handle.id;
                    var behaviorId = child.id + "_WidgetFloatingBehavior";
                    
                    // make the item draggable by adding floating behaviour to it                    
                    var floatingBehavior = $create(CustomDragDrop.CustomFloatingBehavior, 
                            {"DragHandleID":handleId, "id":behaviorId, "name": behaviorId}, {}, {}, child);
                    
                    Array.add( this._floatingBehaviors, floatingBehavior );
                }
            }            
            child = child.nextSibling;
        }
    },
    
    get_DragItemClass : function()
    {
        return this._DragItemClassValue;
    },
    
    set_DragItemClass : function(value)
    {
        this._DragItemClassValue = value;
    },
    
    get_DropCueID : function()
    {
        return this._DropCueIDValue;
    },
    
    set_DropCueID : function(value)
    {
        this._DropCueIDValue = value;
    },
    
    get_DragItemHandleClass : function()
    {
        return this._DragItemHandleClassValue;
    },
    
    set_DragItemHandleClass : function(value)
    {
        this._DragItemHandleClassValue = value;
    },
    
    getDescriptor : function() {
        var td = CustomDragDrop.CustomDragDropBehavior.callBaseMethod(this, 'getDescriptor');
        return td;
    },

    // IDropTarget members.
    get_dropTargetElement : function() {
        return this.get_element();
    },
    
    drop : function(dragMode, type, data) { 
        this._hideDropCue(data);
        this._placeItem(data);
    },
    
    canDrop : function(dragMode, dataType) {
        //return true;
        return dataType == "_CustomFloatingItem";
    },
    
    onDragEnterTarget : function(dragMode, type, data) {
        this._showDropCue(data);    
    },
    
    onDragLeaveTarget : function(dragMode, type, data) {
        this._hideDropCue(data);
    },
    
    onDragInTarget : function(dragMode, type, data) {
        this._repositionDropCue(data);
    },
    
    _findItemAt : function(x,y, item)
    {
        var el = this.get_element();
        if (el == null)
         return null;
        
        var child = el.firstChild;
        while( child != null )
        {
            if( this._isItemDraggable(child, this._DragItemClassValue) && child != this._dropCue && child != item )
            {
                var pos = Sys.UI.DomElement.getLocation(child);
                
                if( y <= pos.y )
                {
                    return child;
                }
            }
            child = child.nextSibling;
        }
        
        return null;
    },
    
    _showDropCue : function(data)
    {        
        this._repositionDropCue(data);
        
        this._dropCue.style.display = "block";
        this._dropCue.style.visibility = "visible";
        
        var bounds = Sys.UI.DomElement.getBounds(data.item);
        
        if( this._dropCue.style.height == "" )
            this._dropCue.style.height = bounds.height.toString() + "px";
        
    },
    
    _hideDropCue : function(data)
    {
        this._dropCue.style.display = "none";
        this._dropCue.style.visibility = "hidden";        
    },
    
    _repositionDropCue : function(data)
    {
        var location = Sys.UI.DomElement.getLocation(data.item);
        var nearestChild = this._findItemAt(location.x, location.y, data.item);
        
        var el = this.get_element();        
            
        if( null == nearestChild )
        {
            if( el.lastChild != this._dropCue )
            {
                el.removeChild(this._dropCue);
                el.appendChild(this._dropCue);
            }
        }
        else
        {
            if( nearestChild.previousSibling != this._dropCue )
            {
                el.removeChild(this._dropCue);
                el.insertBefore(this._dropCue, nearestChild);            
            }            
        }
    },
    
    _placeItem : function(data)
    {
        var el = this.get_element();
                
        data.item.parentNode.removeChild( data.item );
        el.insertBefore( data.item, this._dropCue );
        
        // Find the position of the dropped item
        var position = 0;
        var item = el.firstChild;
        while( item != data.item )
        {  
            if( this._isItemDraggable(item, this._DragItemClassValue ))
            {
                position++; 
            }
            
            item = item.nextSibling; 
        }
        this._raiseDropEvent( /* Container */ el, /* droped item */ data.item, /* position */ position );
    },
    
    
    _raiseDropEvent : function( container, droppedItem, position )
    {
        this._raiseEvent( "onDrop", new CustomDragDrop.DropEventArgs(container, droppedItem, position) );
    }
}

CustomDragDrop.CustomDragDropBehavior.registerClass('CustomDragDrop.CustomDragDropBehavior', 
AjaxControlToolkit.BehaviorBase, 
AjaxControlToolkit.IDragSource, 
AjaxControlToolkit.IDropTarget,
/*Sys.UI.Behavior, 
Sys.Preview.UI.IDragSource, 
Sys.Preview.UI.IDropTarget,*/
Sys.IDisposable);


CustomDragDrop.DropEventArgs = function(container, droppedItem, position) {
    CustomDragDrop.DropEventArgs.initializeBase(this);
    
    this._container = container;
    this._droppedItem = droppedItem;
    this._position = position;
}

CustomDragDrop.DropEventArgs.prototype = {
    get_container : function() {
        return this._container;
    },
    get_droppedItem : function() {
        return this._droppedItem;
    },
    get_position : function() {
        return this._position;
    }
}

CustomDragDrop.DropEventArgs.registerClass("CustomDragDrop.DropEventArgs", Sys.EventArgs);

