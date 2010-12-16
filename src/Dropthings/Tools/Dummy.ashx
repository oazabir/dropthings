<%@ WebHandler Language="C#" Class="Dummy" %>

using System;
using System.Web;

public class Dummy : IHttpHandler {
    private readonly static string dummyOutput = new Func<string>(() => 
        {
            var buffer = new System.Text.StringBuilder(1000);
            for (int i = 0; i < 1000; i++)
                buffer.Append('X');
            return buffer.ToString();
        })();
    
    public void ProcessRequest (HttpContext context) {
        System.Threading.Thread.Sleep(500);
        
        context.Response.Buffer = false;
        context.Response.BufferOutput = false;
        context.Response.ContentType = "text/plain";

        for (int i = 0; i < 10; i++)
        {
            context.Response.Write(dummyOutput);
            System.Threading.Thread.Sleep(200);
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}