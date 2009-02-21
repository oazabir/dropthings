namespace MyOfficeTest.Rules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    using Procurios.Public;

    [DisplayName("Extract JSON")]
    public class ExtractJSON : ExtractionRule
    {
        #region Methods

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            object obj = JSON.JsonDecode(e.Response.BodyString);

            ProcessObject(obj, this.ContextParameterName, e.WebTest.Context);
        }

        private void ProcessObject(object obj, string keyName, WebTestContext context)
        {
            if (obj is ArrayList)
            {
                ArrayList list = obj as ArrayList;
                int counter = 1;
                foreach (object o in list)
                {
                    string keyNameWithIndex = keyName + "." + counter ++;
                    ProcessObject(o, keyNameWithIndex, context);
                }
            }
            else if (obj is Hashtable)
            {
                Hashtable table = obj as Hashtable;
                foreach (DictionaryEntry entry in table)
                {
                    string keyNameWithProperty = keyName + "." + entry.Key;
                    ProcessObject(entry.Value, keyNameWithProperty, context);
                }
            }
            else
            {
                context[keyName] = obj;
            }
        }

        #endregion Methods
    }
}