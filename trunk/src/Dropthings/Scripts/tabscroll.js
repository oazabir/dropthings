; (function($) {
		
	// plugin initialization
	$.fn.extend({
		scrollable: function(arg1, arg2, arg3) { 
			return this.each(function() {
				if (typeof arg1 == "string") {
					var el = $.data(this, "scrollable");
					el[arg1].apply(el, [arg2, arg3]);
					
				} else { 
					new $.scrollable(this, arg1, arg2);
				}
			});
		}		
	});
		
	// constructor
	$.scrollable = function(el, opts) {   
		// store this instance
		$.data(el, "scrollable", this);
		
		this.init(el, opts); 
	};
	
	
	// methods
	$.extend($.scrollable.prototype, { 
			
		init: function(el, config)  {
			// current instance
			var self = this;  
			
			var opts = {								
				speed: 300,
				scrollRepeatInterval: 230,
				
				// jquery selectors
				activeClass:'.activetab',
				itemRootClass: '.tab_container_header',
				stripClass: '.tab-strip',
				scrollerLeft:'.scrollerLeft',
				scrollerRight:'.scrollerRight',
				tabOptions: '.tab_container_options',
				newTab: '.newtab'
			}; 
	
			this.opts = $.extend(opts, config); 			
	
			// root / itemRoot
			var root = this.root = $(el);//<div id="tab_container" class="tab_container">
			var itemRoot = this.itemRoot = $(opts.itemRootClass);//<div class="tab_container_header" id="tabHeader">
				
			// wrap itemRoot.children() inside container
			itemRoot.children(":first").wrapAll('<div class="tabwrap" id="tabs" />');
			
			this.stripWrap = itemRoot.children(":first");//<div class="tabwrap" id="tabs" />
			
			var strip = this.strip = $(opts.stripClass);//<ul class="tabs tab-strip" runat="server" id="tabList">
            this.items = strip.children(':not(.newtab)');
            strip.append('<li class="tab-edge" />');
            this.edge = strip.children(":last");            
            
            $(opts.newTab).insertAfter(this.edge);
            self.autoScrollTabs();			
		},
		
        // private
        autoScrollTabs : function(){
            var count = this.items.length;
            var tw = this.itemRoot.outerWidth();

            var wrap = this.stripWrap;
            var wd = wrap[0];
            var pos = this.getScrollPos();
            var l = this.edge.offset().left - this.stripWrap.offset().left;

            if(l <= tw){
                //wd.animate({scrollLeft: 100}, this.opts.speed);
                wrap.width(tw);
                if(this.scrolling){
                    this.scrolling = false;
                    this.itemRoot.removeClass('tab-scrolling');
                }
            }else{
                if(!this.scrolling){
                    this.itemRoot.addClass('tab-scrolling');
                }
                wrap.width(tw);
                $(this.opts.newTab).remove();
                $('.newtabscrolling').show();
                if(!this.scrolling){
                if(!this.scrollLeft){
                    this.createScrollers();
                }else{
                    this.scrollerLeft.show();
                    this.scrollerRight.show();
                }
            }
            this.scrolling = true;
            //this.updateScrollButtons(pos);
            
            if(pos > (l-tw)){ // ensure it stays within bounds
                wd.animate({scrollLeft: l-tw}, this.opts.speed);
            }else{ // otherwise, make sure the active tab is still visible
                this.scrollToItem($(this.opts.activeClass));
            }
        }
     },
     createScrollers : function(){
        var self = this;
        this.root.find(this.opts.tabOptions).append('<div class="tab-scroller-left" />')
                 .find('div.tab-scroller-left')
                 .height(this.itemRoot.height())
                 .bind('click', function() {
					self.onSnapLeft();
				 })
				 .bind('mousedown', function() {
				    self.intervalID = window.setInterval(function() {self.onScrollLeft();}, self.opts.scrollRepeatInterval);
				    return false;
				 })
				 .bind('mouseup', function() {
					window.clearInterval(self.intervalID);
					return false;
				 });
				 
				 
		this.scrollerLeft = this.root.find('div.tab-scroller-left');
	    
	    this.root.find(this.opts.tabOptions).append('<div class="tab-scroller-right" />')
                 .find('div.tab-scroller-right')
                 .height(this.itemRoot.height())
                 .bind('click', function() {
					self.onSnapRight();		
				 })
				 .bind('mousedown', function() {
				    self.intervalID = window.setInterval(function() {self.onScrollRight();}, self.opts.scrollRepeatInterval);
				    return false;
				 })
				 .bind('mouseup', function() {
					window.clearInterval(self.intervalID);
					return false;
				 });
				 
	    this.scrollerRight = this.root.find('div.tab-scroller-right');
	},
    
    onScrollRight : function(){
        var sw = this.getScrollWidth()-this.getScrollArea();//sw = pos i.e scroll left width + right hidden portion
        var pos = this.getScrollPos() + this.itemRoot.offset().left;
        var s = Math.min(sw, pos + this.getScrollIncrement());
        if(pos + this.getScrollIncrement() <= sw){
            this.scrollTo(s);
        }
    },

    onScrollLeft : function(){
        var pos = this.getScrollPos();
        var s = Math.max(0, pos - this.getScrollIncrement());
        if(s != pos){
            this.scrollTo(s);
            this.currentItemSnapedOnLeft = -1;
        }
    },
    
    onSnapRight : function(){
        var self = this;
        var pos = self.getScrollPos(), area = this.itemRoot.outerWidth();
        var left = area + pos - this.itemRoot.offset().left;	
        var itemsWidth =  0, index = -1;
        this.items.each(function(i) {
                var item = $(this);
                itemsWidth += item.outerWidth();
		        if(itemsWidth >= left){
		            index = i;
		            return false;
		        }
			});
		if(index > -1){
		    self.scrollToItem(self.items.eq(index));
		}
		else{
		    self.scrollToItem(self.items.eq(self.items.length - 1));
		}
    },
    
    onSnapLeft : function(){
        var self = this;
        var pos = self.getScrollPos() + this.itemRoot.offset().left;	
        var itemsWidth = 0, index = -1;//
        this.items.each(function(i) {
                var item = $(this);
                itemsWidth += item.innerWidth();                        
		        if(itemsWidth > pos){
		            index = i;
	                if(index > 0 && self.currentItemSnapedOnLeft == index){index--;}
	                self.currentItemSnapedOnLeft = index;
	                return false;
		        }
			});
			
		if(index > -1){
		 self.scrollToItem(self.items.eq(index));
		}
    },

    getScrollWidth : function(){
        return this.edge.offset().left + this.getScrollPos();
    },

    getScrollPos : function(){
        return parseInt(this.stripWrap.scrollLeft(), 10) ||0;
    },

    getScrollArea : function(){
        return parseInt(this.stripWrap.outerWidth(), 10) || 0;
    },

    getScrollIncrement : function(){
        return this.scrollIncrement || 96;
    },

    scrollToItem : function(item){        
        if(!item){ return; }
        var pos = this.getScrollPos(), area = this.getScrollArea();
        var left = item.offset().left + pos - this.stripWrap.offset().left;
        var right = left + item.outerWidth();
        if(left < pos){
            this.scrollTo(left);
        }else if(right > (pos + area)){
            this.scrollTo(right - area);
        }
    },

    scrollTo : function(pos){
        this.stripWrap.animate({scrollLeft: pos}, this.opts.speed);
        this.updateScrollButtons(pos);
    },

    updateScrollButtons : function(pos){
        var sw = this.getScrollWidth() - this.getScrollArea();
        if(pos == 0){
            window.clearInterval(this.intervalID);
            this.scrollerLeft.addClass('tab-scroller-disabled');
        }
        else{
             this.scrollerLeft.removeClass('tab-scroller-disabled');
        }
        
        if(pos + this.getScrollIncrement() >= (this.getScrollWidth() - this.getScrollArea() + 49)){
            window.clearInterval(this.intervalID);
            this.scrollerRight.addClass('tab-scroller-disabled');
        }
        else{
             this.scrollerRight.removeClass('tab-scroller-disabled');
        }
    }
	});  
	
})(jQuery);



