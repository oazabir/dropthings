using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MasterChildEventArgs
/// </summary>
public class MasterChildEventArgs : EventArgs
{
    #region Constructors

    public MasterChildEventArgs(string who, string message)
    {
        this.Who = who;
        this.Message = message;
    }

    #endregion Constructors

    #region Properties

    public string Message
    {
        get; set;
    }

    public string Who
    {
        get; set;
    }

    #endregion Properties
}