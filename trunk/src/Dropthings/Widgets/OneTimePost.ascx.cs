using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Widgets_OneTimePost : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Page.IsPostBack)
        //    this.Visible = false;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (Page.IsPostBack)
            return;

        if (!Page.ClientScript.IsStartupScriptRegistered(typeof(Widgets_OneTimePost), typeof(Widgets_OneTimePost).FullName))
        {
            Page.ClientScript.RegisterStartupScript(typeof(Widgets_OneTimePost), typeof(Widgets_OneTimePost).FullName, string.Format(@"
if (!window.postBackQueue)
    window.postBackQueue = [];

window.postMeBack = function() {{
    var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
    pgRegMgr.remove_endRequest(window.postMeBack);

    if (pgRegMgr.get_isInAsyncPostBack()) {{
        pgRegMgr.add_endRequest(window.postMeBack);                
    }}
    else {{
        if (window.postBackQueue.length > 0) {{
            var item = window.postBackQueue.splice(0, 1);
            //console.log('posting back ' + item);
            document.getElementById(item).click();             
            pgRegMgr.add_endRequest(window.postMeBack);                
        }}
    }}    
}};

Array.prototype.indexOf = function(obj, start) {{
     for (var i = (start || 0), j = this.length; i < j; i++) {{
         if (this[i] === obj) {{ return i; }}
     }}
     return -1;
}}
", this.ClientID, this.Postback.ClientID), true);
        }

        Page.ClientScript.RegisterStartupScript(typeof(Widgets_OneTimePost), Guid.NewGuid().ToString(), string.Format(@"
Sys.Application.add_init(function() {{
    if (window.postBackQueue.indexOf('{1}')<0) {{
        window.postBackQueue.push('{1}');    
        window.postMeBack();
    }}
}});
", this.ClientID, this.Postback.ClientID), true);
        
    }
}