using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using Dropthings.Test.WatiN.Pages;

namespace Dropthings.Test.WatiN.Controls
{
    public class RssWidgetControl : WidgetControl
    {
        public Div SettingsPanel
        {
            get
            {
                return base.Element.Div(Find.ById(id => id.EndsWith("SettingsPanel")));
            }
        }

        public TextField UrlTextBox
        {
            get
            {
                return SettingsPanel.TextField(Find.ById(id => id.EndsWith("FeedUrl")));
            }
        }

        public SelectList CountDropdown
        {
            get
            {
                return SettingsPanel.SelectList(Find.ById(id => id.EndsWith("FeedCountDropDownList")));
            }
        }

        public Button SaveButton
        {
            get
            {
                return SettingsPanel.Button(Find.ById(id => id.EndsWith("SaveSettings")));
            }
        }

        public LinkCollection FeedLinks
        {
            get
            {
                return base.Element.Links.Filter(Find.ByClass("feed_item_link"));
            }
        }

        public static RssWidgetControl GetTheFirstRssWidget(HomePage page)
        {
            // Find the widgets which has the RssWidgetTimer. Then they are the RSS widgets. Unless 
            // someone has copied and pasted the whole RSS widget's code into a new widget and forgot
            // to change the timer name.
            var widget = page.Widgets.Where(w => w.Element.Span(Find.ById(id => id.EndsWith("RSSWidgetTimer"))).Exists).First();
            var rssWidget = RssWidgetControl.CreateControl<RssWidgetControl>(widget.Element);

            return rssWidget;
        }
    }
}
