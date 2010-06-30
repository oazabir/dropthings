#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for StockQuote
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class StockQuote : System.Web.Services.WebService
{
    #region Constructors

    public StockQuote()
    {
        //Uncomment the following line if using designed components
        //InitializeComponent();
    }

    #endregion Constructors

    #region Methods

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    #endregion Methods
}