<%@ Page Language="C#" MasterPageFile="Admin.master" %>

<script runat="server">
	private void Page_PreRender()
	{
		MembershipUserCollection allUsers = Membership.GetAllUsers();
		MembershipUserCollection filteredUsers = new MembershipUserCollection();
		bool isLockedOut = true;
		foreach (MembershipUser user in allUsers)
		{
			if (user.IsLockedOut == isLockedOut)
			{
				filteredUsers.Add(user);
			}
		}
		Users.DataSource = filteredUsers;
		Users.DataBind();
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="body" Runat="Server">


<table class="webparts">
<tr>
	<th>Locked Out Users</th>
</tr>
<tr>
<td class="details" valign="top">

<!-- #include file="_nav3.aspx -->

<br />
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


</td>

</tr></table>


</asp:Content>
