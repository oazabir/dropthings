<%@ Page Language="C#" MasterPageFile="Admin.master" %>

<script runat="server">
	string username;
	
	MembershipUser user;
	
	private void Page_Load()
	{
		username = Request.QueryString["username"];
		if (username == null || username == "")
		{
			Response.Redirect("users.aspx");
		}
		user = Membership.GetUser(username);
		UserUpdateMessage.Text = "";
	}

	protected void UserInfo_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
	{
		//Need to handle the update manually because MembershipUser does not have a
		//parameterless constructor  

		user.Email = (string)e.NewValues[0];
		user.Comment = (string)e.NewValues[1];
		user.IsApproved = (bool)e.NewValues[2];

		try
		{
			// Update user info:
			Membership.UpdateUser(user);
			
			// Update user roles:
			UpdateUserRoles();
			
			UserUpdateMessage.Text = "Update Successful.";
			
			e.Cancel = true;
			UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
		}
		catch (Exception ex)
		{
			UserUpdateMessage.Text = "Update Failed: " + ex.Message;

			e.Cancel = true;
			UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
		}
	}

	private void Page_PreRender()
	{
		// Load the User Roles into checkboxes.
		UserRoles.DataSource = Roles.GetAllRoles();
		UserRoles.DataBind();

		// Disable checkboxes if appropriate:
		if (UserInfo.CurrentMode != DetailsViewMode.Edit)
		{
			foreach (ListItem checkbox in UserRoles.Items)
			{
				checkbox.Enabled = false;
			}
		}
		
		// Bind these checkboxes to the User's own set of roles.
		string[] userRoles = Roles.GetRolesForUser(username);
		foreach (string role in userRoles)
		{
			ListItem checkbox = UserRoles.Items.FindByValue(role);
			checkbox.Selected = true;
		}
	}
	
	private void UpdateUserRoles()
	{
		foreach (ListItem rolebox in UserRoles.Items)
		{
			if (rolebox.Selected)
			{
				if (!Roles.IsUserInRole(username, rolebox.Text))
				{
					Roles.AddUserToRole(username, rolebox.Text);
				}
			}
			else
			{
				if (Roles.IsUserInRole(username, rolebox.Text))
				{
					Roles.RemoveUserFromRole(username, rolebox.Text);
				}
			}
		}
	}

	private void DeleteUser(object sender, EventArgs e)
	{
		//Membership.DeleteUser(username, false); // DC: My apps will NEVER delete the related data.
		Membership.DeleteUser(username, true); // DC: except during testing, of course!
		Response.Redirect("users.aspx");
	}

	private void UnlockUser(object sender, EventArgs e)
	{
		// Dan Clem, added 5/30/2007 post-live upgrade.
		
		// Unlock the user.
		user.UnlockUser();
		
		// DataBind the GridView to reflect same.
		UserInfo.DataBind();
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="body" Runat="Server">

<table class="webparts">
<tr>
	<th>User Information</th>
</tr>
<tr>
<td class="details" valign="top">

<h3>Roles:</h3>
<asp:CheckBoxList ID="UserRoles" runat="server" />

<h3>Main Info:</h3>
<asp:DetailsView AutoGenerateRows="False" DataSourceID="MemberData"
  ID="UserInfo" runat="server" OnItemUpdating="UserInfo_ItemUpdating"
  >
  
<Fields>
	<asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:CheckBoxField DataField="IsApproved" HeaderText="Active User" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	<asp:CheckBoxField DataField="IsLockedOut" HeaderText="Is Locked Out" ReadOnly="true" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	
	<asp:CheckBoxField DataField="IsOnline" HeaderText="Is Online" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	<asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
	 HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="LastLockoutDate" HeaderText="LastLockoutDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="LastPasswordChangedDate" HeaderText="LastPasswordChangedDate"
	ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:CommandField ButtonType="button" ShowEditButton="true" EditText="Edit User Info" />
</Fields>
</asp:DetailsView>
<div class="alert" style="padding: 5px;">
<asp:Literal ID="UserUpdateMessage" runat="server">&nbsp;</asp:Literal>
</div>


<div style="text-align: right; width: 100%; margin: 20px 0px;">
<asp:Button ID="Button1" runat="server" Text="Unlock User" OnClick="UnlockUser" OnClientClick="return confirm('Click OK to unlock this user.')" />
&nbsp;&nbsp;&nbsp;
<asp:Button ID="Button2" runat="server" Text="Delete User" OnClick="DeleteUser" OnClientClick="return confirm('Are Your Sure?')" />
</div>


<asp:ObjectDataSource ID="MemberData" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser" SelectMethod="GetUser" UpdateMethod="UpdateUser" TypeName="System.Web.Security.Membership">
	<SelectParameters>
		<asp:QueryStringParameter Name="username" QueryStringField="username" />
	</SelectParameters>
</asp:ObjectDataSource> 
</td>

</tr></table>



</asp:Content>