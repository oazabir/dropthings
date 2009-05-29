<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TwitterWidget.ascx.cs" Inherits="TwitterWidget" %>
<script src="/Widgets/TwitterWidget/TwitterService.js" id="twitterService" type="text/javascript"></script>
<script type="text/javascript">
    var Twitter_<%= WidgetID %> = function() {
        this.LoadWidget();
    };
    Twitter_<%= WidgetID %>.prototype = {
        _selectedOption: null,
        _currentPage: 1,
        _pagingEnable: false,
        _currentResponse: null,
        _loggedIn: false,
        _pageCount: 1,
        _widgetState: null,
        _uName: null,
        _uPass: null,
        _widgetID: <%= WidgetHostID %>,
        ResetTabs: function() {
            $('#<%= WidgetID %>_MainView .twitterTabs li').removeClass('selected');
        },
        SetSelected: function(el) {
            if (typeof (el) != 'object') el = $('#'+el);
            this._selectedOption = $(el).html();
            $('#<%= WidgetID %>_TwUpdateError').html('');
            if(!this._loggedIn) 
            {
                if(this._selectedOption != 'Public')
                {
                    this._selectedOption = 'Public';
                    return;
                }
            }
            this.ResetTabs();
            $(el).parent().addClass('selected');
            $('#<%= WidgetID %>_TwView').html('');
            if (this._selectedOption != 'Update') {
                $('#<%= WidgetID %>_TwView').css('display', '');
                $('#<%= WidgetID %>_TwUpdatePanel').css('display', 'none');
            }
            switch (this._selectedOption) {
                case 'Update': {
                    $('#<%= WidgetID %>_TwView').css('display', 'none');
                    $('#<%= WidgetID %>_TwUpdateError').html('');
                    $('#<%= WidgetID %>_TwUpdatePanel').css('display', '');
                    $('#<%= WidgetID %>_TwUpdateText').css('width', (($('.twitterUpdate').width() - 10) + 'px'));
                    break;
                }
                case 'Friends': this.LoadFriendsTimeline(); break;
                case 'Public': this.LoadPublicTimeline(); break;
                case 'Archive': this.LoadMyTimeline(); break;
            }
        },
        Update: function() {
            this.ShowProgress('Updating...');
            $('#<%= WidgetID %>_TwUpdateBtn').attr('disabled','true');
            TwitterService.UpdateStaus(this._uName, this._uPass, $('#<%= WidgetID %>_TwUpdateText').val(),function(resp){
                $('#<%= WidgetID %>_TwUpdateBtn').attr('disabled','false');
                if(resp.toString().indexOf("error") > -1) $('#<%= WidgetID %>_TwUpdateError').html('Update failed. Please check your credentials.');
                else Tw_<%= WidgetID %>.SetSelected('<%= WidgetID %>_TwFriends');
            }, 
            function(msg){
                $('#<%= WidgetID %>_TwUpdateBtn').attr('disabled','false');
                if(resp.toString().indexOf("error") > -1) $('#<%= WidgetID %>_TwUpdateError').html('Update failed:' + msg);
            });
        },
        HideSettings: function() {
            $('#<%= WidgetID %>_SettingsPanel').css('display','none');
            $('#<%= WidgetID %>_Content').css('display','');
        },
        ShowSettings: function() {
            $('#<%= WidgetID %>_SettingsPanel').css('display','');
            $('#<%= WidgetID %>_Content').css('display','none');
        },
        ViewDisplay: function(items) {
            $('#<%= WidgetID %>_MainView').css('display', 'block');
            $('#<%= WidgetID %>_Progress').css('display', 'none');
            $('#<%= WidgetID %>_TwUpdateError').html('');
            $('#<%= WidgetID %>_TwView').html('');
            var mainDiv;
            var dataDiv;
            if(!this._loggedIn) {
                $('<div/>').addClass('twitterLogin').html('Please <a href="javascript:void(0)" onclick="Tw_<%= WidgetID %>.ShowSettings()">sign in</a> to view & update').appendTo('#<%= WidgetID %>_TwView');
            }
            $.each(items, function(i, item) {
                mainDiv = 'u_' + item.user.id + '_' + i;
                dataDiv = 'd_' + item.user.id + '_' + i;
                if (i % 2) {
                    $('<div/>').attr('id', mainDiv).addClass('even').appendTo('#<%= WidgetID %>_TwView');
                }
                else {
                    $('<div/>').attr('id', mainDiv).addClass('odd').appendTo('#<%= WidgetID %>_TwView');
                }
                $('<img/>').attr('src', item.user.profile_image_url).attr('alt', item.user.screen_name).attr('title', item.user.screen_name).appendTo('#' + mainDiv);
                $('<div/>').attr('id', dataDiv).appendTo('#' + mainDiv);
                $('<a/>').attr('href', ((item.user.url != null) ? item.user.url : 'http://twitter.com/' + item.user.screen_name)).attr('target', '_blank').html(item.user.screen_name).appendTo('#' + dataDiv);
                $('<div/>').html(item.text).appendTo('#' + dataDiv);
                $('<div/>').html(item.created_at).appendTo('#' + dataDiv);
            });
            if (this._pagingEnable) {
                if (this._currentPage != this._pageCount) $('<a/>').html('Next >').addClass('nextLink').attr('href', 'javascript:void(0)').click(this.NextPage).appendTo('#<%= WidgetID %>_TwView');
                if (this._currentPage != 1) $('<a/>').html('< Previous').addClass('prevLink').attr('href', 'javascript:void(0)').click(this.PrevPage).appendTo('#<%= WidgetID %>_TwView');
            }
        },
        SetPageItems: function() {
            var returnArray = new Array();
            var startIdx = (this._currentPage - 1) * 5;
            for (var p = startIdx; p < ((this._currentResponse.length > (startIdx + 5)) ? (startIdx + 5) : this._currentResponse.length); p++)
                returnArray.push(this._currentResponse[p]);

            this.ViewDisplay(returnArray);
        },
        ParseStatus: function(resp) {
            this.HideProgress();
            this._currentResponse = Utility.getJSONObject(resp);
            if (this._currentResponse.length > 5) {
                this._pagingEnable = true;
                this._pageCount = (Math.ceil(this._currentResponse.length / 5));
            }

            this._currentPage = 1;
            this.SetPageItems();
        },
        NextPage: function() {
            Tw_<%= WidgetID %>._currentPage++;
            Tw_<%= WidgetID %>.SetPageItems();
        },
        PrevPage: function() {
            Tw_<%= WidgetID %>._currentPage--;
            Tw_<%= WidgetID %>.SetPageItems();
        },
        ParseError: function(msg){
            alert(msg);
        },
        LoadPublicTimeline: function(){
            this.ShowProgress('Loading public updates...');
            TwitterService.GetPublicStatuses(function(resp){
                Tw_<%= WidgetID %>.ParseStatus(resp);
            }, 
            function(msg){
                Tw_<%= WidgetID %>.ParseError(msg);
            });
        },
        LoadFriendsTimeline: function(){
            this.ShowProgress('Loading friends updates...');
            TwitterService.GetFriendStatuses(this._uName, this._uPass, function(resp){
                Tw_<%= WidgetID %>.ParseStatus(resp);
            }, 
            function(msg){
                Tw_<%= WidgetID %>.ParseError(msg);
            });
        },
        LoadMyTimeline: function(){
            this.ShowProgress('Loading my timeline...');
            TwitterService.GetUserStatuses(this._uName, this._uPass, function(resp){
                Tw_<%= WidgetID %>.ParseStatus(resp);
            }, 
            function(msg){
                Tw_<%= WidgetID %>.ParseError(msg);
            });
        },
        ShowProgress: function(msg){
            $('#<%= WidgetID %>_Progress').html(msg).css('display','block');
        },
        HideProgress: function(){
            $('#<%= WidgetID %>_Progress').css('display','none');
        },
        SuccessVerify: function(msg){
            if(msg.toString().indexOf('error') < 0)
            {
                Tw_<%= WidgetID %>._loggedIn = true;
                if($('#<%= WidgetID %>_Content').css('display') == 'none')
                {
                    Dropthings.Web.Framework.WidgetService.SaveWidgetState(Tw_<%= WidgetID %>._widgetID, Tw_<%= WidgetID %>.PrepareState(), function(msg){
                        Tw_<%= WidgetID %>.SuccessStateSave(msg);
                    },
                    function(err){
                        Tw_<%= WidgetID %>.FailStateSave(msg);
                    });
                }
                Tw_<%= WidgetID %>.HideSettings();
                Tw_<%= WidgetID %>.SetSelected('<%= WidgetID %>_TwFriends');
                
                
            }
            else
            {
                Tw_<%= WidgetID %>.FailVerify(msg);
            }
        },
        SuccessStateSave: function(msg){
            // nothing to do
        },
        FailStateSave: function(err){
            $('#<%= WidgetID %>_TwUpdateError').html('Failed to save your credentials');
        },
        PrepareState: function(){
            return '<state><username>' + this._uName + '</username><password>' + this._uPass + '</password></state>';
        },
        FailVerify: function(msg){
            Tw_<%= WidgetID %>._loggedIn = false;
            if($('#<%= WidgetID %>_Content').css('display') == 'none')
                $('#<%= WidgetID %>_TwError').html('Login failed. Please check your credentials.');
            else
                Tw_<%= WidgetID %>.SetSelected('<%= WidgetID %>_TwPublic');
        },
        VerifyCredentials: function(){
            if($('#<%= WidgetID %>_Content').css('display') == 'none')
            {
                $('#<%= WidgetID %>_TwError').html('');
                this._uName = $('#<%= WidgetID %>_TwUsername').val();
                this._uPass = $('#<%= WidgetID %>_TwPassword').val();
            }
            else
            {
                this._uName = this._widgetState.getElementsByTagName('state')[0].getElementsByTagName('username')[0].firstChild.nodeValue;
                this._uPass = this._widgetState.getElementsByTagName('state')[0].getElementsByTagName('password')[0].firstChild.nodeValue;
            }
            TwitterService.VerifyCredentials(this._uName, this._uPass, this.SuccessVerify, this.FailVerify);
        },
        CheckLength: function(el){
            var left = 140-el.value.length;
            if(left < 0)
            {
                el.value = el.value.substring(0, 140);
                left--;
            }
            $('#<%= WidgetID %>_TwUpdateLeft').html(left+' characters left');
        },
        LoadNew: function() {
            this._loggedIn = false;
            this.SetSelected('<%= WidgetID %>_TwPublic');
        },
        LoadWidget: function(){
            this._widgetState = Utility.getXMLDocument("<%= WidgetState %>");
            try
            {
                if(this._widgetState.getElementsByTagName('state')[0].getElementsByTagName('username')[0].firstChild.nodeValue != '')
                    this.VerifyCredentials();
                else
                    this.LoadNew();
            }
            catch(e)
            {
                this.LoadNew();
            }
        }
    };
    var Tw_<%= WidgetID %>;
</script>
<link rel="Stylesheet" id="twitterCSS" href="/Widgets/TwitterWidget/TwitterWidget.css" />
<div id="<%= WidgetID %>_SettingsPanel" class="twSettings" style="display:none">
    <h3 class="twitterHeading">Twitter Credentials</h3>
    <table width="100%" border="0">
        <tr>
            <td colspan="2" class="twitterError" id="<%= WidgetID %>_TwError"></td>
        </tr>
        <tr>
            <td>Twitter Username</td>
            <td><input type="text" id="<%= WidgetID %>_TwUsername" class="twCredentialInput" /></td>
        </tr>
        <tr>
            <td>Twitter Password</td>
            <td><input type="password" id="<%= WidgetID %>_TwPassword" class="twCredentialInput" /></td>
        </tr>
        <tr>
            <td></td>
            <td><input type="button" id="<%= WidgetID %>_TwSave" onclick="Tw_<%= WidgetID %>.VerifyCredentials()" value="Save" />&nbsp;<input type="button" id="<%= WidgetID %>_TwCancel" onclick="Tw_<%= WidgetID %>.HideSettings()" value="Cancel" /></td>
        </tr>
    </table>
</div>
<div id="<%= WidgetID %>_Content">
    <div id="<%= WidgetID %>_MainView" style="display:none">
        <span id="<%= WidgetID %>_TwUpdateError" class="twitterError"></span>
        <div class="twitterTabs">
            <ul>
                <li><a id="<%= WidgetID %>_TwUpdate" href="javascript:void(0)" onclick="Tw_<%= WidgetID %>.SetSelected(this);return false;">Update</a></li>
                <li><a id="<%= WidgetID %>_TwFriends" href="javascript:void(0)" onclick="Tw_<%= WidgetID %>.SetSelected(this)">Friends</a></li>
                <li><a id="<%= WidgetID %>_TwArchive" href="javascript:void(0)" onclick="Tw_<%= WidgetID %>.SetSelected(this)">Archive</a></li>
                <li><a id="<%= WidgetID %>_TwPublic" href="javascript:void(0)" onclick="Tw_<%= WidgetID %>.SetSelected(this)">Public</a></li>
            </ul>
        </div>
        <div id="<%= WidgetID %>_TwView" class="twitterView"></div>
        <div id="<%= WidgetID %>_TwUpdatePanel" class="twitterUpdate" style="display:none">
            <textarea id="<%= WidgetID %>_TwUpdateText" rows="4" cols="" onkeyup="Tw_<%= WidgetID %>.CheckLength(this)"></textarea>
            <input type="button" id="<%= WidgetID %>_TwUpdateBtn" onclick="Tw_<%= WidgetID %>.Update()" value="Update" />
            <span id="<%= WidgetID %>_TwUpdateLeft" class="twitterCCount">140 characters left</span>
        </div>
    </div>
    <div id="<%= WidgetID %>_Progress" class="twitterProgress">
        Loading from Twitter...
    </div>
</div>
