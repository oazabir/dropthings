ALTER procedure [dbo].[Resurrection]
AS

-- This SP deletes the anonymous users from the database
-- and all associated records with it. Use this to setup a 
-- clean database or if you want to give all anonymous users
-- a fresh start.

declare @users as table (UserID uniqueidentifier)
insert into @users 
select userID from aspnet_Users where IsAnonymous = 1

delete from WidgetInstance 
	where WidgetZoneId IN (select Id from WidgetZone
		where ID in (select [Column].WidgetZoneId from [Column]
			where [Column].PageId IN (select Page.ID from Page where Page.UserId IN 
				(select UserId from @users)
			)
		)
	)

delete from [Column]
	where [Column].PageId IN (select Page.ID from Page where Page.UserId IN 
		(select UserId from @users)
	)
	
delete from WidgetZone
	where ID in (select [Column].WidgetZoneId from [Column]
		where [Column].PageId IN (select Page.ID from Page where Page.UserId IN 
			(select UserId from @users)
		)
	)

delete from UserSetting
	where UserId in (select UserId from @users)

delete from Page
	where UserId in (select UserId from @users)
	
delete from aspnet_profile
	where UserId in (select UserId from @users)
	
delete from aspnet_UsersInRoles 
	where UserId in (select UserId from @users)
	
delete from aspnet_users 
	where UserId in (select UserId from @users)

GO 