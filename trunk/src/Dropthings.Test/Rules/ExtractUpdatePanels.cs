namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    /// <summary>
    /// Extracts all the update panels in the output of an aspx page. It only supports visit or postback, it does not support scanning
    /// the output of Async Postback. Something to do.
    /// The update panels are added as: 
    /// $UPDATEPANEL.WidgetBodyUpdatePanel.1        = tWidgetPage$WidgetZone2407$WidgetContainer3930$WidgetBodyUpdatePanel	
    /// $UPDATEPANEL.WidgetBodyUpdatePanel.1.$POS   = 94788	(Position in the HTML)
    /// It also adds all the UpdatePanels in a sequence:
    /// $UPDATEPANEL.1	        = tUserTabPage$TabUpdatePanel	
    /// $UPDATEPANEL.1.$POS	    = 94789	
    /// </summary>
    public class ExtractUpdatePanels : ExtractionRule
    {
        #region Fields

        public const string UPDATE_PANEL_COUNT_KEY = UPDATE_PANEL_PREFIX + ".COUNT";
        public const string UPDATE_PANEL_DECLARATION = "Sys.WebForms.PageRequestManager.getInstance()._updateControls([";
        public const string UPDATE_PANEL_POS_KEY = ".$POS";
        public const string UPDATE_PANEL_PREFIX = "$UPDATEPANEL.";

        #endregion Fields

        #region Methods

        public static void ExtractUpdatePanelNames(string body, WebTestContext context)
        {
            RuleHelper.NotAlreadyDone(context, "$UPDATEPANEL.EXTRACTED", () =>
                {
                    // Do not extract update panel names twice
                    int pos = body.IndexOf(UPDATE_PANEL_DECLARATION);
                    if (pos > 0)
                    {
                        // found declaration of all update panels on the page
                        pos += UPDATE_PANEL_DECLARATION.Length;
                        int endPos = body.IndexOf(']', pos);
                        string updatePanelNamesDelimited = body.Substring(pos, endPos - pos);
                        string[] updatePanelNames = updatePanelNamesDelimited.Split(',');
                        int updatePanelCounter = 1;
                        foreach (string updatePanelName in updatePanelNames)
                        {
                            // Create a unique key in the context using the UpdatePanel's Last part of the ID which is usually the
                            // ID specified in aspx page
                            string updatePanelFullId = updatePanelName.TrimStart('\'').TrimEnd('\'').TrimStart('t');
                            string updatePanelIdLastPart = updatePanelFullId.Substring(updatePanelFullId.LastIndexOf('$') + 1);
                            string contextKeyName = UPDATE_PANEL_PREFIX + updatePanelIdLastPart;
                            string keyName = RuleHelper.PlaceUniqueItem(context, contextKeyName, updatePanelFullId);

                            // Store all update panels as $UPDATEPANEL.1, $UPDATEPANEL.2, ...
                            context[UPDATE_PANEL_PREFIX + updatePanelCounter] = updatePanelFullId;

                            // Find the position of the UpdatePanel
                            string updatePanelDivId = updatePanelFullId.Replace('$', '_');
                            // Look for a div with id having the updatepanel ID, e.g. <div id="UserTabPage_TabUpdatePanel">
                            string lookingFor = "<div id=\"" + updatePanelDivId + "\"";
                            int updatePanelDivIdPos = body.IndexOf(lookingFor);
                            context[UPDATE_PANEL_PREFIX + updatePanelCounter + UPDATE_PANEL_POS_KEY] = updatePanelDivIdPos;
                            context[keyName + UPDATE_PANEL_POS_KEY] = updatePanelDivIdPos;

                            updatePanelCounter++;
                        }

                        context[UPDATE_PANEL_COUNT_KEY] = updatePanelCounter;
                    }
                });
        }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string body = e.Response.BodyString;
            ExtractUpdatePanelNames(body, e.WebTest.Context);
        }

        #endregion Methods
    }
}