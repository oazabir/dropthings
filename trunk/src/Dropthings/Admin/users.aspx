<%@ Page Language="C#" MasterPageFile="Admin.master" %>
<%@ Register TagPrefix="dc" TagName="alphalinks" Src="alphalinks.ascx" %>

<script runat="server">
	private void Page_PreRender()
	{
		if (Alphalinks.Letter == "All")
		{
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AdminConnectionStringName"]].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT Top (20) u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate as CreationDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.Email, 
            m.Comment,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate,
            u.LastActivitydate,
            m.Isapproved
    FROM   dbo.aspnet_Membership m
INNER JOIN dbo.aspnet_Users u on u.UserId = m.UserId
    WHERE u.IsAnonymous = 0
    ORDER BY u.LoweredUserName", con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    Users.DataSource = table;
                }
            }
		}
		else
		{
			Users.DataSource = Membership.FindUsersByName(Alphalinks.Letter + "%");
		}
		Users.DataBind();
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="body" Runat="Server">



<table class="webparts">
<tr>
	<th>Users by Name</th>
</tr>
<tr>
<td class="details" valign="top">

<!-- #include file="_nav3.aspx -->


User Name filter:&nbsp;&nbsp;&nbsp;
<dc:alphalinks runat="server" ID="Alphalinks" />

<br /><br />

<asp:GridView runat="server" ID="Users" AutoGenerateColumns="false"
	CssClass="list" AlternatingRowStyle-CssClass="odd" GridLines="none"
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
	<asp:BoundField DataField="islockedout" HeaderText="Is Locked Out" />

</Columns>
</asp:GridView>

</td>

</tr></table>


</asp:Content>
