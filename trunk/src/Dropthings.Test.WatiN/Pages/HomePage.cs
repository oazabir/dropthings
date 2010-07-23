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
        [FindBy(Id = "TabControlPanel_ShowAddContentPanel")]
        public Link AddStuffLink;

        [FindBy(ClassRegex = "newtab_add*")]
        public Link AddNewTabLink;

        public void ShowAddStuff()
        {
            AddStuffLink.Click();
        }

        public void AddNewTab()
        {
            AddNewTabLink.Click();
        }

        public LinkCollection TabLinks
        {
            get
            {
                return base.Document.Links.Filter(Find.ByClass("tab_link"));
            }
        }

        public Span CurrentTab
        {
            get
            {
                return this.Document.Span(Find.ByClass("current_tab"));
            }
        }

        public Table WidgetDataList
        {
            get
            {
                return base.Document.Table("TabControlPanel_WidgetListControlAdd_WidgetDataList");
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
