<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiggWidget.ascx.cs" Inherits="Widgets_DiggWidget" %>
    
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">

</asp:Panel>
    <div id="silverlightControlHost" style="height:450px; position:relative;">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
            <param name="Source" value='<asp:Literal ID="Source" runat="server" />' />      
            <param name="onError" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="3.0.40624.0" />
            <param name="autoUpgrade" value="true" />
            <param name="Width" value="100%" />
            <param name="Height" value="100%" />
            <param name="InitParams" value='<asp:Literal ID="InitParams" runat="server" />' />

            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration:none">
              <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
            </a>
        </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
    </div>
