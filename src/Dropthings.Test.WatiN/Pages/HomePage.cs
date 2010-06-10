using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using Dropthings.Test.WatiN.Controls;

namespace Dropthings.Test.WatiN.Pages
{
    [Page(UrlRegex=Urls.Homepage)]
    public class HomePage : Page
    {
        [FindBy(Id = "ChangeSettingsControl_ShowAddContentPanel")]
        public Link AddStuffLink;

        public void ShowAddStuff()
        {
            AddStuffLink.Click();
        }

        public Table WidgetDataList
        {
            get
            {
                return base.Document.Table("ChangeSettingsControl_WidgetListControlAdd_WidgetDataList");
            }
        }

        public List<Link> AddWidgetLinks
        {
            get
            {
                return base.Document.Links.Where(link => link.ClassName == "widgetitem").ToList();
            }
        }

        public ControlCollection<WidgetControl> Widgets
        {
            get
            {
                return base.Document.Controls<WidgetControl>();
            }
        }

        public void WaitForAsyncPostbackComplete()
        {
            
        }
    }
}
