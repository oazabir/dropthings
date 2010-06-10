using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using System.Text.RegularExpressions;

namespace Dropthings.Test.WatiN.Controls
{
    public class WidgetControl : Control<Div>
    {
        public override global::WatiN.Core.Constraints.Constraint ElementConstraint
        {
            get
            {
                return Find.ByClass(className => className == "widget" || className.Contains("widget "))
                    .And(Find.ById("new_widget_template").Not());
            }
        }

        public string Title
        {
            get
            {
                return TitleLink.Text.Trim();
            }
        }

        public Link TitleLink
        {
            get
            {
                return base.Element.Link(Find.ByClass("widget_title_label"));
            }
        }

        public TextField TitleEditor
        {
            get
            {
                return base.Element.TextField(Find.ByClass("widget_title_input"));
            }
        }

        public Button TitleSaveButton
        {
            get
            {
                return base.Element.Button(Find.ByClass("widget_title_submit"));
            }
        }

        public Link CloseLink
        {
            get
            {
                return base.Element.Link(link => link.Id.EndsWith("CloseWidget"));
            }
        }

        public Link EditLink
        {
            get
            {
                return base.Element.Link(Find.ByClass("widget_edit"));
            }
        }

        public Div Header
        {
            get
            {
                return base.Element.Div(div => div.ClassName.Contains("widget_header"));
            }
        }

        public void EditTitle()
        {
            TitleLink.Click();
        }

        public void SetNewTitle(string newTitle)
        {
            TitleEditor.TypeText(newTitle);
            TitleSaveButton.Click();
        }

        public void Close()
        {
            CloseLink.Click();
        }

        
    }
}
