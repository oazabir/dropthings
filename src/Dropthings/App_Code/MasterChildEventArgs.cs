using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MasterChildEventArgs
/// </summary>
public class MasterChildEventArgs : EventArgs
{
    public string Who { get; set; }
    public string Message { get; set; }

    public MasterChildEventArgs(string who, string message)
    {
        this.Who = who;
        this.Message = message;
    }
}
