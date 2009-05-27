<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TwitterWidget.ascx.cs" Inherits="TwitterWidget" %>
<script type="text/javascript">
    var TwitterWidgetOperations = function() {
    };
    TwitterWidgetOperations.prototype = {
        _selectedOption: null,
        _currentPage: 1,
        _pagingEnable: false,
        _currentResponse: null,
        _loggedIn: false,
        _pageCount: 1,
        LoggedIn: function(val) {
            this._loggedIn = (val == 'True') ? true : false;
        },
        ResetTabs: function() {
            $('.twitterTabs ul li').removeClass('selected');
        },
        SetSelected: function(el) {
            this.ResetTabs();
            if (typeof (el) != 'object') el = $get(el);
            $(el).parent().addClass('selected');
            this._selectedOption = $(el).html();
            if (this._selectedOption == 'Update') {
                $('.twitterView').css('display', 'none');
                $('.twitterUpdate').css('display', '');
                $('#<%= txtTwUpdate.ClientID %>').css('width', (($('.twitterUpdate').width() - 10) + 'px'));
            }
            else {
                $('.twitterView').css('display', '');
                $('.twitterUpdate').css('display', 'none');
            }
        },
        ShowSettings: function() {
            $('#<%= btnTwShowSettings.ClientID %>').click();
        },
        ViewDisplay: function(items) {
            $('.twitterView').css('display', 'block').html('');
            var mainDiv;
            var dataDiv;
            if (this._loggedIn == false) {
                $('<div/>').addClass('twitterLogin').html('Please <a href="javascript:void(0)" onclick="Tw_op.ShowSettings()">Sign in</a> to update your account').appendTo('.twitterView');
            }
            $.each(items, function(i, item) {
                mainDiv = 'u_' + item.user.id + '_' + i;
                dataDiv = 'd_' + item.user.id + '_' + i;
                if (i % 2) {
                    $('<div/>').attr('id', mainDiv).addClass('even').appendTo('.twitterView');
                }
                else {
                    $('<div/>').attr('id', mainDiv).addClass('odd').appendTo('.twitterView');
                }
                $('<img/>').attr('src', item.user.profile_image_url).attr('alt', item.user.screen_name).attr('title', item.user.screen_name).appendTo('#' + mainDiv);
                $('<div/>').attr('id', dataDiv).appendTo('#' + mainDiv);
                $('<a/>').attr('href', ((item.user.url != null) ? item.user.url : 'javascript:void(0)')).html(item.user.screen_name).appendTo('#' + dataDiv);
                $('<div/>').html(item.text).appendTo('#' + dataDiv);
                $('<div/>').html(item.created_at).appendTo('#' + dataDiv);
            });
            if (this._pagingEnable) {
                if (this._currentPage != this._pageCount) $('<a/>').html('Next >').addClass('nextLink').attr('href', 'javascript:void(0)').click(this.NextPage).appendTo('.twitterView');
                if (this._currentPage != 1) $('<a/>').html('< Previous').addClass('prevLink').attr('href', 'javascript:void(0)').click(this.PrevPage).appendTo('.twitterView');
            }
        },
        SetPageItems: function() {
            var returnArray = new Array();
            var startIdx = (this._currentPage - 1) * 5;
            for (var p = startIdx; p < ((this._currentResponse.length > (startIdx + 5)) ? (startIdx + 5) : this._currentResponse.length); p++)
                returnArray.push(this._currentResponse[p]);

            this.ViewDisplay(returnArray);
        },
        ParseUpdate: function(resp) {
        },
        ParseStatus: function(resp) {
            this._currentResponse = resp;
            if (this._currentResponse.length > 5) {
                this._pagingEnable = true;
                this._pageCount = (Math.ceil(this._currentResponse.length / 5));
            }

            this._currentPage = 1;
            this.SetPageItems();
        },
        SetDisplayFromResponse: function() {
            var response = $('#<%= hdnResponseField.ClientID %>').val();
            var obj = eval(response);
            switch (this._selectedOption) {
                case 'Update': this.ParseUpdate(obj); break;
                case 'Friends':
                case 'Public':
                case 'Archive': this.ParseStatus(obj); break;
                default: this.ParseStatus(obj); break;
            }
        },
        NextPage: function() {
            Tw_op._currentPage++;
            Tw_op.SetPageItems();
        },
        PrevPage: function() {
            Tw_op._currentPage--;
            Tw_op.SetPageItems();
        }
    };
    
</script>
<link rel="Stylesheet" href="/Widgets/TwitterWidget.css" />
<asp:Panel ID="settingsPanel" runat="server" Visible="False">
    <h3 class="twitterHeading">Twitter Credentials</h3>
    <table width="100%" border="0">
        <tr>
            <td colspan="2" class="twitterError"><asp:Literal ID="litTwError" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td>Twitter Username</td>
            <td><asp:TextBox ID="txtTwUsername" runat="server" style="width:140px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Twitter Password</td>
            <td><asp:TextBox ID="txtTwPassword" TextMode="Password" runat="server" style="width:140px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />&nbsp;<asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:MultiView ID="TwitterWidgetMultiview" runat="server" ActiveViewIndex="0">
    <asp:View runat="server" ID="TwitterWidgetProgressView">
        <asp:Label runat="Server" ID="label1" Text="Loading..." Font-Size="smaller" ForeColor="DimGray" />
    </asp:View>
    <asp:View runat="server" ID="TwitterWidgetMainView">
        <asp:Panel ID="dataPanel" runat="server">
            <div class="twitterTabs">
                <ul>
                    <li><asp:LinkButton ID="lbtnUpdate" runat="server" Text="Update" OnClientClick="Tw_op.SetSelected(this);return false;" Enabled="false"></asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbtnFriends" runat="server" Text="Friends" OnClick="lbtnFriends_Click" OnClientClick="Tw_op.SetSelected(this)" Enabled="false"></asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbtnArchive" runat="server" Text="Archive" OnClick="lbtnArchive_Click" OnClientClick="Tw_op.SetSelected(this)" Enabled="false"></asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbtnPublic" runat="server" Text="Public" OnClick="lbtnPublic_Click" OnClientClick="Tw_op.SetSelected(this)"></asp:LinkButton></li>
                </ul>
            </div>
            <div class="twitterView"></div>
            <div class="twitterUpdate" style="display:none">
                <asp:Label ID="lblTwUpdtaeError" CssClass="twitterError" runat="server"></asp:Label>
                <asp:TextBox TextMode="MultiLine" ID="txtTwUpdate" runat="server" Rows="4"></asp:TextBox>
                <asp:Button ID="btnTwUpdate" Text="Update" runat="server" OnClick="btnTwUpdate_Click" />
            </div>
        </asp:Panel>
    </asp:View>
</asp:MultiView>
<asp:Button ID="btnTwShowSettings" OnClick="btnTwShowSettings_Click" runat="server" style="display:none" />
<asp:HiddenField ID="hdnResponseField" runat="server" />
<asp:Timer ID="TwitterWidgetTimer" Interval="100" OnTick="LoadWidget" runat="server" /> 