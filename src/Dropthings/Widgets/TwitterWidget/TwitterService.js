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
TwitterService.UpdateStaus= function(username,password,updateText,onSuccess,onFailed,userContext) {TwitterService._staticInstance.UpdateStaus(username,password,updateText,onSuccess,onFailed,userContext); }
