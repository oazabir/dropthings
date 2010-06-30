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

public partial class WidgetPanels : System.Web.UI.UserControl
{
    #region Fields

    private double leftPanelSize;
    private double middlePanelSize;
    private string middlePanelVisible;
    private int padding = 1;
    private double rightPanelSize;
    private string rightPanelVisible;

    #endregion Fields

    #region Properties

    public string LeftPanelSize
    {
        get { return (this.leftPanelSize - padding).ToString() + "%"; }
    }

    public string MiddlePanelSize
    {
        get { return (this.middlePanelSize - padding).ToString() + "%"; }
    }

    public string MiddlePanelVisible
    {
        get { return this.middlePanelVisible; }
    }

    public string RightPanelSize
    {
        get { return (this.rightPanelSize - padding).ToString() + "%"; }
    }

    public string RightPanelVisible
    {
        get { return this.rightPanelVisible; }
    }

    #endregion Properties

    #region Methods

    public void SetLayout(int layoutID)
    {
        switch (layoutID)
        {
            case 1:
                this.leftPanelSize = 33;
                this.middlePanelSize = 33;
                this.rightPanelSize =33;
                this.rightPanelVisible = "visible";
                this.middlePanelVisible = "visible";
                break;
            case 2:
                this.leftPanelSize = 25;
                this.middlePanelSize = 75;
                this.rightPanelSize = 0;
                this.rightPanelVisible = "hidden";
                this.middlePanelVisible = "visible";
                break;
            case 3:
                this.leftPanelSize = 75;
                this.middlePanelSize = 25;
                this.rightPanelSize = 0;
                this.rightPanelVisible = "hidden";
                this.middlePanelVisible = "visible";
                break;
            case 4:
                this.leftPanelSize = 100;
                this.middlePanelSize = 0;
                this.rightPanelSize = 0;
                this.rightPanelVisible = "hidden";
                this.middlePanelVisible = "hidden";
                break;
            default:
                this.leftPanelSize =33;
                this.middlePanelSize = 33;
                this.rightPanelSize = 33;
                this.rightPanelVisible = "visible";
                this.middlePanelVisible = "visible";
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #endregion Methods
}