var TwitterService=function() {
TwitterService.initializeBase(this);
this._timeout = 0;
this._userContext = null;
this._succeeded = null;
this._failed = null;
}
TwitterService.prototype={
_get_path:function() {
 var p = this.get_path();
 if (p) return p;
 else return TwitterService._staticInstance.get_path();},
GetPublicStatuses:function(succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'GetPublicStatuses',false,{},succeededCallback,failedCallback,userContext); },
VerifyCredentials:function(username,password,succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'VerifyCredentials',false,{username:username,password:password},succeededCallback,failedCallback,userContext); },
GetUserStatuses:function(username,password,succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'GetUserStatuses',false,{username:username,password:password},succeededCallback,failedCallback,userContext); },
GetFriendStatuses:function(username,password,succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'GetFriendStatuses',false,{username:username,password:password},succeededCallback,failedCallback,userContext); },
UpdateStaus:function(username,password,updateText,succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'UpdateStaus',false,{username:username,password:password,updateText:updateText},succeededCallback,failedCallback,userContext); }}
TwitterService.registerClass('TwitterService',Sys.Net.WebServiceProxy);
TwitterService._staticInstance = new TwitterService();
TwitterService.set_path = function(value) { TwitterService._staticInstance.set_path(value); }
TwitterService.get_path = function() { return TwitterService._staticInstance.get_path(); }
TwitterService.set_timeout = function(value) { TwitterService._staticInstance.set_timeout(value); }
TwitterService.get_timeout = function() { return TwitterService._staticInstance.get_timeout(); }
TwitterService.set_defaultUserContext = function(value) { TwitterService._staticInstance.set_defaultUserContext(value); }
TwitterService.get_defaultUserContext = function() { return TwitterService._staticInstance.get_defaultUserContext(); }
TwitterService.set_defaultSucceededCallback = function(value) { TwitterService._staticInstance.set_defaultSucceededCallback(value); }
TwitterService.get_defaultSucceededCallback = function() { return TwitterService._staticInstance.get_defaultSucceededCallback(); }
TwitterService.set_defaultFailedCallback = function(value) { TwitterService._staticInstance.set_defaultFailedCallback(value); }
TwitterService.get_defaultFailedCallback = function() { return TwitterService._staticInstance.get_defaultFailedCallback(); }
TwitterService.set_path("/TwitterService.asmx");
TwitterService.GetPublicStatuses= function(onSuccess,onFailed,userContext) {TwitterService._staticInstance.GetPublicStatuses(onSuccess,onFailed,userContext); }
TwitterService.VerifyCredentials= function(username,password,onSuccess,onFailed,userContext) {TwitterService._staticInstance.VerifyCredentials(username,password,onSuccess,onFailed,userContext); }
TwitterService.GetUserStatuses= function(username,password,onSuccess,onFailed,userContext) {TwitterService._staticInstance.GetUserStatuses(username,password,onSuccess,onFailed,userContext); }
TwitterService.GetFriendStatuses= function(username,password,onSuccess,onFailed,userContext) {TwitterService._staticInstance.GetFriendStatuses(username,password,onSuccess,onFailed,userContext); }
TwitterService.UpdateStaus = function(username, password, updateText, onSuccess, onFailed, userContext) { TwitterService._staticInstance.UpdateStaus(username, password, updateText, onSuccess, onFailed, userContext); }

var Twitter = function() {
    this._selectedOption = null;
    this._currentPage = 1;
    this._pagingEnable = false;
    this._currentResponse = null;
    this._loggedIn = false;
    this._pageCount = 1;
    this._widgetState = null;
    this._uName = null;
    this._uPass = null;
    this._widgetID = null;
    this.TIME_ENUM = { MSECOND: 1000, SECOND: 60, MINUTE: 60, HOUR: 24 };
};
Twitter.prototype = {
    ResetTabs: function() {
        $('#W' + this._widgetID + '_MainView .twitterTabs li').removeClass('selected');
    },
    SetSelected: function(el) {
        if (typeof (el) != 'object') el = $('#' + el);
        if (this._selectedOption == $(el).html()) return;
        this._selectedOption = $(el).html();
        $('#W' + this._widgetID + '_TwUpdateError').html('').css('display', 'none');
        var wgt = this;
        if (!this._loggedIn) {
            if (this._selectedOption != 'Public') {
                this._selectedOption = 'Public';
                if ($('#W' + this._widgetID + '_TwFeatures').css('display') != 'none') return;
                $('#W' + this._widgetID + '_TwFeatures').slideDown();
                $('#W' + this._widgetID + '_TwFeatures a').click(function() { wgt.ShowSettings(); });
                $('#W' + this._widgetID + '_TwFeatClose').click(function() { $('#W' + wgt._widgetID + '_TwFeatures').slideUp(); });
                return;
            }
        }
        this.ResetTabs();
        $(el).parent().addClass('selected');
        $('#W' + this._widgetID + '_TwView').html('');
        if (this._selectedOption != 'Update') {
            $('#W' + this._widgetID + '_TwView').css('display', '');
            $('#W' + this._widgetID + '_TwUpdatePanel').css('display', 'none');
        }
        else {
            $('#W' + this._widgetID + '_TwUpdateBtn').disabled = false;
        }
        $('#W' + this._widgetID + '_TwFeatures').css('display', 'none');
        switch (this._selectedOption) {
            case 'Update':
                {
                    $('#W' + this._widgetID + '_TwView').css('display', 'none');
                    $('#W' + this._widgetID + '_TwUpdateError').html('').css('display', 'none');
                    $('#W' + this._widgetID + '_TwUpdatePanel').css('display', '');
                    $('#W' + this._widgetID + '_TwUpdateText').css('width', (($('#W' + this._widgetID + '_TwUpdatePanel').width() - 10) + 'px'));
                    break;
                }
            case 'Friends': this.LoadFriendsTimeline(); break;
            case 'Public': this.LoadPublicTimeline(); break;
            case 'Archive': this.LoadMyTimeline(); break;
        }
    },
    Update: function() {
        $('#W' + this._widgetID + '_TwUpdateBtn').attr('disabled', 'true');
        var wgt = this;
        var wgtId = this._widgetID;
        wgt.ShowProgress('Updating...');
        TwitterService.UpdateStaus(this._uName, this._uPass, $('#W' + this._widgetID + '_TwUpdateText').val(), function(resp) {
            $('#W' + wgtId + '_TwUpdateBtn').removeAttr('disabled');
            $('#W' + wgtId + '_TwUpdateText').val('');
            if (resp.toString().indexOf("error") > -1) $('#W' + wgtId + '_TwUpdateError').html('Update failed. Please check your credentials.').css('display', '');
            else wgt.SetSelected('W' + wgtId + '_TwFriends');
        },
        function(msg) {
            $('#W' + wgtId + '_TwUpdateBtn').removeAttr('disabled');
            if (resp.toString().indexOf("error") > -1) $('#W' + wgtId + '_TwUpdateError').html('Update failed:' + msg).css('display', '');
        });
    },
    HideSettings: function() {
        $('#W' + this._widgetID + '_SettingsPanel').css('display', 'none');
        $('#W' + this._widgetID + '_Content').css('display', '');
    },
    ShowSettings: function() {
        $('#W' + this._widgetID + '_SettingsPanel').css('display', '');
        $('#W' + this._widgetID + '_Content').css('display', 'none');
    },
    ViewDisplay: function(items) {
        $('#W' + this._widgetID + '_MainView').css('display', 'block');
        $('#W' + this._widgetID + '_Progress').css('display', 'none');
        $('#W' + this._widgetID + '_TwUpdateError').html('').css('display', 'none');
        $('#W' + this._widgetID + '_TwView').html('');
        var mainDiv;
        var dataDiv;
        var wgt = this;

        if (!this._loggedIn) {
            $('<div/>').addClass('twitterLogin').html('Please <a href="javascript:void(0)">sign in</a> to view & update').click(function() { wgt.ShowSettings(); }).appendTo('#W' + this._widgetID + '_TwView');
        }

        $.each(items, function(i, item) {
            mainDiv = 'u_' + item.user.id + '_' + i;
            dataDiv = 'd_' + item.user.id + '_' + i;
            if (i % 2) {
                $('<div/>').attr('id', mainDiv).addClass('even').appendTo('#W' + wgt._widgetID + '_TwView');
            }
            else {
                $('<div/>').attr('id', mainDiv).addClass('odd').appendTo('#W' + wgt._widgetID + '_TwView');
            }
            $('<img/>').attr('src', item.user.profile_image_url).attr('alt', item.user.screen_name).attr('title', item.user.screen_name).appendTo('#' + mainDiv);
            $('<div/>').attr('id', dataDiv).appendTo('#' + mainDiv);
            $('<a/>').attr('href', ((item.user.url != null) ? item.user.url : 'http://twitter.com/' + item.user.screen_name)).attr('target', '_blank').addClass('twitterSName').html(item.user.screen_name).appendTo('#' + dataDiv);

            $('<div/>').html(wgt.ProcessTweet(item.text)).appendTo('#' + dataDiv);

            $('<div/>').addClass('twTime').html(wgt.FormatDateDiff(item.created_at) + ' ago').appendTo('#' + dataDiv);
        });
        if (this._pagingEnable) {
            if (this._currentPage != this._pageCount) $('<a/>').html('Next >').addClass('nextLink').attr('href', 'javascript:void(0)').click(function() {
                wgt._currentPage++;
                wgt.SetPageItems();
            }).appendTo('#W' + this._widgetID + '_TwView');
            if (this._currentPage != 1) $('<a/>').html('< Previous').addClass('prevLink').attr('href', 'javascript:void(0)').click(function() {
                wgt._currentPage--;
                wgt.SetPageItems();
            }).appendTo('#W' + this._widgetID + '_TwView');
        }
    },
    ProcessTweet: function(text) {
        var temp = text; var end;
        while (temp.indexOf("http://") != -1) {
            var start = temp.indexOf("http://");
            var url = temp.substr(start);
            if (url.indexOf(" ") != -1) {
                url = url.substr(0, url.indexOf(" "));
            }
            end = this.StringFilter(url);
            url = url.substr(0, end);
            while (url.charAt(url.length - 1) == ".") {
                url = url.substr(0, url.length - 1);
            }
            while (url.charAt(url.length - 1) == "/") {
                url = url.substr(0, url.length - 1);
            }
            while (url.charAt(url.length - 1) == "-") {
                url = url.substr(0, url.length - 1);
            }
            temp = temp.substr(start + end);
            text = text.replace(url, '<a target="_blank" href="' + url + '">' + url + "</a>");
        }
        temp = text;
        while (temp.indexOf("https://") != -1) {
            var start = temp.indexOf("https://");
            var url = temp.substr(start);
            if (url.indexOf(" ") != -1) {
                url = url.substr(0, url.indexOf(" "));
            }
            end = this.StringFilter(url);
            url = url.substr(0, end);
            while (url.charAt(url.length - 1) == ".") {
                url = url.substr(0, url.length - 1);
            }
            while (url.charAt(url.length - 1) == "/") {
                url = url.substr(0, url.length - 1);
            }
            while (url.charAt(url.length - 1) == "-") {
                url = url.substr(0, url.length - 1);
            }
            temp = temp.substr(start + end);
            text = text.replace(url, '<a target="_blank" href="' + url + '">' + url + "</a>");
        }
        temp = text;
        while (temp.indexOf("@") != -1) {
            var start = temp.indexOf("@");
            var scrName = temp.substr(start);
            if (scrName.indexOf(" ") != -1) {
                scrName = scrName.substr(0, scrName.indexOf(" "));
            }
            end = this.StringFilter(scrName);
            scrName = scrName.substr(1, end);
            while (scrName.charAt(scrName.length - 1) == ".") {
                scrName = scrName.substr(0, scrName.length - 1);
            }
            while (scrName.charAt(scrName.length - 1) == "/") {
                scrName = scrName.substr(0, scrName.length - 1);
            }
            while (scrName.charAt(scrName.length - 1) == "-") {
                scrName = scrName.substr(0, scrName.length - 1);
            }
            temp = temp.substr(start + end);
            text = text.replace(scrName, '<a target="_blank" href="http://twitter.com/' + scrName + '">' + scrName + "</a>");
        }
        return text;
    },
    StringFilter: function(str) {
        var filteredValues = "()<>\\{}[]_!*|,;'\"";
        var i; var returnString = "";
        for (i = 0; i < str.length; i++) {
            var c = str.charAt(i);
            if (filteredValues.indexOf(c) != -1) {
                return i;
            }
        }
        return i;
    },
    FormatDateDiff: function(feedDate) {
        var dtSplit = feedDate.split(" ");
        var dtMain = dtSplit[0] + " " + dtSplit[1] + " " + dtSplit[2] + " " + dtSplit[3] + " UTC" + dtSplit[4] + " " + dtSplit[5];
        var fDate = new Date(dtMain);
        _instantTime = new Date();
        var dateDiff = _instantTime.getTime() - fDate.getTime();
        if (dateDiff < (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND)) {
            var seconds = Math.round(dateDiff / this.TIME_ENUM.MSECOND);
            if (seconds < 5) {
                return "less than 5 seconds";
            }
            else {
                return "less than " + seconds + " seconds";
            }
        }
        else {
            if ((dateDiff > (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND)) && (dateDiff < (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE))) {
                return "about " + (Math.round((dateDiff / (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND)))) + " minutes";
            }
            else {
                if ((dateDiff > (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE)) && (dateDiff < (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE * this.TIME_ENUM.HOUR))) {
                    return "about " + (Math.round((dateDiff / (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE)))) + " Hours";
                }
                else {
                    if ((dateDiff > (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE * this.TIME_ENUM.HOUR))) {
                        return "about " + (Math.round(dateDiff / (this.TIME_ENUM.MSECOND * this.TIME_ENUM.SECOND * this.TIME_ENUM.MINUTE * this.TIME_ENUM.HOUR))) + " days";
                    }
                }
            }
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
    ParseError: function(msg) {
        $('#W' + this._widgetID + '_TwUpdateError').html('Error: ' + msg).css('display', '');
    },
    LoadPublicTimeline: function() {
        this.ShowProgress('Loading public updates...');
        var wgt = this;
        TwitterService.GetPublicStatuses(function(resp) {
            wgt.ParseStatus(resp);
        },
        function(msg) {
            wgt.ParseError(msg);
        });
    },
    LoadFriendsTimeline: function() {
        this.ShowProgress('Loading friends updates...');
        var wgt = this;
        TwitterService.GetFriendStatuses(this._uName, this._uPass, function(resp) {
            wgt.ParseStatus(resp);
        },
        function(msg) {
            wgt.ParseError(msg);
        });
    },
    LoadMyTimeline: function() {
        this.ShowProgress('Loading my timeline...');
        var wgt = this;
        TwitterService.GetUserStatuses(this._uName, this._uPass, function(resp) {
            wgt.ParseStatus(resp);
        },
        function(msg) {
            wgt.ParseError(msg);
        });
    },
    ShowProgress: function(msg) {
        $('#W' + this._widgetID + '_Progress').html(msg).css('display', 'block');
    },
    HideProgress: function() {
        $('#W' + this._widgetID + '_Progress').css('display', 'none');
    },
    SuccessStateSave: function(msg) {
        // nothing to do
    },
    FailStateSave: function(err) {
        $('#W' + this._widgetID + '_TwUpdateError').html('Failed to save your credentials').css('display', '');
    },
    PrepareState: function() {
        return '<state><username>' + this._uName + '</username><password>' + this._uPass + '</password></state>';
    },
    VerifyCredentials: function() {
        if ($('#W' + this._widgetID + '_Content').css('display') == 'none') {
            $('#W' + this._widgetID + '_TwError').html('');
            $('#W' + this._widgetID + '_TwLoginProgress').css('display', '');
            $('#W' + this._widgetID + '_TwSave').attr('disabled', 'true');
            $('#W' + this._widgetID + '_TwCancel').attr('disabled', 'true');
            this._uName = $('#W' + this._widgetID + '_TwUsername').val();
            this._uPass = $('#W' + this._widgetID + '_TwPassword').val();
        }
        else {
            this._uName = this._widgetState.getElementsByTagName('state')[0].getElementsByTagName('username')[0].firstChild.nodeValue;
            this._uPass = this._widgetState.getElementsByTagName('state')[0].getElementsByTagName('password')[0].firstChild.nodeValue;
        }
        var wgt = this;
        var wgtId = this._widgetID;

        TwitterService.VerifyCredentials(this._uName, this._uPass, function(msg) {
            if ($('#W' + wgtId + '_Content').css('display') == 'none') {
                $('#W' + wgtId + '_TwSave').removeAttr('disabled');
                $('#W' + wgtId + '_TwCancel').removeAttr('disabled');
                $('#W' + wgtId + '_TwLoginProgress').css('display', 'none');
            }
            if (msg.toString().indexOf('error') < 0) {
                wgt._loggedIn = true;
                if ($('#W' + wgtId + '_Content').css('display') == 'none') {

                    Dropthings.Web.Framework.WidgetService.SaveWidgetState(wgt._widgetID, wgt.PrepareState(), function(msg) {
                        wgt.SuccessStateSave(msg);
                    },
                    function(err) {
                        wgt.FailStateSave(msg);
                    });
                }
                wgt.HideSettings();
                wgt.SetSelected('W' + wgtId + '_TwFriends');
            }
            else {
                if ($('#W' + wgtId + '_Content').css('display') == 'none') $('#W' + wgtId + '_TwLoginProgress').css('display', 'none');
                wgt.FailVerify(msg);
            }
        },
        function(msg) {
            if ($('#W' + wgtId + '_Content').css('display') == 'none') {
                $('#W' + wgtId + '_TwSave').removeAttr('disabled');
                $('#W' + wgtId + '_TwCancel').removeAttr('disabled');
                $('#W' + wgtId + '_TwLoginProgress').css('display', 'none');
            }
            wgt.FailVerify(msg);
        });
    },
    FailVerify: function(msg) {
        this._loggedIn = false;
        if ($('#W' + this._widgetID + '_Content').css('display') == 'none')
            $('#W' + this._widgetID + '_TwError').html('Login failed. Please check your credentials.');
        else
            this.SetSelected('W' + wgtId + '_TwPublic');
    },
    CheckLength: function(el) {
        if (typeof (el) != 'object') el = $('#' + el);
        var left = 140 - el.val().length;
        if (left < 0) {
            el.val(el.val().substring(0, 140));
            left--;
        }
        if (left < 0) left = 0;
        $('#W' + this._widgetID + '_TwUpdateLeft').html(left + ' characters left');
    },
    LoadNew: function() {
        this._loggedIn = false;
        this.SetSelected('W' + this._widgetID + '_TwPublic');
    },
    load: function(instanceId) {
        this._widgetID = instanceId;
        $('#Widget' + this._widgetID + '_EditWidget').attr('href', 'javascript:void(0)');
        $('#Widget' + this._widgetID + '_EditWidget').css('color', 'Gray');

        var wgt = this;
        $('#W' + this._widgetID + '_TwUpdate').click(function() {
            wgt.SetSelected('W' + wgt._widgetID + '_TwUpdate');
        });
        $('#W' + this._widgetID + '_TwFriends').click(function() {
            wgt.SetSelected('W' + wgt._widgetID + '_TwFriends');
        });
        $('#W' + this._widgetID + '_TwArchive').click(function() {
            wgt.SetSelected('W' + wgt._widgetID + '_TwArchive');
        });
        $('#W' + this._widgetID + '_TwPublic').click(function() {
            wgt.SetSelected('W' + wgt._widgetID + '_TwPublic');
        });

        $('#W' + this._widgetID + '_TwUpdateBtn').click(function() {
            wgt.Update();
        });

        $('#W' + this._widgetID + '_TwUpdateText').keyup(function() {
            wgt.CheckLength('W' + wgt._widgetID + '_TwUpdateText');
        });

        $('#W' + this._widgetID + '_TwSave').click(function() {
            wgt.VerifyCredentials();
        });
        $('#W' + this._widgetID + '_TwCancel').click(function() {
            wgt.HideSettings();
        });

        $('#W' + this._widgetID + '_TwPassword').keypress(function(event) {
            if (event.keyCode == 13)
                $('#W' + wgt._widgetID + '_TwSave').click();
        });

        Dropthings.Web.Framework.WidgetService.GetWidgetState(this._widgetID, function(stateXml) {
            wgt._widgetState = Utility.getXMLDocument(stateXml);
            try {
                if (wgt._widgetState.getElementsByTagName('state')[0].getElementsByTagName('username')[0].firstChild.nodeValue != '')
                    wgt.VerifyCredentials();
                else
                    wgt.LoadNew();
            }
            catch (e) {
                wgt.LoadNew();
            }
        },
        function(msg) {
            wgt.LoadNew();
        });
    }
};
