using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for LinkButtonWithLayout
/// </summary>
namespace Dropthings.Web.UI
{
    public class LinkButtonWithLayout : LinkButton
    {
        public LinkButtonWithLayout()
        {

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(
                  this.UniqueID,
                  "1"
               );
            Page.ClientScript.RegisterForEventValidation(
          this.UniqueID,
          "2"
       );

            Page.ClientScript.RegisterForEventValidation(
          this.UniqueID,
          "3"
       );

            Page.ClientScript.RegisterForEventValidation(
          this.UniqueID,
          "4"
       );

            base.Render(writer);
        }
    }
}