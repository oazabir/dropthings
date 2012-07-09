<%@ Page Language="C#" MasterPageFile="Admin.master" %>

<script runat="server">
	
	private void Page_Load()
	{
		if (IsPostBack)
		{
			try
			{
				AddUser();

				Response.Redirect("users.aspx");
			}
			catch (Exception ex)
			{
				ConfirmationMessage.InnerText = "Insert Failure: " + ex.Message;
			}
		}
	}

	protected void AddUser()
	{
		// Add User.
		MembershipUser newUser = Membership.CreateUser(username.Text, password.Text, email.Text);
		newUser.Comment = comment.Text;
		Membership.UpdateUser(newUser);
		
		// Add Roles.
		foreach (ListItem rolebox in UserRoles.Items)
		{
			if (rolebox.Selected)
			{
				Roles.AddUserToRole(username.Text, rolebox.Text);
			}
		}
	}

	private void Page_PreRender()
	{
		UserRoles.DataSource = Roles.GetAllRoles();
		UserRoles.DataBind();
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="body" Runat="Server">

<table class="webparts">
<tr>
	<th>Add User</th>
</tr>
<tr>
<td class="details" valign="top">

<h3>Roles:</h3>
<asp:CheckBoxList ID="UserRoles" runat="server" />

<h3>Main Info:</h3>

<table>
<tr>
	<td class="detailheader">Active User</td>
	<td>
		<asp:CheckBox ID="isapproved" runat="server" Checked="true" />
	</td>
</tr>
<tr>
	<td class="detailheader">User Name</td>
	<td>
		<asp:TextBox ID="username" runat="server"></asp:TextBox>
	</td>
</tr>
<tr>
	<td class="detailheader">Password</td>
	<td>
		<asp:TextBox ID="password" runat="server"></asp:TextBox>
	</td>
</tr>
<tr>
	<td class="detailheader">Email</td>
	<td>
		<asp:TextBox ID="email" runat="server"></asp:TextBox>
	</td>
</tr>
<tr>
	<td class="detailheader">Comment</td>
	<td>
		<asp:TextBox ID="comment" runat="server"></asp:TextBox>
	</td>
</tr>
<tr>
	<td colspan="2"><br />
		<input type="submit" value="Add User" />&nbsp;
		<input type="reset" />
	</td>
</tr>
<tr>
	<td colspan="2">
	<div id="ConfirmationMessage" runat="server" class="alert"></div>
	</td>
</tr>
</table>

<asp:ObjectDataSource ID="MemberData" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser" SelectMethod="GetUser" UpdateMethod="UpdateUser" TypeName="System.Web.Security.Membership">
	<SelectParameters>
		<asp:QueryStringParameter Name="username" QueryStringField="username" />
	</SelectParameters>
</asp:ObjectDataSource> 
</td>

</tr></table>

</asp:Content>