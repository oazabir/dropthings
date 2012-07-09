<%@ Page Language="C#" MasterPageFile="Admin.master" %>

<script runat="server">
	private void Page_Init()
	{
		UserRoles.DataSource = Roles.GetAllRoles();
		UserRoles.DataBind();
	}
	
	private void Page_PreRender()
	{
		/*
		 * Dan Clem, 3/7/2007 and 4/27/2007.
		 * The logic here is necessitated by the limitations of the built-in object model.
		 * The Membership class does not provide a method to get users by role.
		 * The Roles class DOES provide a GetUsersInRole method, but it returns an array of UserName strings
		 * rather than a proper collection of MembershipUser objects.
		 * 
		 * This is my workaround.
		 * 
		 * Note to self: the two-collection approach is necessitated because you can't remove items from a collection
		 * while iterating through it: "Collection was modified; enumeration operation may not execute."
		 */
		
		MembershipUserCollection allUsers = Membership.GetAllUsers();
		MembershipUserCollection filteredUsers = new MembershipUserCollection();
		
		if (UserRoles.SelectedIndex > 0)
		{
			string[] usersInRole = Roles.GetUsersInRole(UserRoles.SelectedValue);
			foreach (MembershipUser user in allUsers)
			{
				foreach (string userInRole in usersInRole)
				{
					if (userInRole == user.UserName)
					{
						filteredUsers.Add(user);
						break; // Breaks out of the inner foreach loop to avoid unneeded checking.
					}
				}
			}
		}
		else
		{
			filteredUsers = allUsers;
		}
		Users.DataSource = filteredUsers;
		Users.DataBind();
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="body" Runat="Server">

<table class="webparts">
<tr>
	<th>Users by Role</th>
</tr>
<tr>
<td class="details" valign="top">

<!-- #include file="_nav3.aspx -->

Role filter:

<asp:DropDownList ID="UserRoles" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
<asp:ListItem>Show All</asp:ListItem>
</asp:DropDownList>


<br /><br />

<asp:GridView runat="server" ID="Users" AutoGenerateColumns="false"
	CssClass="list" AlternatingRowStyle-CssClass="odd" GridLines="none"
	AllowSorting="true"
	>
<Columns>
	<asp:TemplateField>
		<HeaderTemplate>User Name</HeaderTemplate>
		<ItemTemplate>
		<a href="edit_user.aspx?username=<%# Eval("UserName") %>"><%# Eval("UserName") %></a>
		</ItemTemplate>
	</asp:TemplateField>
	<asp:BoundField DataField="email" HeaderText="Email" />
	<asp:BoundField DataField="comment" HeaderText="Comments" />
	<asp:BoundField DataField="creationdate" HeaderText="Creation Date" />
	<asp:BoundField DataField="lastlogindate" HeaderText="Last Login Date" />
	<asp:BoundField DataField="lastactivitydate" HeaderText="Last Activity Date" />
	<asp:BoundField DataField="isapproved" HeaderText="Is Active" />
	<asp:BoundField DataField="isonline" HeaderText="Is Online" />
	<asp:BoundField DataField="islockedout" HeaderText="Is Locked Out" />
</Columns>
</asp:GridView>

</td></tr></table>



</asp:Content>
