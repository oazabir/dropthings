namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [DisplayName("Extract UpdatePanel for Control")]
    public class ExtractUpdatePanelForControl : ExtractionRule
    {
        #region Properties

        public string ControlID
        {
            get; set;
        }

        [DefaultValue(true)]
        public bool Required
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Extracts the UpdatePanel Client ID which contains the specified Control. If the 
        /// Control is inside an UpdatePanel, then it returns the UpdatePanel Client ID, otherwise
        /// it returns empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string body = e.Response.BodyString;

            // Look for the doPostback('ControlID')
            string lookingFor = "__doPostBack('" + this.ControlID + "'";
            int controlPos = body.IndexOf(lookingFor);
            if (controlPos == -1)
                throw new ApplicationException("Cannot find in html: " + lookingFor);

            // Find the nearest UpdatePanel
            ExtractUpdatePanels.ExtractUpdatePanelNames(e.Response.BodyString, e.WebTest.Context);
            int updatePanelCount = (int)e.WebTest.Context[ExtractUpdatePanels.UPDATE_PANEL_COUNT_KEY];
            int minDistance = int.MaxValue;
            string closestUpdatePanel = string.Empty;
            for (int i = 1; i < updatePanelCount; i++)
            {
                string updatePanelKeyName = ExtractUpdatePanels.UPDATE_PANEL_PREFIX + i + ExtractUpdatePanels.UPDATE_PANEL_POS_KEY;
                int updatePanelPos = (int)e.WebTest.Context[updatePanelKeyName];

                // UpdatePanel must start before the control
                if (updatePanelPos < controlPos)
                {
                    int distance = controlPos - updatePanelPos;
                    if (distance <= minDistance)
                    {
                        minDistance = distance;
                        // Get the ID of the update panel
                        closestUpdatePanel = e.WebTest.Context[ExtractUpdatePanels.UPDATE_PANEL_PREFIX + i] as string;
                    }
                }
            }

            if (!string.IsNullOrEmpty(closestUpdatePanel))
            {
                e.WebTest.Context[this.ContextParameterName] = closestUpdatePanel;
            }
            else
            {
                if (Required)
                    throw new ApplicationException(string.Format("UpdatePanel not found for Control ID: {0}", this.ControlID));
                else
                    e.WebTest.Context[this.ContextParameterName] = string.Empty;
            }
        }

        #endregion Methods
    }
}