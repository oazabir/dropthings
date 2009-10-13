======================================
Dropthings v.1.7.0
(C) Omar AL Zabir. All rights reserved.
======================================


How to run the project:

1) Install Visual Studio 2008 (Web Developer Express edition will also work)
2) Install SQL Server 2005 Express unless you haven't done so with Visual Studio 2008.
3) Install Visual Studio 2008 SP1
http://www.microsoft.com/downloads/details.aspx?FamilyId=FBEE1648-7106-44A7-9649-6D9F6D58056E&displaylang=en
4) Install Microsoft® Silverlight™ Tools for Visual Studio 2008 SP1
http://www.microsoft.com/downloads/details.aspx?familyid=C22D6A7B-546F-4407-8EF6-D60C8EE221ED&displaylang=en
3) Load the solution Dropthings.sln
4) Hit run.

======================================

If you already have SQL Server 2005 (not the express one) then you need to do the following:

1) Attach the \Dropthings\App_Data\Dropthings.mdf file.
2) Run the following query to create a "dropthings" user in your SQL Server:


use master
go

Begin Try
CREATE LOGIN dropthings
WITH PASSWORD = 'dropthings',
CHECK_POLICY = OFF;
End Try
Begin Catch
PRINT 'User dropthings already in SQL Server, which is fine'
End Catch


USE Dropthings
go
Begin Try
CREATE USER dropthings;
End Try
Begin Catch
PRINT 'User dropthings already in Database, which is fine'
End Catch
go

sp_change_users_login 'auto_fix', 'dropthings'
GO




3) Change the web.config's connection string to:
<add name="DashboardConnectionString" connectionString="Data Source=.;Initial Catalog=dropthings;user id=dropthings;password=dropthings" providerName="System.Data.SqlClient"/>


The End.



Hosting Dropthings on IIS
=========================
* Open Dropthings.Silverlight project and change the path of the webservice in: ServiceReferences.ClientConfig to match your IIS host location. Currently it is set to "http://localhost:8000/WidgetService.asmx"
* Copy the Website to IIS root website. Do NOT copy to a Virtual Directory. It does not work well inside a virtual directory.
* Update the web.config and change all http://localhost:8000/ to your IIS host.
* Update the web.config