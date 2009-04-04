/* Copyright (c) 2008 DIYism (email/msn/gtalk:kexianbin@diyism.com web:http://diyism.com)
 * Licensed under GPL (http://www.opensource.org/licenses/gpl-license.php) license.
 *
 * Version: k88r
 * Requires jQuery 1.1.3+
 * Docs: http://ejohn.org/blog/javascript-micro-templating/
 */
;(function($) {

$.fn.drink = function(json)
             {if (!(arguments.callee.name_class = this[0].className))
                 {this.each(function(){json[this.id] && (this.innerHTML=json[this.id]);});
                  return;
                 }
              var name_tpl_fun_cache = 'tpl_fun_cache_'+arguments.callee.name_class;
              if (!window[name_tpl_fun_cache])
                 {var tpl=unescape(this.html())
                          .replace(/<!--|&lt;!|\/\*/g, "<!")
                          .replace(/-->|!&gt;|\*\//g, "!>")
                          .replace(/\r|\*="/g, ' ')
                          .split('<!').join("\r")
                          .replace(/(?:^|!>)[^\r]*/g, function(){return arguments[0].replace(/'|\\/g, "\\$&").replace(/\n/g, "\\n");})
                          .replace(/\r=(.*?)!>/g, "',$1,'")
                          .split("\r").join("');")
                          .split('!>').join("write.push('")
                          .replace(/!\,/g, ',');
                  window[name_tpl_fun_cache]
                  =new Function('json',
                                "var write=[];with (json){write.push('"+tpl+"');}return write.join('');"
                               );
                 }
              if (!json)
                 {return;
                 }
              this.html(window[name_tpl_fun_cache](json));
              arguments[1] || this.show();
             };

})(jQuery);