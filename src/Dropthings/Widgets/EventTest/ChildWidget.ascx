<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChildWidget.ascx.cs" Inherits="Widgets_EventTest_ChildWidget" EnableViewState="false" %>
<b>I am a child widget</b><br />
Say: <asp:TextBox ID="Message" runat="server" Text="Greetings Master" /><br />
<asp:Button ID="Raise" runat="server" Text="Greet Master" OnClick="Raise_Clicked" /><br />
<br />
<asp:Label ID="Received" runat="server" Font-Bold="true" />