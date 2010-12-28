using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;

public partial class Widgets_HtmlWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;

    #endregion Fields

    #region Properties

    private XElement State
    {
        get
        {
            string state = this._Host.GetState();
            if (string.IsNullOrEmpty(state))
                state = "<state></state>";
            if (_State == null) _State = XElement.Parse(state);
            return _State;
        }
    }

    #endregion Properties

    #region Methods

    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    void IWidget.Closed()
    {
    }

    void IWidget.Collasped()
    {
    }

    void IWidget.Expanded()
    {
    }

    void IWidget.HideSettings(bool userClicked)
    {
        SettingsPanel.Visible = false;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.Maximized()
    {
    }

    void IWidget.Restored()
    {
    }

    void IWidget.ShowSettings(bool userClicked)
    {
        if (userClicked)
        {
//            this.HtmltextBox.Text = this.State.Value;
        }
        SettingsPanel.Visible = true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        SaveSettings.OnClientClick = string.Format(SaveSettings.OnClientClick, HtmltextBox.ClientID);

        var html = (this.State.FirstNode as XCData ?? new XCData("")).Value;
        this.Output.Text = html;

        if (SettingsPanel.Visible)
        {
            HtmltextBox.Text = html;
            var scriptsToLoad = new string[] {
                ResolveClientUrl("~/Widgets/HtmlEditorWidget/jHtmlArea-0.7.0.min.js"),
                ResolveClientUrl("~/Widgets/HtmlEditorWidget/jHtmlArea.ColorPickerMenu-0.7.0.min.js")
            };
            var cssToLoad = new string[] {
                ResolveClientUrl("~/Widgets/HtmlEditorWidget/style/jqueryui/ui-lightness/jquery-ui-1.7.2.custom.css"),
                ResolveClientUrl("~/Widgets/HtmlEditorWidget/style/jHtmlArea.css")
            };

            var currentPageCss = (from link in this.Page.Header.Controls.OfType<HtmlLink>()
                                 where link.Href.Contains(".css")
                                 select link.Href).ToArray();
            // jHtmlArea is not supporting multiple CSS yet. So, need to use only one. Use the last theme CSS
            // assuming that will give closest rendering inside the editor as the parent page
            //var cssToUseInsideHtmlEditor = "['" + string.Join("','", currentPageCss) + "']";
            var cssToUseInsideHtmlEditor = ResolveClientUrl((from css in currentPageCss
                                            where css.Contains(this.Page.Theme)
                                            select css).Last());

            WidgetHelper.RegisterWidgetScript(this, scriptsToLoad, cssToLoad, null,
                @"jQuery('#{0}').htmlarea({{ css: '{1}' }}); 
                if (!window.__hijackedDoPostback) 
                {{
                    window.__hijackedDoPostback = window.__doPostBack;
                    window.__doPostBack = function(a,b) 
                    {{
                        try
                        {{
                            jQuery('#{0}').htmlarea('dispose');
                            jQuery('#{0}').remove();
                        }} catch(e) 
                        {{ 
                        }}
                        window.__doPostBack = window.__hijackedDoPostback;
                        window.__hijackedDoPostback = null;
                        window.__doPostBack(a,b);
                    }};
                }}
                ".FormatWith(HtmltextBox.ClientID, cssToUseInsideHtmlEditor));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveSettings_Clicked(object sender, EventArgs e)
    {
        this.State.RemoveAll();

        var html = HttpUtility.UrlDecode(this.HtmltextBox.Text);
        this.State.RemoveAll();
        this.State.Add(new XCData(html));

        this.SaveState();
    }

    protected void CancelSettings_Clicked(object sender, EventArgs e)
    {
        this._Host.HideSettings(true);
    }

    private void SaveState()
    {
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
    }

    #endregion Methods
}