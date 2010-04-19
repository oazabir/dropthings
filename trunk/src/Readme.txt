======================================
Dropthings v.2.5.1
(C) Omar AL Zabir. All rights reserved.
======================================

How to run the project:

1) Install Visual Studio 2008 (Web Developer Express edition will also work)
2) Install SQL Server 2008 Express unless you haven't done so with Visual Studio 2008.
3) Install Visual Studio 2008 SP1
http://www.microsoft.com/downloads/details.aspx?FamilyId=FBEE1648-7106-44A7-9649-6D9F6D58056E&displaylang=en
4) Install Microsoft® Silverlight™ Tools for Visual Studio 2008 SP1
http://www.microsoft.com/downloads/details.aspx?familyid=C22D6A7B-546F-4407-8EF6-D60C8EE221ED&displaylang=en
3) Load the solution Dropthings.sln
4) Hit run.

======================================


======================================
Running on SQL Server 2008
======================================
If you already have SQL Server 2008 then you need to do the following:

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
<add name="DropthingsConnectionString" connectionString="Data Source=.;Initial Catalog=dropthings;user id=dropthings;password=dropthings" providerName="System.Data.SqlClient"/>

or if you are using SqlExpress

<add name="DropthingsConnectionString" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=dropthings;user id=dropthings;password=dropthings" providerName="System.Data.SqlClient"/>


=================================
Running on SQL Server 2005
=================================

The .mdf file that comes with Dropthings is now a SQL Server 2008 File and not accessible by SQL Server 2005 users.
Therefore if you want to use SQL Server 2005 use the CreateNewDatabase-SQL2005.sql script file in the App_Data directory to create your database (replaces step 1 above).
Then follow the other steps above (this method is not officially supported).
This file may be slightly out of date from the standard database.


=========================
Hosting Dropthings on IIS
=========================
* Open Dropthings.Silverlight project and change the path of the webservice in: ServiceReferences.ClientConfig to match your IIS host location. Currently it is set to "http://localhost:8000/WidgetService.asmx"
* Copy the Website to IIS root website or a virtual directory.
* Update the web.config and change all http://localhost:8000/ to your IIS host.
