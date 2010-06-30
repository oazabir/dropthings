CREATE PROCEDURE [dbo].[DeleteWidgetCascade]
	(
	@WidgetId int,	
	@RESULT int OUT
	)	
AS
	/* SET NOCOUNT ON */
	delete from WidgetsInRoles where WidgetId = @widgetId
	delete from WidgetInstance where WidgetId = @widgetid
	delete from Widget where ID = @WidgetId
	SELECT @@ROWCOUNT
	
