/// <summary>
/// Summary description for ImageButtonWithLayout
/// </summary>
namespace Dropthings.Web.UI
{
    using System;
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

    public class ImageButtonWithLayout : ImageButton
    {
        #region Constructors

        public ImageButtonWithLayout()
        {
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}